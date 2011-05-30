using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests
{
	[TestFixture,Explicit]
	public class RawWebDriver
	{
	    private RemoteWebDriver browser;

	    [SetUp]
		public void SetUp()
		{
            browser = new FirefoxDriver();
		}
        
		[TearDown]
		public void TearDown()
		{
			browser.Dispose();
		}


	    [Test]
	    public void FindingStuff_CarBuzz()
	    {
            browser.Navigate().GoToUrl("http://carbuzz.heroku.com/car_search");

	        var start = DateTime.Now;

            browser.FindElementByName("make_17").Click();
            browser.FindElementByName("make_14").Click();
            browser.FindElementByName("make_11").Click();

            browser.FindElementByXPath("//ul[@class='seats']/li/button[@value='4']").Click();

	        browser.FindElementByXPath("//ul[@class='features']/li/button[@value='Fun to Drive']").Click();
            browser.FindElementByXPath("//ul[@class='features']/li/button[@value='Cabriolet']").Click();

	        Console.WriteLine(DateTime.Now - start);

            Assert.That(browser.PageSource.Contains(" 8 car reviews found"));
	    }

	    [Test]
		public void Retries_AutotraderCurrentAPI()
        {
            browser.Navigate().GoToUrl("http://www.autotrader.co.uk/used-cars");

	        browser.FindElementByName("postcode").SendKeys("N1 1AA");

	        browser.FindElementByName("make").FindElements(By.TagName("option")).First(e => e.Value == "citroen").Select();
	        browser.FindElementByName("model").FindElements(By.TagName("option")).First(e => e.Value == "c4_grand_picasso").Select();
            browser.FindElementByName("radius").FindElements(By.TagName("option")).First(e => e.Text == "National").Select();
            browser.FindElementByName("fuel-type").FindElements(By.TagName("option")).First(e => e.Value == "diesel").Select();
            browser.FindElementByName("maximum-age").FindElements(By.TagName("option")).First(e => e.Value == "up_to_7_years_old").Select();
            browser.FindElementByName("maximum-mileage").FindElements(By.TagName("option")).First(e => e.Value == "up_to_60000_miles").Select();

	        browser.FindElementByName("keywords").SendKeys("vtr");
        }

	    [Test]
		public void Visibility_NewTwitter()
		{
            browser.Navigate().GoToUrl("http://www.twitter.com");

            browser.FindElementByName("session[username_or_email]").SendKeys("theuser");
            browser.FindElementByName("session[password]").SendKeys("thepassword");
		}
	}
}