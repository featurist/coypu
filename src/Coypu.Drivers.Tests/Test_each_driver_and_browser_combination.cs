using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;
using NSpec;

namespace Coypu.Drivers.Tests
{
	public class Test_each_driver_and_browser_combination : nspec
	{
		private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
		Driver driver;

		public void when_testing_each_driver()
		{
			LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs), Browser.Firefox);
			LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs), Browser.InternetExplorer);
			
			LoadSpecsFor(typeof(WatiNDriver), typeof(DriverSpecs), Browser.InternetExplorer);
		}

		private void LoadDriverSpecs(Type driverType, Browser browser, Type specsToRun)
		{
			before = () => LoadTestHTML(driverType, browser);

			Assembly.GetExecutingAssembly().GetTypes()
					.Where(t => t.IsClass && specsToRun.IsAssignableFrom(t) && IsSupported(t))
					.Do(LoadSpecs);

			it["cleans up"] = () => { if (!driver.Disposed) driver.Dispose();};
		}

		private bool IsSupported(Type t)
		{
			return !t.GetCustomAttributes(typeof(NotSupportedByAttribute),true).Any();
		}

		private void LoadSpecs(Type driverSpecsType)
		{
			describe[driverSpecsType.Name.ToLowerInvariant().Replace('_', ' ')] 
				= ((DriverSpecs)Activator.CreateInstance(driverSpecsType)).Specs(GetDriver, describe, it);
		}

		private Driver GetDriver()
		{
			return driver;
		}

		private void LoadTestHTML(Type driverType, Browser browser)
		{
			EnsureDriver(driverType, browser);
			driver.Visit(GetTestHTMLPathLocation());
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
			if (driver != null && !driver.Disposed)
			{
				if (driverType == driver.GetType() && Configuration.Browser == browser)
					return;

				driver.Dispose();  
			} 
			
			Configuration.Browser = browser;
			driver = (Driver)Activator.CreateInstance(driverType);
		}

		private string GetTestHTMLPathLocation()
		{
			var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			var projRoot = Path.Combine(assemblyDirectory,@"..\..\");
			return new FileInfo(Path.Combine(projRoot,INTERACTION_TESTS_PAGE)).FullName;
		}
	}
}