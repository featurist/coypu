using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Coypu.Drivers.Selenium;
using NSpec;

namespace Coypu.Drivers.Tests
{
	public class Test_each_driver_and_browser_combination : nspec
	{
		private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
		Driver driver;

		public void when_testing_each_driver()
		{
			LoadSpecsFor(typeof(SeleniumWebDriver), typeof(DriverSpecs)); // All
			// LoadSpecsFor(typeof(SeleniumWebDriver), typeof(When_inspecting_dialog_text)); // Individual
		}

		private void LoadSpecsForEachBrowser(Type driverType, Type specsToRun)
		{
			LoadSpecsFor(driverType, Browser.Firefox, specsToRun);
			//LoadSpecsFor(driverType, Browser.Chrome, specsToRun);
			//LoadSpecsFor(driverType, Browser.InternetExplorer, specsToRun);
		}

		private void LoadDriverSpecs(Type driverType, Browser browser, Type specsToRun)
		{
			before = () => LoadTestHTML(driverType, browser);

			Assembly.GetExecutingAssembly().GetTypes()
					.Where(t => t.IsClass && specsToRun.IsAssignableFrom(t))
					.Do(LoadSpecs);
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

		private void LoadSpecsFor(Type driverType, Type specsToRun)
		{
			context["and the driver is " + driverType.Name] = () => LoadSpecsForEachBrowser(driverType,specsToRun);
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