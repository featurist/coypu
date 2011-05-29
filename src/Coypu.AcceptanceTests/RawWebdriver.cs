using System.Linq;
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

        // Raw selenium should fail on select option with 'Element is no longer attached to the DOM'
	    [Test]
	    public void FindingStuff_CarBuzz()
	    {
//            Configuration.AppHost = "carbuzz.heroku.com";
//
//            browser.Visit("/car_search");
//            browser.WithinSection("Make",
//                                  () =>
//                                      {
//                                          browser.Check("Audi");
//                                          browser.Check("Ford");
//                                          browser.Check("Maserati");
//                                      });
//            
//            Assert.That(browser.HasContent("30 car reviews found"));
//
//            browser.WithinSection("Seats", () => browser.ClickButton("4"));
//
//            Assert.That(browser.HasContent("8 car reviews found"));
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