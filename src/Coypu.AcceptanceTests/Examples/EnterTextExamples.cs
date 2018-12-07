using System.IO;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class EnterTextExamples : WaitAndRetryExamples
    {
        [Test]
        public void FillIn_file_example()
        {
            const string someLocalFile = @"local.file";
            try
            {
                var directoryInfo = new DirectoryInfo(Path.GetTempPath());
                var fullPath = Path.Combine(directoryInfo.FullName, someLocalFile);
                using (File.Create(fullPath)) { }

                Browser.FillIn("forLabeledFileFieldId")
                       .With(fullPath);
                var findAgain = Browser.FindField("forLabeledFileFieldId");
                Assert.That(findAgain.Value, Does.EndWith(someLocalFile));
            }
            finally
            {
                File.Delete(someLocalFile);
            }
        }

        [Test]
        public void FillInWith_element_example()
        {
            Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                   .FillInWith("New text input value");
            Assert.That(Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void FillInWith_example()
        {
            Browser.FillIn("scope2ContainerLabeledTextInputFieldId")
                   .With("New text input value");
            Assert.That(Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void SendKeys_example()
        {
            Browser.FindField("containerLabeledTextInputFieldId")
                   .SendKeys(" - send these keys");
            Assert.That(Browser.FindField("containerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("text input field two val - send these keys"));
        }
    }
}