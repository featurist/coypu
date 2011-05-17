using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_scoping_interactions : BrowserInteractionTests
	{
        [Test]
        public void It_sets_the_scope_before_executing_action()
        {
            var expectedScope = new StubElement();
            Element actualScope = null;
            session.Within(() => expectedScope, () =>
            {
                actualScope = driver.Scope;
            });

            Assert.That(actualScope, Is.SameAs(expectedScope));
        }

        [Test]
        public void It_clears_the_scope_after_executing_action()
        {
            var expectedScope = new StubElement();
            session.Within(() => expectedScope, () => {});

            Assert.That(driver.Scope, Is.Null);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_action()
        {
            var expectedScope = new StubElement();

            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.Within(
                    () => expectedScope, 
                    () => {throw new ExplicitlyThrownTestException("Exception from action within scope");}));

            Assert.That(driver.Scope, Is.Null);
        }
	}
}