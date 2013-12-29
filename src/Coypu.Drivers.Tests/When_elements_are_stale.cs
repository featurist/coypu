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
            var elementWithinScope1 = Fieldset("Scope 1", Root, DefaultOptions);
            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_false();

            Driver.Click(Button("empty scope1", Root, DefaultOptions));

            elementWithinScope1.Stale(new Options{ConsiderInvisibleElements = true}).should_be_true();
        }

        [Test]
        public void Stale_element_became_invisible()
        {
            var elementWithinScope1 = Fieldset("Scope 1", Root, DefaultOptions);
            Assert.That(elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = false }), Is.False, "visible scope was stale");

            Driver.Click(Button("hide scope1"));

            Assert.That(elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = false }), Is.True, "invisible scope was NOT stale");
            Assert.That(elementWithinScope1.Stale(new Options { ConsiderInvisibleElements = true }), Is.False, "visible scope was stale");
        }

        [Test]
        public void Stale_frame()
        {
            var frame = Frame("iframe1");
            frame.Stale(new Options()).should_be_false();

            Driver.Click(Button("destroy frames", Root, DefaultOptions));

            frame.Stale(new Options()).should_be_true();
        }

        [Test]
        public void Stale_frame_becomes_invisible()
        {
            var frame = Frame("iframe1");
            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_false();

            Driver.Click(Button("hide frames", Root, DefaultOptions));

            frame.Stale(new Options { ConsiderInvisibleElements = false }).should_be_true();
            frame.Stale(new Options { ConsiderInvisibleElements = true }).should_be_false();
        }

        [Test]
        public void Stale_window_closed()
        {
            Driver.Click(Link("Open pop up window"));

            var popUpScope = new DriverScope(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy );

            var popUpWindow = popUpScope.Now();
            popUpWindow.Stale(new Options()).should_be_false();

            Driver.Click(Button("close", popUpScope, DefaultOptions));

            popUpWindow.Stale(new Options()).should_be_true();
        }
    }
}