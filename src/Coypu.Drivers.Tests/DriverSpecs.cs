using System;
using System.IO;
using System.Text.RegularExpressions;
using Coypu.Drivers.Tests.Sites;
using Coypu.Finders;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

[SetUpFixture]
public class AssemblyTearDown
{
    public static SelfishSite TestSite;

    [OneTimeSetUp]
    public void StartTestSite()
    {
        TestSite = new SelfishSite();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        TestSite.Dispose();

        Coypu.Drivers.Tests.DriverSpecs.DisposeDriver();
    }
}


namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        protected string TestSiteUrl(string path)
        {
            return new Uri(AssemblyTearDown.TestSite.BaseUri, path).AbsoluteUri;
        }

        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
        private static DriverScope root;
        private static IDriver driver;

        private static readonly Browser browser = Browser.Chrome;
        private static readonly Type driverType = typeof (Selenium.SeleniumWebDriver);

//        private static readonly Browser browser = Browser.InternetExplorer;
//        private static readonly Type driverType = typeof (Watin.WatiNDriver);

        [SetUp]
        public virtual void SetUp()
        {
            Driver.Visit(GetTestHTMLPathLocation(),Root);
        }

        protected string GetTestHTMLPathLocation()
        {
            return TestHtmlPathLocation(TestPage);
        }

        protected static string TestHtmlPathLocation(string testPage)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.Combine(@"..\..\", testPage));
            return "file:///" + file.Replace('\\', '/');
        }

        protected virtual string TestPage
        {
            get { return INTERACTION_TESTS_PAGE; }
        }

        protected static DriverScope Root
        {
            get { return root ?? (root = new BrowserWindow(DefaultSessionConfiguration, new DocumentElementFinder(Driver, DefaultSessionConfiguration), null, new ImmediateSingleExecutionFakeTimingStrategy(), null, null, new ThrowsWhenMissingButNoDisambiguationStrategy() )); }
        }

        protected readonly static Options DefaultOptions = new Options();

        protected static readonly SessionConfiguration DefaultSessionConfiguration = new SessionConfiguration
            {
                Browser = browser,
                Driver = driverType,
                TextPrecision = TextPrecision.Exact
            };

        private static void EnsureDriver()
        {
            if (driver != null && !driver.Disposed)
            {
                if (driverType == driver.GetType())
                    return;

                driver.Dispose();
            }

            driver = (IDriver)Activator.CreateInstance(driverType,browser);
            root = null;
        }

        public static IDriver Driver
        {
            get
            {
                EnsureDriver();
                return driver;
            }
        }

        public static void DisposeDriver()
        {
            if (driver != null && !driver.Disposed)
            {
                driver.Dispose();
            }
        }

        protected static readonly DisambiguationStrategy DisambiguationStrategy = new ThrowsWhenMissingButNoDisambiguationStrategy();
        protected static Element FindSingle(ElementFinder finder)
        {
            return DisambiguationStrategy.ResolveQuery(finder);
        }

        protected static Element Frame(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new FrameFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Button(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new ButtonFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Link(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new LinkFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Id(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new IdFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Field(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new FieldFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element XPath(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element XPath(string locator, Regex text, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element XPath(string locator, string text, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Css(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Css(string locator, Regex text, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Css(string locator, string text, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text));
        }

        protected static Element Section(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new SectionFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Fieldset(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new FieldsetFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }

        protected static Element Window(string locator, DriverScope scope = null, Options options = null)
        {
            return FindSingle(new WindowFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions));
        }
    }
}