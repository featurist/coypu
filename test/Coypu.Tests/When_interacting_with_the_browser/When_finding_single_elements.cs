using System;
using Coypu.Finders;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_finding_single_elements
    {
        protected Driver driver;
        protected BrowserSession browserSession;
        protected SessionConfiguration sessionConfiguration;

        public When_finding_single_elements()
        {
            driver = new StubDriver();
            sessionConfiguration = new SessionConfiguration{ConsiderInvisibleElements = true, RetryInterval = TimeSpan.FromMilliseconds(123)};
            browserSession = TestSessionBuilder.Build(sessionConfiguration, driver, null, null,null, null);
        }
        
        [Fact]
        public void FindButton_Should_find_robust_scope()
        {
            Should_find_robust_scope<ButtonFinder, ElementScope, SynchronisedElementScope>(browserSession.FindButton);
        }
        
        [Fact]
        public void FindLink_Should_find_robust_scope()
        {
            Should_find_robust_scope<LinkFinder, ElementScope, SynchronisedElementScope>(browserSession.FindLink);
        }

        [Fact]
        public void FindField_Should_find_robust_scope()
        {
            Should_find_robust_scope<FieldFinder, ElementScope, SynchronisedElementScope>(browserSession.FindField);
        }

        [Fact]
        public void FindCss_Should_find_robust_scope()
        {
            Should_find_robust_scope<CssFinder, ElementScope, SynchronisedElementScope>(browserSession.FindCss);
        }

        [Fact]
        public void FindId_Should_find_robust_scope()
        {
            Should_find_robust_scope<IdFinder, ElementScope, SynchronisedElementScope>(browserSession.FindId);
        }

        [Fact]
        public void FindSection_Should_find_robust_scope()
        {
            Should_find_robust_scope<SectionFinder, ElementScope, SynchronisedElementScope>(browserSession.FindSection);
        }

        [Fact]
        public void FindFieldset_Should_find_robust_scope()
        {
            Should_find_robust_scope<FieldsetFinder, ElementScope, SynchronisedElementScope>(browserSession.FindFieldset);
        }

        [Fact]
        public void FindWindow_Should_find_robust_scope()
        {
            Should_find_robust_scope<WindowFinder, BrowserWindow, BrowserWindow>(browserSession.FindWindow);
        }

        [Fact]
        public void FindFrame_Should_find_robust_scope()
        {
            Should_find_robust_scope<FrameFinder, ElementScope, SynchronisedElementScope>(browserSession.FindFrame);
        }

        [Fact]
        public void FindXPath_Should_find_robust_scope()
        {
            Should_find_robust_scope<XPathFinder,ElementScope,SynchronisedElementScope>(browserSession.FindXPath);
        }

        private void Should_find_robust_scope<T,TScope,TReturn>(Func<string, Options, TScope> findButton) where TScope : DriverScope
        {
            var options = new Options { TextPrecision = TextPrecision.Exact, Timeout = TimeSpan.FromMilliseconds(999) };
            var scope = findButton("Some locator", options);
            Assert.IsType<TReturn>(scope);

            GetValue<T>(scope, options);
        }

        private void GetValue<T>(DriverScope scope, Options options)
        {
            Assert.Same(browserSession, scope.OuterScope);
            Assert.IsType<T>(scope.ElementFinder);
            Assert.Equal(driver, scope.ElementFinder.Driver);
            Assert.Equal("Some locator", scope.ElementFinder.Locator);
            Assert.Equal(Options.Merge(options, sessionConfiguration), scope.ElementFinder.Options);
        }
    }
}