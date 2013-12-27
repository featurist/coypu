using System.Linq;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_querying_with_any_finder : BrowserInteractionTests
    {
        [Test]
        public void It_reports_that_a_findable_element_exists()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(),driver,new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);

            Assert.That(browserSession.FindLink("Sign out").Exists());
        }

        [Test]
        public void It_reports_that_a_missing_element_does_not_exist()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);

            Assert.That(browserSession.FindLink("Sign in").Exists(), Is.EqualTo(false));
        }

        [Test]
        public void It_reports_that_a_missing_element_is_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);

            Assert.That(browserSession.FindLink("Sign in").Missing());
        }

        [Test]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);

            Assert.That(browserSession.FindLink("Sign out").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_checks_for_existing_elements_with_a_RobustQuery()
        {
            SpyTimingStrategy.StubQueryResult(true,false);

            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);
            browserSession.FindLink("Sign in").Exists();
            browserSession.FindLink("Sign out").Exists();

            var firstQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(0);
            var secondQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(1);

            var firstQueryResult = RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQueryResult, Is.False);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQueryResult, Is.True);
        }

        [Test]
        public void It_checks_for_missing_elements_with_a_RobustQuery()
        {
            SpyTimingStrategy.StubQueryResult(true, false);

            driver.StubLink("Sign out", new StubElement(), browserSession, Options.ExactTrue);
            browserSession.FindLink("Sign in").Missing();
            browserSession.FindLink("Sign out").Missing();

            var firstQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(0);
            var secondQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(1);

            var firstQueryResult = RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQueryResult, Is.True);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQueryResult, Is.False);
        }
    }
}