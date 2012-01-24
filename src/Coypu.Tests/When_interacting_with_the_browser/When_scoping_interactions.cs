using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_scoping_interactions : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_scope_on_the_driver() 
        {
            var expectedScope = new DriverScope();

            expectedScope.FindField("some field");

            Assert.That(driver.LastUsedScope, Is.SameAs(expectedScope));

        }

        [Test]
        public void It_clears_the_scope_afterwards() 
        {
            var expectedScope = new StubElement();

            expectedScope.FindField("some field");

            Assert.That(driver.Scope, Is.Null);
        }

        [Test]
        public void It_clears_the_scope_after_an_exception() 
        {
            var expectedScope = new StubElement();

            Assert.Throws<ExplicitlyThrownTestException>(
                () =>
                    {
                        expectedScope.FindField("some field")
                                     .Throws(new ExplicitlyThrownTestException("Exception from action within scope"));
                    });

            Assert.That(driver.Scope, Is.Null);
        }
    }
}