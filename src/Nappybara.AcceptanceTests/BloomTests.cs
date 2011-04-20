using System;
using Nappybara.API;
using Nappybara.Drivers;
using Nappybara.Robustness;
using NUnit.Framework;

namespace Nappybara.AcceptanceTests
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
			}
		}
	}
}