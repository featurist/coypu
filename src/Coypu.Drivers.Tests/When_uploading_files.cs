using System.IO;
using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_uploading_files : DriverSpecs
	{
		internal override void Specs()
		{
			it["sets the path to be uploaded"] = () =>
			{
				const string someLocalFile = @"local.file";
				try
				{
					var directoryInfo = new DirectoryInfo(".");
					var fullPath = Path.Combine(directoryInfo.FullName,someLocalFile);
					using (File.Create(fullPath)) { }

					var textField = driver.FindField("forLabeledFileFieldId");
					driver.Set(textField, fullPath);

					var findAgain = driver.FindField("forLabeledFileFieldId");
					findAgain.Value.should_be(fullPath);
				}
				finally
				{
					File.Delete(someLocalFile);
				}
			};
		}
	}
}