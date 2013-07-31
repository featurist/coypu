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
                Driver.FindWindow("popUpWindowName", Root).Text.should_contain("I am a pop up window");
                Driver.HasContent("Open pop up window", Root);
            }
        }

        [Test]
        public void Finds_by_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Driver.FindWindow("Pop Up Window", Root).Text.should_contain("I am a pop up window");
                Driver.HasContent("Open pop up window", Root);
            }
        }

        [Test]
        public void Finds_by_partial_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window 2", Root));
                Driver.FindWindow("Pop Up Window", Root).Text.should_contain("I am a pop up window 2");
                Driver.HasContent("Open pop up window 2", Root);
            }
        }

        [Test]
        public void Finds_by_exact_title_over_partial_title()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Driver.Click(Driver.FindLink("Open pop up window 2", Root));
                Driver.FindWindow("Pop Up Window", Root).Text.should_contain("I am a pop up window");
                Driver.HasContent("Open pop up window", Root);
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

                Assert.That(Driver.HasContent("I am a pop up window", popUp), Is.True);
                Assert.That(Driver.HasContent("I am a pop up window", Root), Is.False);
            }
        }

        [Test]
        public void Errors_on_no_such_window()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                Assert.Throws<MissingWindowException>(() => Driver.FindWindow("Not A Window", Root));
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
                Assert.Throws<MissingWindowException>(() => Driver.FindWindow("Open pop up window", Root));
            }
        }
    }
}