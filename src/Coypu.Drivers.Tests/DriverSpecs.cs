using System;
using System.IO;
using Coypu.Drivers.Selenium;
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

        private static readonly Browser browser = Browser.InternetExplorer;
        private static readonly Type driverType = typeof (WatiNDriver);

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
            get { return root ?? (root = new DriverScope(new SessionConfiguration{Browser = browser, Driver = driverType}, new DocumentElementFinder(Driver), null, new ImmediateSingleExecutionFakeRobustWrapper(), null, null)); }
        }

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
    }
}