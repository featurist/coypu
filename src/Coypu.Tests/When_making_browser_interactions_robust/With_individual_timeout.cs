using System;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class With_individual_timeout 
    {
        [SetUp]
        public void SetUp() 
        {
            Configuration.Driver = typeof(FakeDriver);
        }

        [TearDown]
        public void TearDown() 
        {
            Browser.EndSession();
        }


        [Test]
        public void It_uses_individual_timeout_for_action_then_resets() 
        {
            var defaultTimeout = TimeSpan.FromSeconds(123);
            var individualTimeout = TimeSpan.FromSeconds(321);

            Configuration.Timeout = defaultTimeout;
            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));

            var spyRobustWrapper = new SpyRobustWrapper();
            var session = new Session(null, spyRobustWrapper, null, null, null);

            session.FindLink("bob").WithIndividualTimeout(individualTimeout).Now();

            Assert.Fail("Assert timeout used for query");
            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));
        }

        [Test]
        public void It_returns_through_within_individual_timeout()
        {
            var spyRobustWrapper = new SpyRobustWrapper();
            var expectedElement = new StubElement();
            spyRobustWrapper.AlwaysReturnFromRobustly(expectedElement);

            var session = new Session(null, spyRobustWrapper, null, null, null);

            var actualElement = session.FindLink("bob").WithIndividualTimeout(TimeSpan.FromSeconds(321)).Now();

            Assert.That(actualElement, Is.SameAs(expectedElement));
        }
    }
}