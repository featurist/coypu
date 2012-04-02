using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_elements_are_stale : DriverSpecs
    {
        [Test]
        public void Stale_element_removed_from_DOM()
        {
            var elementWithinScope1 = Driver.FindFieldset("Scope 1", Root);
            elementWithinScope1.Stale.should_be_false();

            Driver.Click(Driver.FindButton("empty scope1", Root));

            elementWithinScope1.Stale.should_be_true();
        }

        [Test]
        public void Stale_element_became_invisible()
        {
            var elementWithinScope1 = Driver.FindFieldset("Scope 1", Root);
            elementWithinScope1.Stale.should_be_false();

            Driver.Click(Driver.FindButton("hide scope1", Root));

            elementWithinScope1.Stale.should_be_true();
        }

        [Test]
        public void Stale_iframe()
        {
            var frame = Driver.FindIFrame("iframe1", Root);
            frame.Stale.should_be_false();

            Driver.Click(Driver.FindButton("destroy frames",Root));

            frame.Stale.should_be_true();
        }

        [Test]
        public void Stale_iframe_becomes_invisible()
        {
            var frame = Driver.FindIFrame("iframe1", Root);
            frame.Stale.should_be_false();

            Driver.Click(Driver.FindButton("hide frames", Root));

            frame.Stale.should_be_true();
        }

        [Test]
        public void Stale_window_closed()
        {
            Driver.Click(Driver.FindLink("Open pop up window", Root));

            var popUpScope = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);

            var popUpWindow = popUpScope.Now();
            popUpWindow.Stale.should_be_false();

            Driver.Click(Driver.FindButton("close", popUpScope));

            popUpWindow.Stale.should_be_true();
        }
    }
}