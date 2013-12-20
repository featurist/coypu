using System;
using System.IO;
using System.Text.RegularExpressions;
using Coypu.Drivers.Tests.Sites;
using Coypu.Drivers.Watin;
using Coypu.Finders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

[SetUpFixture]
public class AssemblyTearDown
{
    private SinatraSite sinatraSite;

    [SetUp]
    public void StartSinatra()
    {
        sinatraSite = new SinatraSite(@"..\..\..\Coypu.AcceptanceTests\sites\site_with_secure_resources.rb");
    }

    [TearDown]
    public void TearDown()
    {
        sinatraSite.Dispose();

        Coypu.Drivers.Tests.DriverSpecs.DisposeDriver();
    }
}


namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
        private static DriverScope root;
        private static Driver driver;

        private static readonly Browser browser = Browser.Chrome;
        private static readonly Type driverType = typeof (Selenium.SeleniumWebDriver);

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
            var file = new FileInfo(Path.Combine(@"..\..\", testPage)).FullName;
            return "file:///" + file.Replace('\\', '/');
        }

        protected virtual string TestPage
        {
            get { return INTERACTION_TESTS_PAGE; }
        }

        protected static DriverScope Root
        {
            get { return root ?? (root = new DriverScope(DefaultSessionConfiguration, new DocumentElementFinder(Driver, DefaultSessionConfiguration), null, new ImmediateSingleExecutionFakeTimingStrategy(), null, null)); }
        }

        protected readonly static Options DefaultOptions = new Options{};
        protected readonly static Options ExactOptions = new Options{Exact = true};
        protected readonly static Options PartialOptions = new Options{Exact = false};

        protected static readonly SessionConfiguration DefaultSessionConfiguration = new SessionConfiguration
            {
                Browser = browser,
                Driver = driverType
            };

        private static void EnsureDriver()
        {
            if (driver != null && !driver.Disposed)
            {
                if (driverType == driver.GetType())
                    return;

                driver.Dispose();
            }

            driver = (Driver)Activator.CreateInstance(driverType,browser);
            root = null;
        }

        public static Driver Driver
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

        protected static ElementFound Frame(string locator, DriverScope scope = null, Options options = null)
        {
            return new FrameFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Button(string locator, DriverScope scope = null, Options options = null)
        {
            return new ButtonFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Link(string locator, DriverScope scope = null, Options options = null)
        {
            return new LinkFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Id(string locator, DriverScope scope = null, Options options = null)
        {
            return new IdFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Field(string locator, DriverScope scope = null, Options options = null)
        {
            return new FieldFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound XPath(string locator, DriverScope scope = null, Options options = null)
        {
            return new XPathFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Css(string locator, DriverScope scope = null, Options options = null)
        {
            return new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Css(string locator, Regex text, DriverScope scope = null, Options options = null)
        {
            return new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text).ResolveQuery();
        }

        protected static ElementFound Css(string locator, string text, DriverScope scope = null, Options options = null)
        {
            return new CssFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions, text).ResolveQuery();
        }

        protected static ElementFound Section(string locator, DriverScope scope = null, Options options = null)
        {
            return new SectionFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }

        protected static ElementFound Fieldset(string locator, DriverScope scope = null, Options options = null)
        {
            return new FieldsetFinder(Driver, locator, scope ?? Root, options ?? DefaultOptions).ResolveQuery();
        }
    }
}