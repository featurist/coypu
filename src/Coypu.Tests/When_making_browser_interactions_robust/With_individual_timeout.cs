using System;
using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;
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

            var fakeDriver = new FakeDriver();
            fakeDriver.StubLink("bob",new StubElement());
            var session = new Session(fakeDriver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);

            session.FindLink("bob").WithIndividualTimeout(individualTimeout).Now();

            Assert.That(fakeDriver.LastUsedTimeout, Is.EqualTo(individualTimeout));
            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));
        }

        [Test]
        public void It_always_resets_after_action() 
        {
            var defaultTimeout = TimeSpan.FromSeconds(123);
            var individualTimeout = TimeSpan.FromSeconds(321);

            Configuration.Timeout = defaultTimeout;

            Action actionThatErrors = () => { throw new ExplicitlyThrownTestException("Error in individual timeout action"); };

            Assert.Throws<ExplicitlyThrownTestException>(() => Browser.Session.WithIndividualTimeout(individualTimeout, actionThatErrors));

            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));
        }

        [Test]
        public void It_uses_individual_timeout_for_function_then_resets() 
        {
            var defaultTimeout = TimeSpan.FromSeconds(123);
            var individualTimeout = TimeSpan.FromSeconds(321);

            Configuration.Timeout = defaultTimeout;
            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));

            var usedTimeout = default(TimeSpan);

            Browser.Session.WithIndividualTimeout(individualTimeout,
                                                  () =>
                                                      {
                                                          usedTimeout = Configuration.Timeout;
                                                          return new Object();
                                                      });

            Assert.That(usedTimeout, Is.EqualTo(individualTimeout));
            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));
        }

        [Test]
        public void It_always_resets_after_function() 
        {
            var defaultTimeout = TimeSpan.FromSeconds(123);
            var individualTimeout = TimeSpan.FromSeconds(321);

            Configuration.Timeout = defaultTimeout;

            Func<object> functionThatErrors = () => { throw new ExplicitlyThrownTestException("Error in individual timeout action"); };

            Assert.Throws<ExplicitlyThrownTestException>(() => Browser.Session.WithIndividualTimeout(individualTimeout, functionThatErrors));

            Assert.That(Configuration.Timeout, Is.EqualTo(defaultTimeout));
        }

        [Test]
        public void It_returns_from_within_individual_timeout()
        {
            var expectedResult = new Object();
            var actualResult = Browser.Session.WithIndividualTimeout(TimeSpan.Zero, () => expectedResult);

            Assert.That(actualResult, Is.SameAs(expectedResult));
        }
    }
}