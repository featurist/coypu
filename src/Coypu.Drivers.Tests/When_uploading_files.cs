using System.IO;
using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_uploading_files : DriverSpecs
    {
        [Test]
        public void Sets_the_path_to_be_uploaded()
        {
            const string someLocalFile = @"local.file";
            try
            {
                var directoryInfo = new DirectoryInfo(".");
                var fullPath = Path.Combine(directoryInfo.FullName, someLocalFile);
                using (File.Create(fullPath))
                {
                }

                var textField = Field("forLabeledFileFieldId");
                Driver.Set(textField, fullPath);

                var findAgain = Field("forLabeledFileFieldId");
                findAgain.Value.ShouldEndWith(someLocalFile);
            }
            finally
            {
                File.Delete(someLocalFile);
            }
        }
    }
}
