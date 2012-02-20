using System;
using System.IO;
using System.Reflection;
using Coypu.Drivers.Selenium;
using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class DriverSpecs
    {
        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
        private DriverScope root;

        [SetUp]
        public void SetUp()
        {
            SetUpDriver(Browser.Firefox, typeof(SeleniumWebDriver));
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Dispose();
        }

        private void SetUpDriver(Browser browser, Type driverType)
        {
            EnsureDriver(driverType, browser);
            LoadTestHTML(driverType, browser);
        }

        private void LoadTestHTML(Type driverType, Browser browser)
        {
            EnsureDriver(driverType, browser);
            Driver.Visit(GetTestHTMLPathLocation());
        }

        private string GetTestHTMLPathLocation()
        {
            return new FileInfo(Path.Combine(@"..\..\", INTERACTION_TESTS_PAGE)).FullName;
        }

        protected DriverScope Root
        {
            get { return root ?? (root = new DriverScope(new DocumentElementFinder(Driver), null, null, null, null)); }
        }

        private void EnsureDriver(Type driverType, Browser browser)
        {
            if (Driver != null && !Driver.Disposed)
            {
                if (driverType == Driver.GetType() && Configuration.Browser == browser)
                    return;

                Driver.Dispose();
            }

            Configuration.Browser = browser;
            Driver = (Driver)Activator.CreateInstance(driverType);
        }

        protected Driver Driver { get; private set; }
    }
}