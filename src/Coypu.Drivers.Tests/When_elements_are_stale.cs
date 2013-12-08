using System.Linq;
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
            var elementWithinScope1 = new FieldsetFinder(Driver, "Scope 1", Root, DefaultOptions).ResolveQuery();
            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_false();

            Driver.Click(new ButtonFinder(Driver, "empty scope1", Root, DefaultOptions).ResolveQuery());

            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_true();
        }

        [Test]
        public void Stale_element_became_invisible()
        {
            var elementWithinScope1 = new FieldsetFinder(Driver, "Scope 1", Root, DefaultOptions).ResolveQuery();
            elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = false }).should_be_false();

            Driver.Click(new ButtonFinder(Driver, "hide scope1", Root, DefaultOptions).ResolveQuery());

            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = false}).should_be_true();
            elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = true }).should_be_false();
        }

        [Test]
        public void Stale_frame()
        {
            var frame = Frame("iframe1");
            frame.Stale(new Options()).should_be_false();

            Driver.Click(new ButtonFinder(Driver, "destroy frames", Root, DefaultOptions).ResolveQuery());

            frame.Stale(new Options()).should_be_true();
        }

        [Test]
        public void Stale_frame_becomes_invisible()
        {
            var frame = Frame("iframe1");
            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_false();

            Driver.Click(new ButtonFinder(Driver, "hide frames", Root, DefaultOptions).ResolveQuery());

            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_true();
            frame.Stale(new Options { ConsiderInvisibleElements = true }).should_be_false();
        }

        [Test]
        public void Stale_window_closed()
        {
            Driver.Click(Link("Open pop up window"));

            var popUpScope = new DriverScope(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null);

            var popUpWindow = popUpScope.Now();
            popUpWindow.Stale(new Options()).should_be_false();

            Driver.Click(new ButtonFinder(Driver,"close", popUpScope, DefaultOptions).ResolveQuery());

            popUpWindow.Stale(new Options()).should_be_true();
        }
    }
}