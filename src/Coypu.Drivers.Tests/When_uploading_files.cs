using System.IO;
using NSpec;
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
                    var fullPath = Path.Combine(directoryInfo.FullName,someLocalFile);
                    using (File.Create(fullPath)) { }

                    var textField = Driver.FindField("forLabeledFileFieldId", Root);
                    Driver.Set(textField, fullPath,false);

                    var findAgain = Driver.FindField("forLabeledFileFieldId", Root);
                    findAgain.Value.should_end_with("\\" + someLocalFile);
                }
                finally
                {
                    File.Delete(someLocalFile);
                }
            }
        }
    }
