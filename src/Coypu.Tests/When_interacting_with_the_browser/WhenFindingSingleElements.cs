using System;
using Coypu.Finders;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class WhenFindingSingleElements
    {
        [SetUp]
        public void SetUp()
        {
            Driver = new StubDriver();
            SessionConfiguration = new SessionConfiguration
                                   {
                                       ConsiderInvisibleElements = true,
                                       RetryInterval = TimeSpan.FromMilliseconds(123)
                                   };
            BrowserSession = TestSessionBuilder.Build(SessionConfiguration, Driver, null, null, null, null);
        }

        protected IDriver Driver;
        protected BrowserSession BrowserSession;
        protected SessionConfiguration SessionConfiguration;

        private void Should_find_robust_scope<T, TScope, TReturn>(Func<string, Options, TScope> findButton)
            where TScope : DriverScope
        {
            var options = new Options
                          {
                              TextPrecision = TextPrecision.Exact,
                              Timeout = TimeSpan.FromMilliseconds(999)
                          };
            var scope = findButton("Some locator", options);
            Assert.That(scope, Is.TypeOf<TReturn>());

            GetValue<T>(scope, options);
        }

        private void GetValue<T>(DriverScope scope,
                                 Options options)
        {
            Assert.That(scope.OuterScope, Is.SameAs(BrowserSession));
            Assert.That(scope.ElementFinder, Is.TypeOf<T>());
            Assert.That(scope.ElementFinder.Driver, Is.EqualTo(Driver));
            Assert.That(scope.ElementFinder.Locator, Is.EqualTo("Some locator"));
            Assert.That(scope.ElementFinder.Options, Is.EqualTo(Options.Merge(options, SessionConfiguration)));
            Assert.That(scope.ElementFinder.ToString(), Is.EqualTo(scope.ElementFinder.QueryDescription));
        }

        [Test]
        public void FindButton_Should_find_robust_scope()
        {
            Should_find_robust_scope<ButtonFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindButton);
        }

        [Test]
        public void FindCss_Should_find_robust_scope()
        {
            Should_find_robust_scope<CssFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindCss);
        }

        [Test]
        public void FindField_Should_find_robust_scope()
        {
            Should_find_robust_scope<FieldFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindField);
        }

        [Test]
        public void FindFieldset_Should_find_robust_scope()
        {
            Should_find_robust_scope<FieldsetFinder, ElementScope, SynchronisedElementScope>(BrowserSession
                                                                                                 .FindFieldset);
        }

        [Test]
        public void FindFrame_Should_find_robust_scope()
        {
            Should_find_robust_scope<FrameFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindFrame);
        }

        [Test]
        public void FindId_Should_find_robust_scope()
        {
            Should_find_robust_scope<IdFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindId);
        }

        [Test]
        public void FindLink_Should_find_robust_scope()
        {
            Should_find_robust_scope<LinkFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindLink);
        }

        [Test]
        public void FindSection_Should_find_robust_scope()
        {
            Should_find_robust_scope<SectionFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindSection);
        }

        [Test]
        public void FindWindow_Should_find_robust_scope()
        {
            Should_find_robust_scope<WindowFinder, BrowserWindow, BrowserWindow>(BrowserSession.FindWindow);
        }

        [Test]
        public void FindXPath_Should_find_robust_scope()
        {
            Should_find_robust_scope<XPathFinder, ElementScope, SynchronisedElementScope>(BrowserSession.FindXPath);
        }
    }
}