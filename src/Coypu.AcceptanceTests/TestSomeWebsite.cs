using System;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture]
	public class TestSomeWebsite
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.Timeout = TimeSpan.FromSeconds(20);
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

				session.FillIn("session[username_or_email]","coyputester");
				session.FillIn("session[password]","nappybara");
				session.ClickButton("Sign in");
				session.ClickLink("find some interesting people");
				session.ClickLink("Technology");
				session.ClickLink("dickc");
				session.ClickLink("@dickc view full profile →");
				session.ClickLink("view all");
				session.ClickLink("← Back to @dickc");
				session.ClickLink("Profile");
				session.FillIn("find users by name","charlie sheen");
				session.ClickButton("Search");
			}

			//TODO: Try some tricky stuff
		}
	}
}