using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using Awabot.Bot.Config;
using Awabot.Bot.Structures;
using Awabot.Core.Helpers;
using Awabot.src.Awabot.Bot.Structures;

using System.Linq;

namespace Awabot.Bot.Bot
{
    class AwakeningResolver
    {
        private ServerConfig _serverConfig { get; }

        public AwakeningResolver(ServerConfig serverConfig)
        {
            _serverConfig = serverConfig;

            string whitelistedCharactersString = new string(_serverConfig.WhitelistedCharacters.ToList().ToArray());

            const string UserWordsSuffix = "user-words";

            // Due to not being able to use the arguments to tesseract.exe to provide 
            // unicode non-english characters I write a new config each time as a workaround
            // Scuffed as fuck
            using (var sw = new StreamWriter(Globals.TesseractFolderName + "\\tessdata\\configs\\whitelisted_characters"))
            {
                sw.WriteLine("tessedit_char_whitelist " + whitelistedCharactersString);

                // sw.WriteLine("load_system_dawg F");
                // sw.WriteLine("load_freq_dawg F");

                // Tells tesseract the suffix on the custom dictionary file
                sw.WriteLine("user_words_suffix " + UserWordsSuffix);
            }

            HashSet<string> words = new HashSet<string>();

            foreach (var awake in serverConfig.AwakeTypes)
            {
                var wordsInAwake = awake.Text.Split(' ');
                foreach (var word in wordsInAwake)
                {
                    words.Add(word);
                }
            }

            foreach (var sentence in serverConfig.OcrIgnoredWords)
            {
                var wordsInIgnoredSentence = sentence.Split(' ');
                foreach (var word in wordsInIgnoredSentence)
                {
                    words.Add(word);
                }
            }

            using (var sw = new StreamWriter(Globals.TesseractFolderName + "\\tessdata\\" + serverConfig.Language + "." + UserWordsSuffix))
            {
                foreach (var word in words)
                {
                    sw.WriteLine(word);
                }
            }
        }

        private string GetText(Bitmap targetBitmap)
        {
            var tempImagePath = Environment.CurrentDirectory + "\\" + Globals.TesseractFolderName + "\\_temp.bmp";

            targetBitmap.Save(tempImagePath);

            ProcessStartInfo info = new ProcessStartInfo
            {
                WorkingDirectory = Environment.CurrentDirectory + "\\" + Globals.TesseractFolderName,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                FileName = "cmd.exe",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments =
                    "/c tesseract.exe " +
                    "\"" + tempImagePath + "\"" +
                    " stdout" +
                    " -l " + _serverConfig.Language +
                    " --psm 6 whitelisted_characters",

                StandardOutputEncoding = Encoding.UTF8
            };

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
        }

        public Bitmap SnapshotRectangle(Rectangle rectangle)
        {
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rectangle.Left, rectangle.Top,
                    0, 0, new Size(rectangle.Width, rectangle.Height), CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        /// <summary>
        /// Turns pixels of specified color into black and the other pixels into white.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public Bitmap DifferentiateAwakeText(Bitmap bitmap)
        {
            var requiredPixelColorsList = _serverConfig.AwakeTextPixelColorList;

            unsafe
            {
                ForeachPixel(bitmap, (x, y, pixel) =>
                {
                    bool matchedAnyRequiredPixels = false;

                    foreach (var item in requiredPixelColorsList)
                    {
                        var requiredPixelColors = item;

                        if (pixel->r >= requiredPixelColors[0].PixelColorMin && pixel->r <= requiredPixelColors[0].PixelColorMax &&
                            pixel->g >= requiredPixelColors[1].PixelColorMin && pixel->g <= requiredPixelColors[1].PixelColorMax &&
                            pixel->b >= requiredPixelColors[2].PixelColorMin && pixel->b <= requiredPixelColors[2].PixelColorMax)
                        {
                            matchedAnyRequiredPixels = true;
                        }
                    }

                    if (matchedAnyRequiredPixels)
                    {
                        pixel->r = 0;
                        pixel->g = 0;
                        pixel->b = 0;
                        pixel->a = 255;
                    }
                    else
                    {
                        pixel->r = 255;
                        pixel->g = 255;
                        pixel->b = 255;
                        pixel->a = 255;
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
                ErrorDisplayer.Error("A bitmap has made an attempt to clone an invalid bitmap " +
                    "rectangle or not enough memory was available." + Environment.NewLine + cropRectangle.ToString() + Environment.NewLine);
            }

            return null;
        }

        unsafe public delegate void PixelIterationCallback(int x, int y, Pixel* pPixel);

        unsafe public void ForeachPixel(Bitmap bitmap, PixelIterationCallback pixelCallback)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte* pBeg = (byte*)bmpData.Scan0.ToPointer();

            for (int y = 0; y < bmpData.Height; ++y)
            {
                for (int x = 0; x < bmpData.Width; ++x)
                {
                    // 24 / 8 is dependant on the pixelformat
                    byte* data = pBeg + y * bmpData.Stride + x * 32 / 8;

                    pixelCallback(x, y, (Pixel*)data);
                }
            }

            bitmap.UnlockBits(bmpData);
        }

        unsafe public delegate bool PixelIterationCallbackCheckBreak(int x, int y, Pixel* pPixel);

        unsafe public void ForeachPixelCheckBreak(Bitmap bitmap, PixelIterationCallbackCheckBreak pixelCallback)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            byte* pBeg = (byte*)bmpData.Scan0.ToPointer();

            bool shouldBreak = false;

            for (int y = 0; y < bmpData.Height; ++y)
            {
                for (int x = 0; x < bmpData.Width; ++x)
                {
                    // 24 / 8 is dependant on the pixelformat
                    byte* data = pBeg + y * bmpData.Stride + x * 32 / 8;

                    if (!pixelCallback(x, y, (Pixel*)data))
                    {
                        shouldBreak = true;
                        break;
                    }
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

        public Bitmap ResizeImage(Bitmap originalBitmap, Size newSize)
        {
            Rectangle newSizeRectangle = new Rectangle(Point.Empty, newSize);
            Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height, originalBitmap.PixelFormat);

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

                lastAwakeEndY = bottom;
            }

            return bitmaps;
        }
    }
}
