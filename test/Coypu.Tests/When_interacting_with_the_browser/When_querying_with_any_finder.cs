using System.Linq;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_querying_with_any_finder : BrowserInteractionTests
    {
        [Fact]
        public void It_reports_that_a_findable_element_exists()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(),driver,new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);

            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.True(browserSession.FindId("Signout").Exists());
        }

        [Fact]
        public void It_reports_that_a_missing_element_does_not_exist()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null,
                new ThrowsWhenMissingButNoDisambiguationStrategy());
            
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.False(browserSession.FindId("Signin").Exists());
        }

        [Fact]
        public void It_reports_that_a_missing_element_is_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null, 
                new ThrowsWhenMissingButNoDisambiguationStrategy());
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

           
            Assert.True(browserSession.FindId("Signin").Missing());
        }

        [Fact]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, null);
            driver.StubId("Signout", new StubElement(), browserSession, sessionConfiguration);

            Assert.False(browserSession.FindId("Signout").Missing());
        }

        [Fact]
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
            Assert.False(firstQueryResult);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.True(secondQueryResult);
        }

        [Fact]
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
            Assert.True(firstQueryResult);

            var secondQueryResult = RunQueryAndCheckTiming(secondQuery);
            Assert.False(secondQueryResult);
        }
    }
}