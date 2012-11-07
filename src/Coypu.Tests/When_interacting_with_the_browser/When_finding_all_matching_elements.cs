using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_all_matching_elements : BrowserInteractionTests
    {
        [Test]
        public void FindAllCss_should_make_direct_call_to_underlying_driver()
        {
            Should_make_direct_call(browserSession.FindAllCss, driver.StubAllCss);
        }

        [Test]
        public void FindAllCss_wrap_each_element_found_in_a_snapshot_scope_with()
        {
            Should_wrap_elements_in_snapshot_scope(browserSession.FindAllCss, driver.StubAllCss);
        }

        [Test]
        public void FindAllXPath_should_make_direct_call_to_underlying_driver()
        {
            Should_make_direct_call(browserSession.FindAllXPath, driver.StubAllXPath);
        }

        [Test]
        public void FindAllXPath_wrap_each_element_found_in_a_snapshot_scope_with()
        {
            Should_wrap_elements_in_snapshot_scope(browserSession.FindAllXPath, driver.StubAllXPath);
        }

        protected void Should_make_direct_call(Func<string, Options, IEnumerable<SnapshotElementScope>> subject, Action<string, IEnumerable<ElementFound>, DriverScope> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var stubElements = new[] { new StubElement() };

            stub(locator, stubElements, browserSession);

            subject(locator,sessionConfiguration);

            Assert.That(spyRobustWrapper.NoQueriesRan, Is.True, "Expected no robust queries run");
        }

        private void Should_wrap_elements_in_snapshot_scope(Func<string, Options, IEnumerable<SnapshotElementScope>> subject, Action<string, IEnumerable<ElementFound>, DriverScope> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var stubElements = new[] {new StubElement(), new StubElement()};

            stub(locator, stubElements, browserSession);

            var actualImmediateResult = subject(locator, sessionConfiguration).ToArray();
            Assert.That(actualImmediateResult.Count(), Is.EqualTo(2));
            Assert.That(actualImmediateResult[0].Now(), Is.EqualTo(stubElements[0]));
            Assert.That(actualImmediateResult[1].Now(), Is.EqualTo(stubElements[1]));
        }
    }
}