using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
    // To run the driver tests type "rake testdrivers > driver_test_results.txt" 
    // in the command line at the root of the repository

    public class DriverSpecRunner : nspec
    {
        private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";

        public Driver Driver { get; private set; }

        public void when_testing_each_driver()
        {
            LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs), Browser.Firefox);
            LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs), Browser.Chrome);
            LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs), Browser.InternetExplorer);
            LoadSpecsFor(typeof(WatiNDriver), typeof(DriverSpecs), Browser.InternetExplorer);
        }

        private void LoadDriverSpecs(Type driverType, Browser browser, Type specsToRun)
        {
            before = () =>
                     {
                         LoadTestHTML(driverType, browser);
                         Driver.ClearScope();
                     };

            Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsClass && t != typeof(DriverSpecs) && specsToRun.IsAssignableFrom(t) && IsSupported(t, driverType))
                    .Do(LoadSpecs);

            it["cleans up"] = () => { if (!Driver.Disposed) Driver.Dispose();};
        }

        private bool IsSupported(Type t, Type driverType)
        {
            return !t.GetCustomAttributes(typeof(NotSupportedByAttribute), true).Cast<NotSupportedByAttribute>().Any(a => a.Types.Contains(driverType));
        }

        private void LoadSpecs(Type driverSpecsType)
        {
            var specs = ((DriverSpecs)Activator.CreateInstance(driverSpecsType));
            specs.DriverSpecRunner = this;

            describe[driverSpecsType.Name.ToLowerInvariant().Replace('_', ' ')] = specs.Specs;
        }

        private void LoadTestHTML(Type driverType, Browser browser)
        {
            EnsureDriver(driverType, browser);
            Driver.Visit(GetTestHTMLPathLocation());
        }

        private void LoadSpecsFor(Type driverType, Type specsToRun, Browser browser)
        {
            context["and the driver is " + driverType.Name] = () => LoadSpecsFor(driverType, browser, specsToRun);
        }

        private void LoadSpecsFor(Type driverType, Browser browser, Type specsToRun)
        {
            context["and the browser is " + browser] = () => LoadDriverSpecs(driverType, browser, specsToRun);
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

        private string GetTestHTMLPathLocation()
        {
            return new FileInfo(INTERACTION_TESTS_PAGE).FullName;
        }

        public ActionRegister NSpecDescribe
        {
            get { return describe; }
        }

        public ActionRegister NSpecContext
        {
            get { return context; }
        }

        public ActionRegister NSpecIt
        {
            get { return it; }
        }

        public Action NSpecBefore
        {
            set { before = value; }
        }

        public Action NSpecAfter
        {
            set { before = value; }
        }
    }
}