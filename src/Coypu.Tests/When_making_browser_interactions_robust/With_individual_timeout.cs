using System;
using System.Linq;
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
        public void It_uses_individual_timeout_for_query_leaving() 
        {
            var individualTimeout = TimeSpan.FromSeconds(321);

            var spyRobustWrapper = new SpyRobustWrapper();
            var session = new Session(null, spyRobustWrapper, null, null, null);

            session.FindLink("bob").WithTimeout(individualTimeout).Now();

            Assert.That(spyRobustWrapper.QueriesRan<Element>().Single().Timeout, Is.EqualTo(individualTimeout));
        }

        [Test]
        public void It_returns_through_within_individual_timeout()
        {
            var spyRobustWrapper = new SpyRobustWrapper();
            var expectedElement = new StubElement();
            spyRobustWrapper.AlwaysReturnFromRobustly(expectedElement);

            var session = new Session(null, spyRobustWrapper, null, null, null);

            var actualElement = session.FindLink("bob").WithTimeout(TimeSpan.FromSeconds(321)).Now();

            Assert.That(actualElement, Is.SameAs(expectedElement));
        }
    }
}