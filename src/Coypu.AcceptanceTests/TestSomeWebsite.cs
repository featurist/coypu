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
			using (var seleniumWebDriver = new SeleniumWebDriver())
			{
				var waitAndRetryRobustWrapper = new WaitAndRetryRobustWrapper(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
				var session = new Session(seleniumWebDriver, waitAndRetryRobustWrapper);

				session.Visit("http://www.google.com");

				session.ClickButton("I'm Feeling Lucky");
				//TODO: Try some tricky stuff
			}
		}
	}
}