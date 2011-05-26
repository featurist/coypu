using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_limiting_scope_to_a_section : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_scope_before_executing_action()
        {
            const string locator = "Section locator";

            var section = new StubElement();
            driver.StubSection(locator, section);

            Element actualScope = null;
            session.WithinSection(locator, () =>
            {
                actualScope = driver.Scope;
            });

            Assert.That(actualScope, Is.SameAs(section));
        }

        [Test]
        public void It_clears_the_scope_after_executing_action()
        {
            const string locator = "Section locator";

            var section = new StubElement();
            driver.StubSection(locator, section);

            session.WithinSection(locator, () => {});

            Assert.That(driver.Scope, Is.Null);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_action()
        {
            const string locator = "Section locator";

            var section = new StubElement();
            driver.StubSection(locator, section);

            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.WithinSection(
                    locator, 
                    () => {throw new ExplicitlyThrownTestException("Exception from action within scope");}));

            Assert.That(driver.Scope, Is.Null);
        }
    }
}