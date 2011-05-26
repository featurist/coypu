using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_limiting_scope_to_a_fieldset : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_scope_before_executing_action()
        {
            const string locator = "Fieldset locator";

            var fieldset = new StubElement();
            driver.StubFieldset(locator, fieldset);

            Element actualScope = null;
            session.WithinFieldset(locator, () =>
            {
                actualScope = driver.Scope;
            });

            Assert.That(actualScope, Is.SameAs(fieldset));
        }

        [Test]
        public void It_clears_the_scope_after_executing_action()
        {
            const string locator = "Fieldset locator";

            var fieldset = new StubElement();
            driver.StubFieldset(locator, fieldset);

            session.WithinFieldset(locator, () => {});

            Assert.That(driver.Scope, Is.Null);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_action()
        {
            const string locator = "Fieldset locator";

            var fieldset = new StubElement();
            driver.StubFieldset(locator, fieldset);

            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.WithinFieldset(
                    locator, 
                    () => {throw new ExplicitlyThrownTestException("Exception from action within scope");}));

            Assert.That(driver.Scope, Is.Null);
        }
    }
}