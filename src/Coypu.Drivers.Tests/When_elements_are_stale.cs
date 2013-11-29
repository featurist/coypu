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
            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"empty scope1", Root).Find());

            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_true();
        }

        [Test]
        public void Stale_element_became_invisible()
        {
            var elementWithinScope1 = Driver.FindFieldset("Scope 1", Root);
            elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = false }).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"hide scope1", Root).Find());

            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = false}).should_be_true();
            elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = true }).should_be_false();
        }

        [Test]
        public void Stale_frame()
        {
            var frame = Driver.FindFrame("iframe1", Root);
            frame.Stale(new Options()).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"destroy frames",Root).Find());

            frame.Stale(new Options()).should_be_true();
        }

        [Test]
        public void Stale_frame_becomes_invisible()
        {
            var frame = Driver.FindFrame("iframe1", Root);
            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"hide frames", Root).Find());

            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_true();
            frame.Stale(new Options { ConsiderInvisibleElements = true }).should_be_false();
        }

        [Test]
        public void Stale_window_closed()
        {
            Driver.Click(Driver.FindLink("Open pop up window", Root));

            var popUpScope = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);

            var popUpWindow = popUpScope.Now();
            popUpWindow.Stale(new Options()).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"close", popUpScope).Find());

            popUpWindow.Stale(new Options()).should_be_true();
        }
    }
}