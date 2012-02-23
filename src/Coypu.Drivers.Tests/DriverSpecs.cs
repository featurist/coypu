using System;
using System.IO;
using Coypu.Drivers.Tests.Sites;
using Coypu.Drivers.Watin;
using Coypu.Finders;
using NUnit.Framework;

[SetUpFixture]
public class AssmeblyTearDown
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

        var driver = Coypu.Drivers.Tests.DriverSpecs.Driver;
        if (driver != null && !driver.Disposed)
            driver.Dispose();
    }
}


namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
        private static DriverScope root;
        private static Driver driver;

        private const Browser browser = Browser.InternetExplorer;
        private static readonly Type driverType = typeof (WatiNDriver);

        [SetUp]
        public void SetUp()
        {
            Driver.Visit(GetTestHTMLPathLocation());
        }

        private string GetTestHTMLPathLocation()
        {
            return new FileInfo(Path.Combine(@"..\..\", INTERACTION_TESTS_PAGE)).FullName;
        }

        protected static DriverScope Root
        {
            get { return root ?? (root = new DriverScope(new DocumentElementFinder(Driver), null, null, null, null)); }
        }

        private static void EnsureDriver()
        {
            if (driver != null && !driver.Disposed)
            {
                if (driverType == driver.GetType() && Configuration.Browser == browser)
                    return;

                driver.Dispose();
            }

            Configuration.Browser = browser;
            driver = (Driver)Activator.CreateInstance(driverType);
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
    }
}