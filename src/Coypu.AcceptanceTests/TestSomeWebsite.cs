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
				session.Visit("http://www.google.com");

				session.ClickButton("I'm Feeling Lucky");
				session.ClickLink("2000");
				session.ClickLink("Next »");
				session.ClickLink("Home");
			}

			Configuration.Browser = Drivers.Browser.Chrome;

			Browser.Session.Visit("http://www.bing.com");

			//TODO: Try some tricky stuff
		}
	}
}