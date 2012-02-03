using Coypu.Queries;
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
            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement());

            Assert.That(session.FindLink("Sign out").Exists());
        }

        [Test]
        public void It_reports_that_a_missing_element_does_not_exist()
        {
            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement());

            Assert.That(session.FindLink("Sign in").Exists(), Is.EqualTo(false));
        }

        [Test]
        public void It_reports_that_a_missing_element_is_missing()
        {
            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement());

            Assert.That(session.FindLink("Sign in").Missing());
        }

        [Test]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement());

            Assert.That(session.FindLink("Sign out").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_checks_for_existing_elements_with_a_RobustQuery()
        {
            driver.StubLink("Sign out", new StubElement());
            session.FindLink("Sign in").Exists();
            session.FindLink("Sign out").Exists();

            var firstQuery = (ElementExistsQuery) spyRobustWrapper.QueriesRan[0];
            var secondQuery = (ElementExistsQuery) spyRobustWrapper.QueriesRan[1];

            firstQuery.Run();
            Assert.That(firstQuery.Result, Is.False);

            secondQuery.Run();
            Assert.That(secondQuery.Result, Is.True);
        }

        [Test]
        public void It_checks_for_missing_elements_with_a_RobustQuery()
        {
            driver.StubLink("Sign out", new StubElement());
            session.FindLink("Sign in").Missing();
            session.FindLink("Sign out").Missing();

            var firstQuery = (ElementMissingQuery)spyRobustWrapper.QueriesRan[0];
            var secondQuery = (ElementMissingQuery)spyRobustWrapper.QueriesRan[1];

            firstQuery.Run();
            Assert.That(firstQuery.Result, Is.True);

            secondQuery.Run();
            Assert.That(secondQuery.Result, Is.False);
        }
    }
}