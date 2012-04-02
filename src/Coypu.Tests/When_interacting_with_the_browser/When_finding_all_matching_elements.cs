using System;
using System.Collections.Generic;
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
        public void FindAllXPath_should_make_direct_call_to_underlying_driver()
        {
            Should_make_direct_call(browserSession.FindAllXPath, driver.StubAllXPath);
        }

        protected void Should_make_direct_call(Func<string, Options, IEnumerable<ElementFound>> subject, Action<string, IEnumerable<ElementFound>, DriverScope> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var expectedImmediateResult = new[] {new StubElement()};

            stub(locator, expectedImmediateResult, browserSession);

            var actualImmediateResult = subject(locator,SessionConfiguration);
            Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

            Assert.That(spyRobustWrapper.NoQueriesRan, Is.True, "Expected no robust queries run");
        }
    }
}