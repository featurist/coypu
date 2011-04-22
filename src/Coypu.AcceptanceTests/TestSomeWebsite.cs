using System;
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
				session.Visit("http://www.autotrader.co.uk");
				session.FillIn("postcode", "N1 1AA");
				session.ClickButton("Search");
				
			}

			//TODO: Try some tricky stuff
		}
	}
}