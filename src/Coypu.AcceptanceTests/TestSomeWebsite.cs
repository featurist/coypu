using System;
using Coypu.API;
using Coypu.Drivers;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture]
	public class TestSomeWebsite
	{
		[Test]
		public void TrySomeStuff()
		{
			using (var seleniumWebDriver = new SeleniumWebDriver())
			{
				var waitAndRetryRobustWrapper = new WaitAndRetryRobustWrapper(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
				var robustDriver = new RobustDriver(seleniumWebDriver, waitAndRetryRobustWrapper);
				var session = new Session(robustDriver);

				session.Visit("http://www.google.com");

				//TODO: Try some tricky stuff
			}
		}
	}
}