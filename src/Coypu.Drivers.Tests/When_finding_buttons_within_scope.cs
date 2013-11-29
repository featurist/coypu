using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons_within_scope : DriverSpecs
    {
        private DriverScope scope1;
        private DriverScope scope2;

        [SetUp]
        public void SetUpScope()
        {
            scope1 = new DriverScope(new SessionConfiguration(), new IdFinder(Driver, "scope1", Root), Driver,null,null,null);
            scope2 = new DriverScope(new SessionConfiguration(), new IdFinder(Driver, "scope2", Root), Driver,null,null,null);
        }

        [Test]
        public void Finds_button_by_name()
        {
            new ButtonFinder(Driver,"scopedButtonName", scope1).Find().Id.should_be("scope1ButtonId");
            new ButtonFinder(Driver, "scopedButtonName", scope2).Find().Id.should_be("scope2ButtonId");
        }

        [Test]
        public void Finds_input_button_by_value()
        {
            new ButtonFinder(Driver, "scoped input button", scope1).Find().Id.should_be("scope1InputButtonId");
            new ButtonFinder(Driver, "scoped input button", scope2).Find().Id.should_be("scope2InputButtonId");
        }

        [Test]
        public void Finds_button_by_text()
        {
            new ButtonFinder(Driver, "scoped button", scope1).Find().Id.should_be("scope1ButtonId");
            new ButtonFinder(Driver, "scoped button", scope2).Find().Id.should_be("scope2ButtonId");
        }
    }
}