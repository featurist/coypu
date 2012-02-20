using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_css : DriverSpecs
    {
        [Test]
        public void Does_not_find_missing_examples()

        {
            const string shouldNotFind = "#inspectingContent p.css-missing-test";
            Assert.That(Driver.HasCss(shouldNotFind, Root), Is.False, "Expected not to find something at: " + shouldNotFind);
        }


        [Test]
        public void Finds_present_examples()

        {
            var shouldFind = "#inspectingContent p.css-test span";
            Assert.That(Driver.HasCss(shouldFind, Root), "Expected to find something at: " + shouldFind);

            shouldFind = "ul#cssTest li:nth-child(3)";
            Assert.That(Driver.HasCss(shouldFind, Root), "Expected to find something at: " + shouldFind);
        }


        [Test]
        public void Only_finds_visible_elements()

        {
            const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
            Assert.That(Driver.HasCss(shouldNotFind, Root), Is.False, "Expected not to find something at: " + shouldNotFind);
        }
    }
}