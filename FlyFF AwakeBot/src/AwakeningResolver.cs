using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FlyFF_AwakeBot.Utils;
using Tesseract;
using System.Collections.Generic;
using FlyFF_AwakeBot.src;
using System.Linq;

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
        private TesseractEngine TessEngine { get; set; }
        private ServerConfigManager ConfigManager { get; }
        private AwakeningRoutines AwakeRoutines { get; }

        public AwakeningResolver(ServerConfigManager configManager, AwakeningRoutines awakeRoutines)
        {
            ConfigManager = configManager;
            AwakeRoutines = awakeRoutines;

            // Write the eng.user-words
            /*
                string dictionary = "";

                foreach (var awake in configManager.AwakeTypes)
                {
                    dictionary += awake.Text + "\n";
                }

                using (StreamWriter sw = new StreamWriter("tessdata\\eng.user-words"))
                {
                    sw.Write(dictionary);
                }
            */

            try
            {
                string characters = "";

                foreach (char c in ConfigManager.WhitelistedCharacters)
                    characters += c;

                var initVars = new Dictionary<string, object>() {
                     { "load_system_dawg", false },
                     { "load_freq_dawg", false },
                     // { "user_words_suffix", "user-words" }, // Load custom dictionary from eng.user-words
                    { "tessedit_char_whitelist", "+-%0123456789 " + characters },
                };

                TessEngine = new TesseractEngine("tessdata", ConfigManager.Language, EngineMode.Default, Enumerable.Empty<string>(), initVars, false)
                {
                    DefaultPageSegMode = PageSegMode.SingleBlock
                };
            }
            catch (Exception ex)
            {
                GeneralUtils.DisplayError(ex.ToString());
                Application.Exit();
            }
        }

        public void Dispose()
        {
            if (!TessEngine.IsDisposed)
                TessEngine.Dispose();
        }

        /// <summary>
        /// Converts an image taken of an awake into pure text.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public string GetAwakening(Bitmap bitmap)
        {
            using (var page = TessEngine.Process(bitmap))
            {
                return page.GetText();
            }
        }

        /// <summary>
        /// Takes a snapshot of the desktop of specified rectangle.
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public Bitmap SnapshotRectangle(Rectangle rectangle)
        {
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format24bppRgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rectangle.Left, rectangle.Top,
                    0, 0, new Size(rectangle.Width, rectangle.Height), CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        /// <summary>
        /// Resizes a bitmap into a new bitmap with preferred size.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public Bitmap ResizeImage(Bitmap bitmap, Size newSize)
        {
            Rectangle newSizeRectangle = new Rectangle(Point.Empty, newSize);
            Bitmap newBitmap = new Bitmap(newSize.Width, newSize.Height);

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
                    g.DrawImage(bitmap, newSizeRectangle, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, imgAttr);
                }
            }

            bitmap.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// Increases the size of an image in a way that makes the text more clear to tesseract.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public Bitmap IncreaseBitmapSize(Bitmap bitmap, float percentage)
        {
            if (percentage < 0)
                percentage = 0;

            float width = bitmap.Width;
            float height = bitmap.Height;

            int newWidth = (int)(width + width * ((double)(percentage) / 100));
            int newHeight = (int)(height + height * ((double)(percentage * 1.7) / 100)); // 1.7

            bitmap = ResizeImage(bitmap, new Size(newWidth, newHeight));

            return bitmap;
        }

        /// <summary>
        /// Converts a bitmap into the specified pixel formats.
        /// </summary>
        /// <param name="oldBitmap"></param>
        /// <param name="pxlFormat"></param>
        /// <returns></returns>
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
            Pixel requiredPixelColor = ConfigManager.AwakeTextPixelColor;

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
                using (Bitmap newBitmap = new Bitmap(bitmap))
                {
                    bitmap.Dispose();
                    return newBitmap.Clone(cropRectangle, newBitmap.PixelFormat);
                }
            }
            catch (OutOfMemoryException)
            {
                AwakeRoutines.CrossThreadAppendLog("A bitmap has made an attempt to clone an invalid bitmap " +
                    "rectangle or not enough memory was available.\n" + cropRectangle.ToString() + "\n");
            }

            Bitmap cleanBitmap = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(cleanBitmap))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, 100, 100));
            }

            return cleanBitmap;
        }

        unsafe public delegate void PixelIterationCallback(int x, int y, Pixel* pPixel);

        /// <summary>
        /// Highly optimized function that iterates through bitmap pixels with simplicity.
        /// Can most likely be optimized even more with while loop instead of two for loops.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="pixelCallback"></param>
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

        /// <summary>
        /// Highly optimized function that iterates through bitmap pixels with simplicity and supports breaking in the middle of looping.
        /// Can most likely be optimized even more with while loop instead of two for loops.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="pixelCallback"></param>
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
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public unsafe Bitmap CropBitmapSmart(Bitmap bitmap)
        {
            int bottom = 0;
            int top = 0;
            int right = 0;
            int left = bitmap.Width;

            int originalBmpWidth = bitmap.Width;
            int originalBmpHeight = bitmap.Height;

            // Determine bottom
            ForeachPixel(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != 255 && pixel->g != 255 && pixel->b != 255)
                {
                    if (y > bottom)
                        bottom = y;
                }
            });

            // Determine top
            ForeachPixelCheckBreak(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != 255 && pixel->g != 255 && pixel->b != 255)
                {
                    top = y;
                    return false;
                }

                return true;
            });

            top -= 30;

            if (top < 0)
                top = 0;

            bottom += 30;

            if (bottom == 30)
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
                if (pixel->r != 255 && pixel->g != 255 && pixel->b != 255)
                {
                    if (x > right)
                        right = x;
                }
            });

            // Determine left
            ForeachPixel(bitmap, (x, y, pixel) =>
            {
                if (pixel->r != 255 && pixel->g != 255 && pixel->b != 255)
                {
                    if (x < left)
                        left = x;
                }
            });

            left -= 30;
            right += 30;

            if (left < 0)
                left = 0;
            else if (left == originalBmpWidth)
                left = 0;

            if (right < left)
                right = originalBmpWidth;

            int newWidth = right - left;

            if (bitmap.Width < newWidth)
            {
                newWidth = bitmap.Width;
            }

            bitmap = CropBitmap(bitmap, new Rectangle(left, 0, newWidth, bitmap.Height));

            return bitmap;
        }
    }
}
