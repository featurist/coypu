using System;
using System.IO;
using System.Text.RegularExpressions;
using Coypu.Drivers.Playwright;
using Coypu.Drivers.Tests;
using Coypu.AcceptanceTests;
using Coypu.AcceptanceTests.Sites;
using Coypu.Finders;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;
using ElementFinder = Coypu.Finders.ElementFinder;
using FrameFinder = Coypu.Finders.FrameFinder;
using Coypu.Drivers.Selenium;
using OpenQA.Selenium.Chrome;

[SetUpFixture]
public class AssemblyTearDown
{
    public static SelfHostedSite TestSite;

    [OneTimeSetUp]
    public void StartTestSite()
    {
        TestSite = new SelfHostedSite();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        TestSite.Dispose();
        DriverSpecs.DisposeDriver();
    }
}

namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        private static IDriver _driver;
        private static DriverScope _root;
        private static readonly bool Headless = true;
        private static readonly Browser Browser = Browser.Chrome;
        protected static readonly Options DefaultOptions = new Options();

        private static readonly Type DriverType = typeof(PlaywrightDriver);

        protected static readonly SessionConfiguration DefaultSessionConfiguration = new SessionConfiguration
                                                                                     {
                                                                                         Browser = Browser,
                                                                                         Driver = DriverType,
                                                                                         TextPrecision = TextPrecision.Exact,
                                                                                     };

        protected static readonly DisambiguationStrategy DisambiguationStrategy = new ThrowsWhenMissingButNoDisambiguationStrategy();

        public static IDriver Driver
        {
            get
            {
                EnsureDriver();
                return _driver;
            }
        }

        protected static DriverScope Root => _root ?? (_root = new BrowserWindow(DefaultSessionConfiguration,
                                                                                 new DocumentElementFinder(Driver, DefaultSessionConfiguration),
                                                                                 null,
                                                                                 new ImmediateSingleExecutionFakeTimingStrategy(),
                                                                                 null,
                                                                                 null,
                                                                                 new ThrowsWhenMissingButNoDisambiguationStrategy()));

        protected virtual string TestPage => @"InteractionTestsPage.htm";

        protected string TestSiteUrl(string path)
        {
            return new Uri(AssemblyTearDown.TestSite.BaseUri, path).AbsoluteUri;
        }

        [SetUp]
        public virtual void SetUp()
        {
            Driver.Visit(PathHelper.GetPageHtmlPath(TestPage), Root);
        }

        private static void EnsureDriver()
        {
            if (_driver != null && !_driver.Disposed)
            {
                if (DriverType == _driver.GetType())
                    return;

                _driver.Dispose();
            }

            _driver = (IDriver) Activator.CreateInstance(DriverType, Browser, Headless);

            _root = null;
        }

        public static void DisposeDriver()
        {
            if (_driver != null && !_driver.Disposed) _driver.Dispose();
        }

        protected static Element FindSingle(ElementFinder finder)
        {
            return DisambiguationStrategy.ResolveQuery(finder);
        }

        protected static Element Frame(string locator,
                                       DriverScope scope = null,
                                       Options options = null)
        {
            return FindSingle(new FrameFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Button(string locator,
                                        DriverScope scope = null,
                                        Options options = null)
        {
            return FindSingle(new ButtonFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Link(string locator,
                                      DriverScope scope = null,
                                      Options options = null)
        {
            return FindSingle(new LinkFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Id(string locator,
                                    DriverScope scope = null,
                                    Options options = null)
        {
            return FindSingle(new IdFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Field(string locator,
                                       DriverScope scope = null,
                                       Options options = null)
        {
            return FindSingle(new FieldFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element XPath(string locator,
                                       DriverScope scope = null,
                                       Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element XPath(string locator,
                                       Regex text,
                                       DriverScope scope = null,
                                       Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element XPath(string locator,
                                       string text,
                                       DriverScope scope = null,
                                       Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Css(string locator,
                                     DriverScope scope = null,
                                     Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Css(string locator,
                                     Regex text,
                                     DriverScope scope = null,
                                     Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Css(string locator,
                                     string text,
                                     DriverScope scope = null,
                                     Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Section(string locator,
                                         DriverScope scope = null,
                                         Options options = null)
        {
            return FindSingle(new SectionFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Fieldset(string locator,
                                          DriverScope scope = null,
                                          Options options = null)
        {
            return FindSingle(new FieldsetFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Window(string locator,
                                        DriverScope scope = null,
                                        Options options = null)
        {
            return FindSingle(new WindowFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }
    }
}
