using System;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_single_elements : BrowserInteractionTests
    {
        [Test]
        public void FindButton_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindButton, elementScope.FindButton, driver.StubButton);
        }

        [Test]
        public void FindLink_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindLink, elementScope.FindLink, driver.StubLink);
        }

        [Test]
        public void FindField_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindField, elementScope.FindField, driver.StubField);
        }

        [Test]
        public void FindCss_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindCss, elementScope.FindCss, driver.StubCss);
        }

        [Test]
        public void FindId_should_make_robust_call_to_underlying_driver() 
        {
            Should_find_robustly(browserSession.FindId, elementScope.FindId, driver.StubId);
        }

        [Test]
        public void FindSection_should_make_robust_call_to_underlying_driver() 
        {
            Should_find_robustly(browserSession.FindSection, elementScope.FindSection, driver.StubSection);
        }

        [Test]
        public void FindFieldset_should_make_robust_call_to_underlying_driver() 
        {
            Should_find_robustly(browserSession.FindFieldset, elementScope.FindFieldset, driver.StubFieldset);
        }

        [Test]
        public void FindWindow_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindWindow, null, driver.StubWindow);
        }

        [Test]
        public void FindFrame_should_make_robust_call_to_underlying_driver()
        {
            Should_find_robustly(browserSession.FindFrame, null, driver.StubFrame);
        }

        [Test]
        public void FindXPath_should_make_robust_call_to_underlying_driver() 
        {
            Should_find_robustly(browserSession.FindXPath,elementScope.FindXPath, driver.StubXPath);
        }

        protected void Should_find_robustly(Func<string, Options, DriverScope> subject, Func<string, Options, DriverScope> scope, Action<string, ElementFound, DriverScope> stub)
        {
            var locator = "Find me " + DateTime.Now.Ticks;

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            var expectedImmediateResult = new StubElement();
            var expectedDeferredResult = new StubElement();

            spyRobustWrapper.AlwaysReturnFromRobustly(expectedImmediateResult);

            stub(locator, expectedDeferredResult,browserSession);
            stub(locator, expectedDeferredResult,elementScope);

            var options = new Options{Timeout = individualTimeout};

            VerifyFoundRobustly(subject, 0, locator, expectedDeferredResult, expectedImmediateResult, options);

            if (scope != null)
                VerifyFoundRobustly(scope, 1, locator, expectedDeferredResult, expectedImmediateResult, options);
        }

        private void VerifyFoundRobustly(Func<string, Options, DriverScope> scope, int driverCallIndex, string locator, StubElement expectedDeferredResult, StubElement expectedImmediateResult, Options options)
        {
            var sub = scope;
            var scopedResult = sub(locator, options).Now();

            Assert.That(scopedResult, Is.Not.SameAs(expectedDeferredResult), "Result was not found robustly");
            Assert.That(scopedResult, Is.SameAs(expectedImmediateResult));

            var elementScopeResult = RunQueryAndCheckTiming<ElementFound>(options.Timeout, driverCallIndex);

            Assert.That(elementScopeResult, Is.SameAs(expectedDeferredResult));
        }
    }
}