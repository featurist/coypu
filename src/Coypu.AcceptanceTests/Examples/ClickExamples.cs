using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ClickExamples : WaitAndRetryExamples
    {
        [Test]
        public void Click_example()
        {
            var element = Browser.FindButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me"));

            element.Click();
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButton_example()
        {
            Browser.ClickButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickLink_example()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ClickLinkWithTitle_example()
        {
            Browser.ClickLink("Link with title");
            Browser.CancelModalDialog();
        }
    }
}