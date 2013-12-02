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
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Driver.FindWindows("popUpWindowName", Root).Text.should_contain("I am a pop up window");
                Driver.FindLink("Open pop up window",Root);
            }
        }

        [Test]
        public void Finds_by_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Driver.FindWindows("Pop Up Window", Root).Text.should_contain("I am a pop up window");
                Driver.FindLink("Open pop up window", Root);
            }
        }

        [Test]
        public void Finds_by_partial_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window 2", Root));
                Driver.FindWindows("Pop Up Window", Root).Text.should_contain("I am a pop up window 2");
                Driver.FindLink("Open pop up window 2", Root);
            }
        }

        [Test]
        public void Finds_by_exact_title_over_partial_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Driver.Click(Driver.FindLink("Open pop up window 2", Root));
                Driver.FindWindows("Pop Up Window", Root).Text.should_contain("I am a pop up window");
                Driver.FindLink("Open pop up window", Root);
            }
        }

        [Test]
        public void Finds_scoped_by_window()
        {
            using (Driver)
            {

                Driver.Click(Driver.FindLink("Open pop up window", Root));

                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root),
                                            Driver, null, null, null);

                Driver.FindId("popUpButtonId", popUp);
                Driver.FindLink("Open pop up window", Root);
            }
        }

        [Test]
        public void Errors_on_no_such_window()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Assert.Throws<MissingWindowException>(() => Driver.FindWindows("Not A Window", Root));
            }
        }

        [Test]
        public void Errors_on_window_closed()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root),
                                            Driver, null, null, null);

                Driver.ExecuteScript("self.close();", popUp);
                Assert.Throws<MissingWindowException>(() => Driver.FindWindows("Open pop up window", Root));
            }
        }
    }
}