using System;
using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture]
	public class TestSomeWebsite
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.Timeout = TimeSpan.FromSeconds(10);
			Configuration.Browser = Drivers.Browser.Firefox;
			Configuration.WebDriver = typeof(SeleniumWebDriver);
		}

		[TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

		[Test]
		public void TrySomeStuff()
		{
			using (var session = Browser.Session)
			{
				session.Visit("http://www.twitter.com");

				session.FillIn("session[username_or_email]","coypu");
				session.FillIn("session[password]","nappybara");
				session.ClickButton("Sign in");
			}


			//TODO: Try some tricky stuff
		}
	}
}