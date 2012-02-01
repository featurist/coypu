using System;
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

            Assert.That(session.FindLink("Sign in").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_reports_that_a_findable_element_is_not_missing()
        {
            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), null, null, null);
            driver.StubLink("Sign out", new StubElement());

            Assert.That(session.FindLink("Sign out").Missing(), Is.EqualTo(false));
        }

        [Test]
        public void It_checks_for_missing_elements_with_a_ElementMissingQuery()
        {
            Assert.Fail("pending");
        }
        [Test]
        public void It_checks_for_existing_elements_with_an_ElementExiststQuery()
        {
            Assert.Fail("pending");
        }

    }
}