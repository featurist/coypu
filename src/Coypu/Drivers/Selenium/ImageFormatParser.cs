using System;
using System.Drawing.Imaging;
using System.IO;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ImageFormatParser
    {
        internal static ScreenshotImageFormat GetImageFormat(string fileName)
        {
            var extension = new FileInfo(fileName).Extension.ToLower().Replace("jpg", "jpeg");

            ScreenshotImageFormat format;
            if (AreEqual(extension, ScreenshotImageFormat.Bmp))
                format = ScreenshotImageFormat.Bmp;
            else if (AreEqual(extension, ScreenshotImageFormat.Gif))
                format = ScreenshotImageFormat.Gif;
            else if (AreEqual(extension, ScreenshotImageFormat.Jpeg))
                format = ScreenshotImageFormat.Jpeg;
            else if (AreEqual(extension, ScreenshotImageFormat.Png))
                format = ScreenshotImageFormat.Png;
            else if (AreEqual(extension, ScreenshotImageFormat.Bmp))
                format = ScreenshotImageFormat.Bmp;
            else
                format = ScreenshotImageFormat.Jpeg;

            return format;
        }

        private static bool AreEqual(string extension, ScreenshotImageFormat imageFormat)
        {
            return extension.Equals(imageFormat.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}