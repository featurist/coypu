using System;
using Coypu.Drivers;

namespace Coypu
{
	public static class Configuration
	{
		static Configuration()
		{
			Timeout = TimeSpan.FromSeconds(1);
			Browser = Drivers.Browser.Firefox;
			WebDriver = typeof (SeleniumWebDriver);
		}

		public static TimeSpan Timeout { get; set; }
		public static Drivers.Browser Browser { get; set; }
		public static Type WebDriver { get; set; }
	}
}