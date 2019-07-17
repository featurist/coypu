using System;
using System.IO;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ImageFormatParser
    {
        internal static ScreenshotImageFormat GetImageFormat(string fileName)
        {
            var extension = new FileInfo(fileName).Extension.ToLower()
                                                  .Replace("jpg", "jpeg");

            extension = extension.Replace(".", "");

            ScreenshotImageFormat format;
            if (AreEqual(extension, ScreenshotImageFormat.Bmp))
                format = ScreenshotImageFormat.Bmp;
            else if (AreEqual(extension, ScreenshotImageFormat.Gif))
                format = ScreenshotImageFormat.Gif;
            else if (AreEqual(extension, ScreenshotImageFormat.Jpeg))
                format = ScreenshotImageFormat.Jpeg;
            else
                format = ScreenshotImageFormat.Png;

            return format;
        }

        private static bool AreEqual(string extension,
                                     ScreenshotImageFormat imageFormat)
        {
            return extension.Equals(imageFormat.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}