using System;
using System.Drawing.Imaging;
using System.IO;

namespace Coypu.Drivers.Selenium
{
    internal class ImageFormatParser
    {
        internal static ImageFormat GetImageFormat(string fileName)
        {
            var extension = new FileInfo(fileName).Extension.ToLower().Replace("jpg", "jpeg");

            ImageFormat format;
            if      (AreEqual(extension, ImageFormat.Bmp))
                format = ImageFormat.Bmp;
            else if (AreEqual(extension, ImageFormat.Gif))
                format = ImageFormat.Gif;
            else if (AreEqual(extension, ImageFormat.Jpeg))
                format = ImageFormat.Jpeg;
            else if (AreEqual(extension, ImageFormat.Png))
                format = ImageFormat.Png;
            else if (AreEqual(extension, ImageFormat.Bmp))
                format = ImageFormat.Bmp;
            else
                format = ImageFormat.Jpeg;

            return format;
        }

        private static bool AreEqual(string extension, ImageFormat imageFormat)
        {
            return extension.Equals(imageFormat.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}