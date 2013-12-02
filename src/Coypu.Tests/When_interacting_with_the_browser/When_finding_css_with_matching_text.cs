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
        public void FindCss_with_partial_text_should_make_robust_call_to_underlying_driver()
        {
            var stubCssResult = new StubElement();
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss,
                                 input: "some text",
                                 matchingPattern: new Regex("some\\ text", RegexOptions.Multiline),
                                 expectedDeferredResult: stubCssResult,
                                 expectedImmediateResult: new StubElement(),
                                 stubCssResult: stubCssResult);
        }

        [Test]
        public void FindCss_should_excape_regex_chars()
        {
            var stubCssResult = new StubElement();
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss,
                                 input: "some (text) *",
                                 matchingPattern: new Regex("some\\ \\(text\\)\\ \\*", RegexOptions.Multiline),
                                 expectedDeferredResult: stubCssResult,
                                 expectedImmediateResult: new StubElement(),
                                 stubCssResult: stubCssResult);
        }

        [Test]
        public void FindCss_with_exact_text_should_use_exact_regex()
        {
            var stubCssResult = new StubElement();
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss,
                                 input: "some text",
                                 matchingPattern: new Regex("^some\\ text$", RegexOptions.Multiline),
                                 expectedDeferredResult: stubCssResult,
                                 expectedImmediateResult: new StubElement(),
                                 stubCssResult: stubCssResult,
                                 exact: true);
        }

        [Test]
        public void FindCss_with_regex_should_make_robust_call_to_underlying_driver()
        {
            var input = new Regex("some.*text$");
            var stubCssResult = new StubElement();
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss,
                                 input: input, 
                                 matchingPattern: input,
                                 expectedDeferredResult: stubCssResult,
                                 expectedImmediateResult: new StubElement(),
                                 stubCssResult: stubCssResult);
        }

        [Test]
        public void HasCss_queries_robustly_Positive_example()
        {
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: new StubElement());
        }

        [Test]
        public void HasCss_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: null);
        }

        [Test]
        public void HasCss_with_text_queries_robustly_Positive_example()
        {
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 suppliedText: "some text",
                                 matchingPattern: new Regex("some\\ text", RegexOptions.Multiline),
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: new StubElement());
        }

        [Test]
        public void HasCss_with_regex_queries_robustly_Positive_example()
        {
            var expectedPattern = new Regex("some.*text$");
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 suppliedText: expectedPattern,
                                 matchingPattern: expectedPattern,
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: new StubElement());
        }

        [Test]
        public void HasCss_with_text_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 suppliedText: "some text",
                                 matchingPattern: new Regex("^some other text$", RegexOptions.Multiline),
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: null);
        }

        [Test]
        public void HasCss_with_regex_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasCss, elementScope.HasCss, driver.StubCss,
                                 suppliedText: new Regex("some.*text$"),
                                 matchingPattern: new Regex("some. other *text$"),
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: null);
        }





        [Test]
        public void HasNoCss_queries_robustly_Positive_example()
        {
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: null);
        }

        [Test]
        public void HasNoCss_with_text_queries_robustly_Positive_example()
        {
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 suppliedText: "some text",
                                 matchingPattern: new Regex("^some other text$", RegexOptions.Multiline),
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: null);
        }

        [Test]
        public void HasNoCss_with_regex_queries_robustly_Positive_example()
        {
            var expectedPattern = new Regex("some.*text$");
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 suppliedText: expectedPattern,
                                 matchingPattern: expectedPattern,
                                 expectedDeferredResult: true,
                                 expectedImmediateResult: false,
                                 stubCssResult: null);
        }

        [Test]
        public void HasNoCss_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: new StubElement());
        }

        [Test]
        public void HasNoCss_with_text_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 suppliedText: "some text",
                                 matchingPattern: new Regex("some\\ text", RegexOptions.Multiline),
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: new StubElement());
        }

        [Test]
        public void HasNoCss_with_regex_queries_robustly_Negative_example()
        {
            Should_find_robustly(browserSession.HasNoCss, elementScope.HasNoCss, driver.StubCss,
                                 suppliedText: new Regex("some.*text$"),
                                 matchingPattern: new Regex("some.*text$"),
                                 expectedDeferredResult: false,
                                 expectedImmediateResult: true,
                                 stubCssResult: new StubElement());
        }

        protected void Should_find_robustly(Func<string, Options, bool> subject, Func<string, Options, bool> scope, Action<string, ElementFound, Scope> stub, bool expectedImmediateResult, bool expectedDeferredResult, ElementFound stubCssResult)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            spyRobustWrapper.AlwaysReturnFromRobustly(expectedImmediateResult);

            stub(locator, stubCssResult, browserSession);
            stub(locator, stubCssResult, elementScope);

            var options = new Options { Timeout = individualTimeout };

            VerifyFoundRobustly(subject, 0, locator, expectedDeferredResult, expectedImmediateResult, options);

            if (scope != null)
                VerifyFoundRobustly(scope, 1, locator, expectedDeferredResult, expectedImmediateResult, options);
        }

        protected void Should_find_robustly<T>(Func<string, T, Options, Scope> subject, Func<string, T, Options, Scope> scope, Action<string, Regex, ElementFound, Scope> stub, T input, Regex matchingPattern, ElementFound expectedImmediateResult, ElementFound expectedDeferredResult, ElementFound stubCssResult, bool exact = false)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            spyRobustWrapper.AlwaysReturnFromRobustly(expectedImmediateResult);

            stub(locator, matchingPattern, stubCssResult, browserSession);
            stub(locator, matchingPattern, stubCssResult, elementScope);

            var options = new Options { Timeout = individualTimeout, Exact = exact};

            VerifyFoundRobustly(subject, 0, locator, input, expectedDeferredResult, expectedImmediateResult, options);

            if (scope != null)
                VerifyFoundRobustly(scope, 1, locator, input, expectedDeferredResult, expectedImmediateResult, options);
        }

        protected void Should_find_robustly<T>(Func<string, T, Options, bool> subject, Func<string, T, Options, bool> scope, Action<string, Regex, ElementFound, Scope> stub, T suppliedText, Regex matchingPattern, bool expectedImmediateResult, bool expectedDeferredResult, ElementFound stubCssResult)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            spyRobustWrapper.AlwaysReturnFromRobustly(expectedImmediateResult);

            stub(locator, matchingPattern, stubCssResult, browserSession);
            stub(locator, matchingPattern, stubCssResult, elementScope);

            var options = new Options { Timeout = individualTimeout };

            VerifyFoundRobustly(subject, 0, locator, suppliedText, expectedDeferredResult, expectedImmediateResult, options);

            if (scope != null)
                VerifyFoundRobustly(scope, 1, locator, suppliedText, expectedDeferredResult, expectedImmediateResult, options);
        }

        protected void VerifyFoundRobustly(Func<string, Options, bool> scope, int driverCallIndex, string locator, bool expectedDeferredResult, bool expectedImmediateResult, Options options)
        {
            var scopedResult = scope(locator, options);

            VerifyFoundRobustly(driverCallIndex, expectedDeferredResult, expectedImmediateResult, options, scopedResult);
        }

        protected void VerifyFoundRobustly<T>(Func<string, T, Options, bool> scope, int driverCallIndex, string locator, T text, bool expectedDeferredResult, bool expectedImmediateResult, Options options)
        {
            var scopedResult = scope(locator, text, options);

            VerifyFoundRobustly(driverCallIndex, expectedDeferredResult, expectedImmediateResult, options, scopedResult);
        }

        protected void VerifyFoundRobustly<T>(Func<string, T, Options, Scope> scope, int driverCallIndex, string locator, T text, ElementFound expectedDeferredResult, ElementFound expectedImmediateResult, Options options)
        {
            var scopedResult = scope(locator, text, options).Find();

            VerifyFoundRobustly(driverCallIndex, expectedDeferredResult, expectedImmediateResult, options, scopedResult);
        }

        private void VerifyFoundRobustly<TResult>(int driverCallIndex, TResult expectedDeferredResult,
                                            TResult expectedImmediateResult, Options options, TResult scopedResult)
        {
            Assert.That(scopedResult, Is.Not.EqualTo(expectedDeferredResult), "Result was not found robustly");
            Assert.That(scopedResult, Is.EqualTo(expectedImmediateResult));

            Assert.That(RunQueryAndCheckTiming<TResult>(options.Timeout, driverCallIndex),
                        Is.EqualTo(expectedDeferredResult), "Deferred query result did not match expected value");
        }
    }
}