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

            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.That(browserSession.FindId("Signout").Exists());
        }

        [Test]
        public void It_reports_that_a_missing_element_does_not_exist()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null,
                new ThrowsWhenMissingButNoDisambiguationStrategy());
            
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.That(browserSession.FindId("Signin").Exists(), Is.EqualTo(false));
        }

        [Test]
        public void It_reports_that_a_missing_element_is_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null, 
                new ThrowsWhenMissingButNoDisambiguationStrategy());
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

           
            Assert.That(browserSession.FindId("Signin").Missing());
        }

        [Test]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.That(browserSession.FindId("Signout").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_checks_for_existing_elements_with_synchronise()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, SpyTimingStrategy, null, null, null,
                new ThrowsWhenMissingButNoDisambiguationStrategy());

            SpyTimingStrategy.StubQueryResult(true,false);

            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);
            browserSession.FindId("Signin").Exists();
            browserSession.FindId("Signout").Exists();

            var firstQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(0);
            var secondQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(1);

            var firstQueryResult = RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQueryResult, Is.False);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQueryResult, Is.True);
        }

        [Test]
        public void It_checks_for_missing_elements_with_synchronise()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(),driver, SpyTimingStrategy, null, null, null,
                new ThrowsWhenMissingButNoDisambiguationStrategy());

            SpyTimingStrategy.StubQueryResult(true, false);

            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);
            browserSession.FindId("Signin").Missing();
            browserSession.FindId("Signout").Missing();

            var firstQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(0);
            var secondQuery = SpyTimingStrategy.QueriesRan<bool>().ElementAt(1);

            var firstQueryResult = RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQueryResult, Is.True);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQueryResult, Is.False);
        }
    }
}