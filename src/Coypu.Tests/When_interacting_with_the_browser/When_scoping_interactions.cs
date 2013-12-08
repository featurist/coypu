using Coypu.Tests.TestBuilders;
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
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeRobustWrapper(), fakeWaiter, null, stubUrlBuilder);
            var section = new StubElement();
            var expectedLink = new StubElement();
            driver.StubSection("some section", section, browserSession);

            var innerScope = browserSession.FindSection("some section");

            driver.StubLink("some link", expectedLink, innerScope);

            var actualLink = innerScope.FindLink("some link").Now();

            Assert.That(actualLink, Is.SameAs(expectedLink));

        }
    }
}