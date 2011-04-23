using System;
using Coypu.Drivers.Selenium;

namespace Coypu
{
	public static class Configuration
	{
		static Configuration()
		{
			Timeout = TimeSpan.FromSeconds(10);
			RetryInterval = TimeSpan.FromSeconds(0.1);
			Browser = Drivers.Browser.Firefox;
			WebDriver = typeof(SeleniumWebDriver);
		}

		public static TimeSpan RetryInterval { get; set; }
		public static TimeSpan Timeout { get; set; }
		public static Drivers.Browser Browser { get; set; }
		public static Type WebDriver { get; set; }
	}
}