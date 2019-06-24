// We do not use .net tesseract wrapper anymore because creator have not updated to latest version
// #define USE_NET_TESSERACT_WRAPPER

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FlyFF_AwakeBot.Utils;


namespace FlyFF_AwakeBot
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pixel
    {
        public byte b;
        public byte g;
        public byte r;
        public byte alpha;
    };

    enum PixelType
    {
        R,
        G,
        B
    }

    class AwakeningResolver : IDisposable
    {
        private ServerConfig ServerConfig { get; }

        public AwakeningResolver(ServerConfig serverConfig)
        {
            ServerConfig = serverConfig;

            // The .net tesseract code is deprectated because the creator has not updated their wrapper to tesseract 4
#if USE_NET_TESSERACT_WRAPPER
            try
            {
                string whitelistedCharsString = "";

                foreach (char c in _serverConfig.WhitelistedCharacters)
                    whitelistedCharsString += c;

                Dictionary<string, object> initialOptions = new Dictionary<string, object>() {
                    // Disable dictionary
                    { "load_system_dawg", "0" },
                    { "load_freq_dawg", "0" },

                    // // Disable the adaptive classifier, gives possibly inconsistent results on same image
                    { "classify_enable_learning", "0" },

                    { "tessedit_char_whitelist", "+-%0123456789 " + whitelistedCharsString },
                };

                TessEngine = new TesseractEngine("tessdata", _serverConfig.Language, EngineMode.Default, null, initialOptions, true)
                {
                    DefaultPageSegMode = PageSegMode.Auto
                };
            }
            catch (Exception ex)
            {
                Display.Error(ex.ToString());
                Application.Exit();
            }
#endif
        }

        public void Dispose()
        {
#if USE_NET_TESSERACT_WRAPPER
            if (!TessEngine.IsDisposed)
                TessEngine.Dispose();
#endif
        }

        private string GetText(Bitmap targetBitmap)
        {
            var tempImagePath = Environment.CurrentDirectory + "\\" + Globals.TesseractFolderName + "\\_temp.bmp";

            targetBitmap.Save(tempImagePath);

            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = Environment.CurrentDirectory + "\\" + Globals.TesseractFolderName;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = false;
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.Arguments =
                "/c tesseract.exe " +
                "\"" + tempImagePath + "\"" +
                " stdout" +
                " -l " + ServerConfig.Language +
                " --psm 6";

            using (Process process = Process.Start(info))
            {
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    // We earlier told tesseract to output in stdout, now we read that output
                    return process.StandardOutput.ReadToEnd().Replace("\r", "");
                }
                else
                {
                    string errorMessage = process.StandardError.ReadToEnd();
                    throw new Exception($"Tesseract failed with errorcode: {process.ExitCode} and the message: {errorMessage}");
                }
            }
        }

        public string GetAwakening(Bitmap bitmap)
        {
            return GetText(bitmap).TrimEnd('\f');

#if USE_NET_TESSERACT_WRAPPER
            using (var page = TessEngine.Process(bitmap))
            {
                return page.GetText();
            }
#endif
        }

        public Bitmap SnapshotRectangle(Rectangle rectangle)
        {
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format24bppRgb);

            bitmap.SetResolution(300, 300);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rectangle.Left, rectangle.Top,
                    0, 0, new Size(rectangle.Width, rectangle.Height), CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        [Obsolete("ResizeImage is deprecated, no longer needed.")]
        public Bitmap ResizeImage(Bitmap originalBitmap, Size newSize)
        {
            Rectangle newSizeRectangle = new Rectangle(Point.Empty, newSize);
            Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height, originalBitmap.PixelFormat);

            // Use 300 DPI because it's better for tesseract
            newBitmap.SetResolution(300, 300);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Required to remove the smooth black borders when an image has been resized
                using (var imgAttr = new ImageAttributes())
                {
                    imgAttr.SetWrapMode(WrapMode.TileFlipXY);
                    g.DrawImage(originalBitmap, newSizeRectangle, 0, 0, originalBitmap.Width, originalBitmap.Height, GraphicsUnit.Pixel, imgAttr);
                }
            }

            originalBitmap.Dispose();

            return newBitmap;
        }

        [Obsolete("DivideEachLineIntoBitmap is deprecated, no longer needed.")]
        unsafe public List<Bitmap> DivideEachLineIntoBitmap(Bitmap bitmap)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();

            int GetNextWhitePixelY(int startY)
            {
                int top_ = -1;

                // find first white pixel from top to bottom
                ForeachPixelCheckBreak(bitmap, (x, y, pixel) =>
                {
                    if (y >= startY)
                    {
                        if (pixel->r == 255 && pixel->g == 255 && pixel->b == 255)
                        {
                            top_ = y;
                            return false;
                        }
                    }

                    return true;
                });

                return top_;
            }

            int GetLastYContainingNoWhitePixel(int startY)
            {
                int bottom_ = 0;

                int lastRowChecked = 0;
                bool isCompletelyBlack = true;
                bool isFirstIteration = true;

                // find end of no white pixel on the whole row
                ForeachPixelCheckBreak(bitmap, (x, y, pixel) =>
                {
                    // if have have passed the top, now, we can start looking for the bottom of the line
                    if (y > startY)
                    {
                        // is the last row we checked is not same as this one, then we moved down to another line
                        if (lastRowChecked != y)
                        {
                            if (isCompletelyBlack && !isFirstIteration)
                            {
                                bottom_ = lastRowChecked;
                                // we break out of the loop
                                return false;
                            }
                            else
                            {
                                isFirstIteration = false;
                            }

                            isCompletelyBlack = true;
                        }

                        if (pixel->r != 0 || pixel->g != 0 || pixel->b != 0)
                        {
                            isCompletelyBlack = false;
                        }
                    }

                    lastRowChecked = y;

                    // continue looping
                    return true;
                });

                return bottom_;
            }

            int lastAwakeEndY = 0;

            while (true)
            {
                int top = GetNextWhitePixelY(lastAwakeEndY);

                // there are no more white pixels
                if (top == -1)
                    break;

                int bottom = GetLastYContainingNoWhitePixel(top);

                int padding = 5;

                Bitmap bitmapPart = bitmap.Clone(new Rectangle(0, top - padding, bitmap.Width, bottom - top + (padding * 2)), bitmap.PixelFormat);
                bitmaps.Add(bitmapPart);
                // bitmapPart.Save("bmppart" + lastAwakeEndY.ToString() + ".bmp");

                lastAwakeEndY = bottom;
            }

            return bitmaps;
        }

        [Obsolete("IncreaseBitmapSize is deprecated, no longer needed.")]
        public Bitmap IncreaseBitmapSize(Bitmap bitmap, float newDpi, float fromDpi)
        {
            float width = bitmap.Width;
            float height = bitmap.Height;

            float ratioX = newDpi / fromDpi;
            float ratioY = newDpi / fromDpi;

            float newWidth = width * ratioX;
            float newHeight = height * ratioY;

            bitmap = ResizeImage(bitmap, new Size((int)newWidth * 2, (int)newHeight * 4));

            return bitmap;
        }

        [Obsolete("ConvertBitmapPixelFormat is deprecated, no longer needed.")]
        public Bitmap ConvertBitmapPixelFormat(Bitmap oldBitmap, PixelFormat pxlFormat)
        {
            Bitmap newBitmap = new Bitmap(oldBitmap.Width, oldBitmap.Height, pxlFormat);

            using (Graphics g = Graphics.FromImage(oldBitmap))
            {
                g.DrawImage(oldBitmap, new Rectangle(0, 0, newBitmap.Width, newBitmap.Height));
            }

            return newBitmap;
        }

        /// <summary>
        /// Turns pixels of specified color into black and the other pixels into white.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public Bitmap DifferentiateAwakeText(Bitmap bitmap)
        {
            Pixel requiredPixelColor = ServerConfig.AwakeTextPixelColor;

            unsafe
            {
                ForeachPixel(bitmap, (x, y, pixel) =>
                {
                    if (pixel->r == requiredPixelColor.r &&
                    pixel->g == requiredPixelColor.g &&
                    pixel->b == requiredPixelColor.b)
                    {

                        pixel->r = 0;
                        pixel->g = 0;
                        pixel->b = 0;
                    }
                    else
                    {
                        pixel->r = 255;
                        pixel->g = 255;
                        pixel->b = 255;
                    }
                });
            }

            return bitmap;
        }

        /// <summary>
        /// Crops the bitmap into the specified rectangle
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="cropRectangle"></param>
        /// <returns></returns>
        public Bitmap CropBitmap(Bitmap bitmap, Rectangle cropRectangle)
        {
            try
            {
                return bitmap.Clone(cropRectangle, bitmap.PixelFormat);
            }
            catch (OutOfMemoryException)
            {
                Display.Error("A bitmap has made an attempt to clone an invalid bitmap " +
                    "rectangle or not enough memory was available." + Environment.NewLine + cropRectangle.ToString() + Environment.NewLine);
            }

            return null;
        }

        unsafe public delegate void PixelIterationCallback(int x, int y, Pixel* pPixel);

        unsafe public void ForeachPixel(Bitmap bitmap, PixelIterationCallback pixelCallback)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

            byte* pBeg = (byte*)bmpData.Scan0.ToPointer();

            for (int y = 0; y < bmpData.Height; ++y)
            {
                for (int x = 0; x < bmpData.Width; ++x)
                {
                    pixelCallback(x, y, (Pixel*)pBeg);
                    pBeg += sizeof(Pixel);
                }
            }

            bitmap.UnlockBits(bmpData);
        }

        unsafe public delegate bool PixelIterationCallbackCheckBreak(int x, int y, Pixel* pPixel);

        unsafe public void ForeachPixelCheckBreak(Bitmap bitmap, PixelIterationCallbackCheckBreak pixelCallback)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

            byte* pBeg = (byte*)bmpData.Scan0.ToPointer();

            bool shouldBreak = false;

            for (int y = 0; y < bmpData.Height; ++y)
            {
                for (int x = 0; x < bmpData.Width; ++x)
                {
                    if (!pixelCallback(x, y, (Pixel*)pBeg))
                    {
                        shouldBreak = true;
                        break;
                    }

                    pBeg += sizeof(Pixel);
                }

                if (shouldBreak)
                    break;
            }

            bitmap.UnlockBits(bmpData);
        }

        /// <summary>
        /// Crops the black text from the big white background and returns a smaller bitmap.
        /// A CHAOTIC FUNCTION THAT I DO NOT CARE TO CLEAN UP :)
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public unsafe Bitmap CropBitmapSmart(Bitmap bitmap)
        {
            int bottom = 0;
            int top = 0;
            int right = 0;
            int left = bitmap.Width;

            const int aroundPadding = 10;

            const int pixelColor = 255;

            int originalBmpWidth = bitmap.Width;
            int originalBmpHeight = bitmap.Height;

            // Determine bottom
            ForeachPixel(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != pixelColor && pixel->g != pixelColor && pixel->b != pixelColor)
                {
                    if (y > bottom)
                        bottom = y;
                }
            });

            // Determine top
            ForeachPixelCheckBreak(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != pixelColor && pixel->g != pixelColor && pixel->b != pixelColor)
                {
                    top = y;
                    return false;
                }

                return true;
            });

            top -= aroundPadding;

            if (top < 0)
                top = 0;

            bottom += aroundPadding;

            if (bottom == aroundPadding)
                bottom = originalBmpHeight;
            else if (bottom > originalBmpHeight)
                bottom = originalBmpHeight;

            int newHeight = bottom - top;

            if (bitmap.Height < newHeight)
            {
                newHeight = bitmap.Height;
            }

            bitmap = CropBitmap(bitmap, new Rectangle(0, top, bitmap.Width, newHeight));

            // Determine right
            ForeachPixel(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != pixelColor && pixel->g != pixelColor && pixel->b != pixelColor)
                {
                    if (x > right)
                        right = x;
                }
            });

            // Determine left
            ForeachPixel(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != pixelColor && pixel->g != pixelColor && pixel->b != pixelColor)
                {
                    if (x < left)
                        left = x;
                }
            });

            left -= aroundPadding;
            right += aroundPadding;

            if (left < 0)
                left = 0;
            else if (left == originalBmpWidth)
                left = 0;

            if (right < left)
                right = originalBmpWidth;

            int newWidth = right - left;

            if (bitmap.Width < (newWidth + left))
            {
                newWidth = bitmap.Width - left;
            }

            bitmap = CropBitmap(bitmap, new Rectangle(left, 0, newWidth, bitmap.Height));

            return bitmap;
        }

        [Obsolete("ApplyThreshhold is deprecated, no longer needed.")]
        public Bitmap ApplyThreshhold(Bitmap bitmap)
        {
            unsafe
            {
                ForeachPixel(bitmap, (x, y, pixel) =>
                {
                    var currentPixelColor = Color.FromArgb(pixel->r, pixel->g, pixel->b);
                    var currentPixelBrightness = currentPixelColor.GetBrightness();

                    if (currentPixelBrightness > 0.5f)
                    {
                        pixel->r = 255;
                        pixel->g = 255;
                        pixel->b = 255;
                    }
                    else
                    {
                        pixel->r = 0;
                        pixel->g = 0;
                        pixel->b = 0;
                    }
                });
            }

            return bitmap;
        }
    }
}
