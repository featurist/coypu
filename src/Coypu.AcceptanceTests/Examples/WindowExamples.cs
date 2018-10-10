using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class WindowExamples : WaitAndRetryExamples
    {
        private object GetOuterHeight()
        {
            return Browser.ExecuteScript("return window.outerHeight;");
        }

        private object GetOuterWidth()
        {
            return Browser.ExecuteScript("return window.outerWidth;");
        }

        [Test]
        public void MaximiseWindow()
        {
            var availWidth = Browser.ExecuteScript("return window.screen.availWidth;");
            var initalWidth = GetOuterWidth();
            Assert.That(initalWidth, Is.LessThan(availWidth));

            Browser.MaximiseWindow();
            Assert.That(GetOuterWidth(), Is.GreaterThanOrEqualTo(availWidth));
        }

        [Test]
        public void RefreshingWindow()
        {
            var tickBeforeRefresh = (long) Browser.ExecuteScript("return window.SpecData.CurrentTick;");
            Browser.Refresh();
            var tickAfterRefresh = (long) Browser.ExecuteScript("return window.SpecData.CurrentTick;");
            Assert.That(tickAfterRefresh - tickBeforeRefresh, Is.GreaterThan(0));
        }

        [Test]
        public void ResizeWindow()
        {
            var initalWidth = GetOuterWidth();
            var initialHeight = GetOuterHeight();
            Assert.That(initalWidth, Is.Not.EqualTo(500));
            Assert.That(initialHeight, Is.Not.EqualTo(600));

            Browser.ResizeTo(500, 600);
            Assert.That(GetOuterWidth(), Is.EqualTo(500));
            Assert.That(GetOuterHeight(), Is.EqualTo(600));
        }

        [Test]
        public void WindowScoping_example()
        {
            var mainWindow = Browser;
            Assert.That(mainWindow.FindButton("scoped button", Options.First)
                                  .Id,
                        Is.EqualTo("scope1ButtonId"));
            mainWindow.ExecuteScript("setTimeout(function() {document.getElementById(\"openPopupLink\").click();}), 3000");
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