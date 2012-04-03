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
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(),driver,new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession);

            Assert.That(browserSession.FindLink("Sign out").Exists());
        }

        [Test]
        public void It_reports_that_a_missing_element_does_not_exist()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession);

            Assert.That(browserSession.FindLink("Sign in").Exists(), Is.EqualTo(false));
        }

        [Test]
        public void It_reports_that_a_missing_element_is_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession);

            Assert.That(browserSession.FindLink("Sign in").Missing());
        }

        [Test]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement(), browserSession);

            Assert.That(browserSession.FindLink("Sign out").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_checks_for_existing_elements_with_a_RobustQuery()
        {
            spyRobustWrapper.StubQueryResult(true,false);

            driver.StubLink("Sign out", new StubElement(), browserSession);
            browserSession.FindLink("Sign in").Exists();
            browserSession.FindLink("Sign out").Exists();

            var firstQuery = spyRobustWrapper.QueriesRan<bool>().ElementAt(0);
            var secondQuery = spyRobustWrapper.QueriesRan<bool>().ElementAt(1);

            RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQuery.Result, Is.False);

            RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQuery.Result, Is.True);
        }

        [Test]
        public void It_checks_for_missing_elements_with_a_RobustQuery()
        {
            spyRobustWrapper.StubQueryResult(true, false);

            driver.StubLink("Sign out", new StubElement(), browserSession);
            browserSession.FindLink("Sign in").Missing();
            browserSession.FindLink("Sign out").Missing();

            var firstQuery = spyRobustWrapper.QueriesRan<bool>().ElementAt(0);
            var secondQuery = spyRobustWrapper.QueriesRan<bool>().ElementAt(1);

            RunQueryAndCheckTiming(firstQuery);
            Assert.That(firstQuery.Result, Is.True);

            RunQueryAndCheckTiming(secondQuery);
            Assert.That(secondQuery.Result, Is.False);
        }
    }
}