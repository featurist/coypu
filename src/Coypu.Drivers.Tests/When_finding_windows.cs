using Coypu.Finders;
using Shouldly;
using NUnit.Framework;
using Coypu.Drivers.Playwright;
using Coypu.Timing;
using Coypu.Actions;
using System;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_windows : DriverSpecs
    {
        [Test]
        public void Finds_by_name()
        {
            using (Driver)
            {
                if (Driver is PlaywrightDriver)
                {
                    Assert.Ignore("Playwright does not seem to support window names");
                }
                OpenPopup();
                var window = Window("popUpWindowName", Root, DefaultOptions);

                window.Text.ShouldContain("I am a pop up window");

                FindPopUpLink();
            }
        }

        private static void OpenPopup()
        {
            Driver.Click(FindPopUpLink());
        }

        private static void OpenPopup2()
        {
            Driver.Click(FindPopUp2Link());
        }

        private static Element FindPopUpLink()
        {
            return Link("Open pop up window", Root, DefaultOptions);
        }

        private static Element FindPopUp2Link()
        {
            return Link("Open pop up window 2", Root, DefaultOptions);
        }

        private static Element FindPopUp()
        {
            return FindWindow("Pop Up Window");
        }

        [Test]
        public void Finds_by_title()
        {
            using (Driver)
            {
                OpenPopup();
                Retry(() => FindPopUp().Text.ShouldContain("I am a pop up window"));
                FindPopUpLink();
            }
        }

        [Test]
        public void Finds_by_substring_title()
        {
            using (Driver)
            {
                OpenPopup2();
                Retry(() => FindPopUp().Text.ShouldContain("I am a pop up window 2"));

                FindPopUp2Link();
            }
        }

        [Test]
        public void Finds_by_exact_title_over_substring()
        {
            using (Driver)
            {
                OpenPopup();
                OpenPopup2();
                Retry(() => FindPopUp().Text.ShouldContain("I am a pop up window"));
                FindPopUpLink();
            }
        }

        [Test]
        public void Finds_scoped_by_window()
        {
            using (Driver)
            {

                OpenPopup();

                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);
                Retry(popUp);
                Id("popUpButtonId", popUp);
                FindPopUpLink();
            }
        }

        [Test]
        public void Errors_on_no_such_window()
        {
            using (Driver)
            {
                OpenPopup();
                Assert.Throws<MissingWindowException>(() => FindWindow("Not A Window"));
            }
        }

        private static Element FindWindow(string locator)
        {
            return Window(locator, Root, DefaultOptions);
        }

        [Test]
        public void Errors_on_window_closed()
        {
            using (Driver)
            {
                OpenPopup();
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);

                Retry(popUp);
                // Playwright errors before returning from ExecuteScript if the window is closed synchronously
                Driver.ExecuteScript("window.setTimeout(() => self.close(), 1);", popUp);
                Assert.Throws<MissingWindowException>(() => FindPopUp());
            }
        }

        private void Retry(Scope popUp)
        {
            Retry(() => popUp.Now());
        }

        private void Retry(Action popUpAction)
        {
            new RetryUntilTimeoutTimingStrategy().Synchronise(
                new LambdaBrowserAction(popUpAction, DefaultOptions)
            );
        }
    }
}
