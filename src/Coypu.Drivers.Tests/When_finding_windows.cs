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
                var window = new WindowFinder(Driver, "popUpWindowName", Root, DefaultOptions).ResolveQuery();

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

        private static ElementFound FindPopUpLink()
        {
            return new LinkFinder(Driver, "Open pop up window", Root, DefaultOptions).ResolveQuery();
        }

        private static ElementFound FindPopUp2Link()
        {
            return new LinkFinder(Driver, "Open pop up window 2", Root, DefaultOptions).ResolveQuery();
        }

        private static ElementFound FindPopUp()
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
        public void Finds_by_partial_title()
        {
            using (Driver)
            {
                OpenPopup2();
                FindPopUp().Text.should_contain("I am a pop up window 2");
                FindPopUp2Link();
            }
        }

        [Test]
        public void Finds_by_exact_title_over_partial_title()
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

                var popUp = new DriverScope(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null);

                new IdFinder(Driver, "popUpButtonId", popUp, DefaultOptions).ResolveQuery();

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

        private static ElementFound FindWindow(string locator)
        {
            return new WindowFinder(Driver, locator, Root, DefaultOptions).ResolveQuery();
        }

        [Test]
        public void Errors_on_window_closed()
        {
            using (Driver)
            {
                OpenPopup();
                var popUp = new DriverScope(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null);

                Driver.ExecuteScript("self.close();", popUp);
                Assert.Throws<MissingWindowException>(() => FindPopUp());
            }
        }
    }
}