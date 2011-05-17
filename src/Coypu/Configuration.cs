using System;
using Coypu.Drivers.Selenium;

namespace Coypu
{
	public static class Configuration
	{
		const string DEFAULT_APP_HOST = "localhost";
		const int DEFAULT_PORT = 80;
		const double DEFAULT_TIMEOUT_SECONDS = 10;
		const double DEFAULT_INTERVAL_SECONDS = 0.1;

		static Configuration()
		{
			Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS);
			RetryInterval = TimeSpan.FromSeconds(DEFAULT_INTERVAL_SECONDS);
			Browser = Drivers.Browser.Firefox;
			Driver = typeof(SeleniumWebDriver);
		}

		public static TimeSpan RetryInterval { get; set; }
		public static TimeSpan Timeout { get; set; }
		public static Drivers.Browser Browser { get; set; }
		public static Type Driver { get; set; }
		
		private static string appHost;
		public static string AppHost
		{
			get { return appHost == default(string) ? DEFAULT_APP_HOST : appHost;}
			set { appHost = value == null ? null : value.TrimEnd('/'); }
		}

		private static int port;
		public static int Port
		{
			get { return port == default(int) ? DEFAULT_PORT : port;}
			set { port = value; }
		}

		public static bool SSL { get; set; }
	}
}