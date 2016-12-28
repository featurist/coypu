using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_windows : DriverSpecs
    {
        [Test]
        public void Finds_by_name()
        {
            using (Driver)
            {
                OpenPopup();
                var window = Window("popUpWindowName", Root, DefaultOptions);

                window.Text.should_contain("I am a pop up window");

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
                FindPopUp().Text.should_contain("I am a pop up window");

                FindPopUpLink();
            }
        }

        [Test]
        public void Finds_by_substring_title()
        {
            using (Driver)
            {
                OpenPopup2();
                FindPopUp().Text.should_contain("I am a pop up window 2");
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
                FindPopUp().Text.should_contain("I am a pop up window");
                
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

                Driver.ExecuteScript("self.close();", popUp);
                Assert.Throws<MissingWindowException>(() => FindPopUp());
            }
        }
    }
}