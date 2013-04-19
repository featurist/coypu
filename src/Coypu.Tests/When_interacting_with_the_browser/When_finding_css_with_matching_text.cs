using System;
using System.Text.RegularExpressions;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_css_with_matching_text : BrowserInteractionTests
    {
        [Test]
        public void FindCss_with_exact_text_should_make_robust_call_to_underlying_driver()
        {
            var input = "some text";
            var expectedPattern = new Regex("^some text$", RegexOptions.Multiline);
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss, input, expectedPattern);
        }

        [Test]
        public void FindCss_with_regex_should_make_robust_call_to_underlying_driver()
        {
            var input = new Regex("some.*text$");
            var expectedPattern = input;
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss, input, expectedPattern);
        }

        protected void Should_find_robustly<T>(Func<string, T, Options, Scope> subject, Func<string, T, Options, Scope> scope, Action<string, Regex, ElementFound, Scope> stub, T input, Regex expectedPattern)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            var expectedImmediateResult = new StubElement();
            var expectedDeferredResult = new StubElement();

            spyRobustWrapper.AlwaysReturnFromRobustly(expectedImmediateResult);

            stub(locator, expectedPattern, expectedDeferredResult, browserSession);
            stub(locator, expectedPattern, expectedDeferredResult, elementScope);

            var options = new Options{Timeout = individualTimeout};

            VerifyFoundRobustly(subject, 0, locator, input, expectedDeferredResult, expectedImmediateResult, options);

            if (scope != null)
                VerifyFoundRobustly(scope, 1, locator, input, expectedDeferredResult, expectedImmediateResult, options);
        }

        protected void VerifyFoundRobustly<T>(Func<string, T, Options, Scope> scope, int driverCallIndex, string locator, T text, StubElement expectedDeferredResult, StubElement expectedImmediateResult, Options options)
        {
            var scopedResult = scope(locator, text, options).Now();

            Assert.That(scopedResult, Is.Not.SameAs(expectedDeferredResult), "Result was not found robustly");
            Assert.That(scopedResult, Is.SameAs(expectedImmediateResult));

            var elementScopeResult = RunQueryAndCheckTiming<ElementFound>(options.Timeout, driverCallIndex);

            Assert.That(elementScopeResult, Is.SameAs(expectedDeferredResult));
        }
    }
}