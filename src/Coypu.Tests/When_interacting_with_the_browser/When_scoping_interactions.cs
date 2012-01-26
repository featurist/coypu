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
            var section = new StubElement();
            var expectedLink = new StubElement();
            driver.StubSection("some section",section);
            driver.StubLink("some link", expectedLink, section);

            var outerScope = new Session();
            var innerScope = outerScope.FindSection("some section");
            var actualLink = innerScope.FindLink("some link");

            Assert.That(actualLink, Is.SameAs(expectedLink));

        }

        [Test]
        public void It_clears_the_scope_afterwards() 
        {
            var section = new StubElement();
            var expectedLink = new StubElement();
            driver.StubSection("some section", section);
            driver.StubLink("some link", expectedLink, section);

            var outerScope = new Session();
            var innerScope = outerScope.FindSection("some section");
            
            innerScope.FindLink("some link");

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