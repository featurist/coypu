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

            var innerScope = session.FindSection("some section");
            var actualLink = innerScope.FindLink("some link").Now();

            Assert.That(actualLink, Is.SameAs(expectedLink));

        }
    }
}