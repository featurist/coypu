using System;
using Nappybara.Drivers;
using NUnit.Framework;

namespace Nappybara.DriverImplementationTests.Drivers
{
	[TestFixture]
	public class SeleniumDriverTests : RealDriverImplementationTestSuite, IDisposable
	{
		private SeleniumWebDriver driver;

		protected override Driver Driver
		{
			get { return driver ?? (driver = new SeleniumWebDriver()); }
		}

		public override void Dispose()
		{
			driver.Dispose();
		}
	}
}