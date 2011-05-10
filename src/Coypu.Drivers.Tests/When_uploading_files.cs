using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_uploading_files : DriverSpecs
	{
		internal override void Specs()
		{
			it["should set the path to be uploaded"] = () =>
			{
				var textField = driver.FindField("forLabeledFileFieldId");
				driver.Set(textField, @"c:\some\local.file");

				var findAgain = driver.FindField("forLabeledFileFieldId");
				findAgain.Value.should_be(@"c:\some\local.file");
			};
		}
	}
}