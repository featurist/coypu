using System;
using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.DriverImplementationTests.Drivers
{
	[TestFixture]
	public class SeleniumDriverTests : DriverImplementationTests, IDisposable
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