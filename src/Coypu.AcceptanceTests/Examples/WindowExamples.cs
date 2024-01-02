using System.Threading;
using Coypu.Drivers.Playwright;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class WindowExamples : WaitAndRetryExamples
    {
        private int GetOuterHeight()
        {
            return int.Parse(Browser.ExecuteScript("return window.outerHeight;").ToString());
        }

        private int GetOuterWidth()
        {
            return int.Parse(Browser.ExecuteScript("return window.outerWidth;").ToString());
        }

        [Test]
        public void MaximiseWindow()
        {
            if (Browser.Driver is PlaywrightDriver)
            {
                Assert.Ignore("Playwright does not support window maximisation");
            }
            var availWidth = int.Parse(Browser.ExecuteScript("return window.screen.availWidth;").ToString());
            var initalWidth = GetOuterWidth();
            Assert.That(initalWidth, Is.LessThan(availWidth));

            Browser.MaximiseWindow();
            Assert.That(GetOuterWidth(), Is.GreaterThanOrEqualTo(availWidth));
        }

        [Test]
        public void RefreshingWindow()
        {
            var tickBeforeRefresh = long.Parse(Browser.ExecuteScript("return window.SpecData.CurrentTick;").ToString());
            Browser.Refresh();
            var tickAfterRefresh = long.Parse(Browser.ExecuteScript("return window.SpecData.CurrentTick;").ToString());
            Assert.That(tickAfterRefresh - tickBeforeRefresh, Is.GreaterThan(0));
        }

        [Test]
        public void ResizeWindow()
        {
            var initalWidth = GetOuterWidth();
            var initialHeight = GetOuterHeight();
            Assert.That(initalWidth, Is.Not.EqualTo(500));
            Assert.That(initialHeight, Is.Not.EqualTo(600));

            Browser.ResizeTo(520, 600);
            Assert.That(GetOuterWidth(), Is.EqualTo(520));
            Assert.That(GetOuterHeight(), Is.EqualTo(600));
        }

        [Test]
        public void WindowScoping_example()
        {
            var mainWindow = Browser;
            Assert.That(mainWindow.FindButton("scoped button", Options.First)
                                  .Id,
                        Is.EqualTo("scope1ButtonId"));
            mainWindow.ExecuteScript("setTimeout(function() {document.getElementById(\"openPopupLink\").click();}, 300)");
            var popUp = mainWindow.FindWindow("Pop Up Window");

            Assert.That(popUp.FindButton("scoped button")
                             .Id,
                        Is.EqualTo("popUpButtonId"));
            // And back
            Assert.That(mainWindow.FindButton("scoped button", Options.First)
                                  .Id,
                        Is.EqualTo("scope1ButtonId"));
        }
    }
}
