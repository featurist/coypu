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
            scope1 = new BrowserWindow(DefaultSessionConfiguration, new IdFinder(Driver, "scope1", Root, DefaultOptions), Driver,null,null,null,DisambiguationStrategy);
            scope2 = new BrowserWindow(DefaultSessionConfiguration, new IdFinder(Driver, "scope2", Root, DefaultOptions), Driver,null,null,null,DisambiguationStrategy);
        }

        [Test]
        public void Finds_button_by_name()
        {
            Button("scopedButtonName", scope1).Id.should_be("scope1ButtonId");
            Button( "scopedButtonName", scope2).Id.should_be("scope2ButtonId");
        }

        [Test]
        public void Finds_input_button_by_value()
        {
            Button( "scoped input button", scope1).Id.should_be("scope1InputButtonId");
            Button( "scoped input button", scope2).Id.should_be("scope2InputButtonId");
        }

        [Test]
        public void Finds_button_by_text()
        {
            Button( "scoped button", scope1).Id.should_be("scope1ButtonId");
            Button( "scoped button", scope2).Id.should_be("scope2ButtonId");
        }
    }
}