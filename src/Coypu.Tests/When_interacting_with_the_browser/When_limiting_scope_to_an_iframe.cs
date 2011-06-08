using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_limiting_scope_to_a_iframe : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_scope_before_executing_action()
        {
            const string locator = "IFrame locator";

            var iframe = new StubElement();
            driver.StubIFrame(locator, iframe);

            Element actualScope = null;
            session.WithinIFrame(locator, () =>
            {
                actualScope = driver.Scope;
            });

            Assert.That(actualScope, Is.SameAs(iframe));
        }

        [Test]
        public void It_clears_the_scope_after_executing_action()
        {
            const string locator = "IFrame locator";

            var iframe = new StubElement();
            driver.StubIFrame(locator, iframe);

            session.WithinIFrame(locator, () => {});

            Assert.That(driver.Scope, Is.Null);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_action()
        {
            const string locator = "IFrame locator";

            var iframe = new StubElement();
            driver.StubIFrame(locator, iframe);

            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.WithinIFrame(
                    locator, 
                    () => {throw new ExplicitlyThrownTestException("Exception from action within scope");}));

            Assert.That(driver.Scope, Is.Null);
        }
    }
}