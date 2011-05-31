using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Coypu.AcceptanceTests
{
	[TestFixture,Explicit]
	public class RawWebDriver
	{
	    [Test]
		public void Retries_Autotrader()
        {
            var browser = new FirefoxDriver();
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
            var browser = new FirefoxDriver();
            browser.Navigate().GoToUrl("http://www.twitter.com");

            browser.FindElementByName("session[username_or_email]").SendKeys("theuser");
            browser.FindElementByName("session[password]").SendKeys("thepass");
            
		}












	    [Test]
	    public void FindingStuff_CarBuzz()
        {
            var browser = new FirefoxDriver();
	        browser.Navigate().GoToUrl("http://carbuzz.heroku.com/car_search");

	        browser.FindElementByName("make_17").Click();
	        browser.FindElementByName("make_14").Click();
	        browser.FindElementByName("make_11").Click();

	        Assert.That(browser.PageSource.Contains(" 5 car reviews found"));

	        browser.FindElementByXPath("//ul[@class='seats']/li/button[@value='4']").Click();

	        Assert.That(browser.PageSource.Contains(" 1 car reviews found"));
	    }
	}
}