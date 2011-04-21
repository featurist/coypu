using System;
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
			using (var seleniumWebDriver = new SeleniumWebDriver(Browser.Firefox))
			{
				var waitAndRetryRobustWrapper = new WaitAndRetryRobustWrapper(TimeSpan.FromSeconds(10));
				var session = new Session(seleniumWebDriver, waitAndRetryRobustWrapper);

				session.Visit("http://www.google.com");

				session.ClickButton("I'm Feeling Lucky");
				session.ClickLink("2000");
				session.ClickLink("Next »");
				session.ClickLink("Home");

				//TODO: Try some tricky stuff
			}
		}
	}
}