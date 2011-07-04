using System;
using System.Text.RegularExpressions;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture, Explicit]
    public class Coypu
    {
        [TearDown]
        public void TearDown()
        {
            Browser.EndSession();
        }

        [Test]
        public void Retries_Autotrader()
        {
            Session browser = Browser.Session;
            browser.Visit("http://www.autotrader.co.uk/used-cars");

            browser.FillIn("postcode").With("N1 1AA");

            browser.Select("citroen").From("make");
            browser.Select("c4_grand_picasso").From("model");

            browser.Select("National").From("radius");
            browser.Select("diesel").From("fuel-type");
            browser.Select("up_to_7_years_old").From("maximum-age");
            browser.Select("up_to_60000_miles").From("maximum-mileage");

            browser.FillIn("Add keyword").With("vtr");
        }


        [Test]
        public void Visibility_NewTwitterLogin()
        {
            Session browser = Browser.Session;
            browser.Visit("http://www.twitter.com");

            browser.FillIn("session[username_or_email]").With("coyputester2");
            browser.FillIn("session[password]").With("Nappybara");

            browser.ClickButton("Sign in");
        }

        [Test,
         Ignore("Make checkboxes on carbuzz are jumping around after you click each one. Re-enable when that is fixed")]
        public void FindingStuff_CarBuzz()
        {
            Session browser = Browser.Session;
            browser.Visit("http://carbuzz.heroku.com/car_search");

            Console.WriteLine(browser.Has(() => browser.FindSection("Make")));
            Console.WriteLine(browser.HasNo(() => browser.FindSection("Bake")));

            browser.Click(() => browser.FindSection("Make"));

            browser.Check("Audi");
            browser.Check("BMW");
            browser.Check("Mercedes");

            Assert.That(browser.HasContentMatch(new Regex(@"\b83 car reviews found")));

            browser.Click(() => browser.FindSection("Seats"));
            browser.ClickButton("4");

            Assert.That(browser.HasContentMatch(new Regex(@"\b28 car reviews found")));
        }

        [Test]
        public void HasNoContentTest()
        {
            Configuration.AppHost = "www.google.com";

            using (Session browser = Browser.Session)
            {
                browser.Visit("/");
                Assert.IsTrue(browser.HasContent("About Google"));
                Assert.IsTrue(browser.HasNoContent("This doesn't exist!"));
            }
        }

        [Test]
        public void SupplyYourOwnRemoteWebDriver()
        {
            Configuration.AppHost = "www.google.com";
            Configuration.Driver = typeof(SeleniumHtmlUnitWebDriver);

            using (Session browser = Browser.Session)
            {
                browser.Visit("/");
                Assert.IsTrue(browser.HasContent("Google"));
            }
        }
    }

    public class SeleniumHtmlUnitWebDriver : SeleniumWebDriver
    {
        public SeleniumHtmlUnitWebDriver() : base(new OpenQA.Selenium.Firefox.FirefoxDriver())
        {
            
        }
    }
}