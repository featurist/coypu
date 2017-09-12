using System.Linq;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Coypu.AcceptanceTests
{
	public class RawWebDriver
	{
	    [Fact]
		public void Retries_Autotrader()
        {
            var browser = new FirefoxDriver();
            browser.Navigate().GoToUrl("http://www.autotrader.co.uk/used-cars");

	        browser.FindElementByName("postcode").SendKeys("N1 1AA");

	        browser.FindElementByName("make").FindElements(By.TagName("option")).First(e => e.GetAttribute("value") == "citroen").Click();
            browser.FindElementByName("model").FindElements(By.TagName("option")).First(e => e.GetAttribute("value") == "c4_grand_picasso").Click();
            browser.FindElementByName("radius").FindElements(By.TagName("option")).First(e => e.Text == "National").Click();
            browser.FindElementByName("fuel-type").FindElements(By.TagName("option")).First(e => e.GetAttribute("value") == "diesel").Click();
            browser.FindElementByName("maximum-age").FindElements(By.TagName("option")).First(e => e.GetAttribute("value") == "up_to_7_years_old").Click();
            browser.FindElementByName("maximum-mileage").FindElements(By.TagName("option")).First(e => e.GetAttribute("value") == "up_to_60000_miles").Click();

	        browser.FindElementByName("keywords").SendKeys("vtr");
        }

	    [Fact]
		public void Visibility_NewTwitter()
		{
            var browser = new FirefoxDriver();
            browser.Navigate().GoToUrl("http://www.twitter.com");

            browser.FindElementByName("session[username_or_email]").SendKeys("theuser");
            browser.FindElementByName("session[password]").SendKeys("thepass");
            
		}












	    [Fact]
	    public void FindingStuff_CarBuzz()
        {
            var browser = new FirefoxDriver();
	        browser.Navigate().GoToUrl("http://carbuzz.heroku.com/car_search");

	        browser.FindElementByName("make_17").Click();
	        browser.FindElementByName("make_14").Click();
	        browser.FindElementByName("make_11").Click();

            Assert.Contains(" 5 car reviews found", browser.PageSource);

            browser.FindElementByXPath("//ul[@class='seats']/li/button[@value='4']").Click();

	        Assert.Contains(" 1 car reviews found", browser.PageSource);
	    }
	}
}