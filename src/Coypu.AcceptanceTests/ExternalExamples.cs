using System;
using System.Text.RegularExpressions;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class ExternalExamples
    {
        [SetUp]
        public void SetUp() 
        {
            Configuration.Reset();

            Configuration.AppHost = "www.google.com";
            Configuration.Driver = typeof(SeleniumWebDriver);

            Configuration.Timeout = TimeSpan.FromSeconds(10);
        }
        [TearDown]
        public void TearDown()
        {
            Configuration.Reset();
            Browser.EndSession();
        }

        [Test]
        public void Retries_Autotrader()
        {
            var browser = Browser.Session;
            browser.Visit("http://www.autotrader.co.uk/used-cars");

            browser.FillIn("postcode").With("N1 1AA");
            
            browser.Click(browser.FindField("make"));
            
            browser.Select("citroen").From("make");
            browser.Select("c4_grand_picasso").From("model");

            browser.Select("National").From("radius");
            browser.Select("diesel").From("fuel-type");
            browser.Select("up_to_7_years_old").From("maximum-age");
            browser.Select("up_to_60000_miles").From("maximum-mileage");

            browser.FillIn("Add keyword").With("vtr");
        }


        [Test, Explicit]
        public void Visibility_NewTwitterLogin()
        {
            var browser = Browser.Session;
            browser.Visit("http://www.twitter.com");

            browser.FillIn("session[username_or_email]").With("coyputester2");
            browser.FillIn("session[password]").With("Nappybara");
        }

        [Test,
         Ignore("Make checkboxes on carbuzz are jumping around after you click each one. Re-enable when that is fixed")]
        public void FindingStuff_CarBuzz()
        {
            var browser = Browser.Session;
            browser.Visit("http://carbuzz.heroku.com/car_search");

            Console.WriteLine(browser.Has(browser.FindSection("Make"));
            Console.WriteLine(browser.HasNo(browser.FindSection("Bake"));

            browser.Check("Audi");
            browser.Check("BMW");
            browser.Check("Mercedes");

            Assert.That(browser.HasContentMatch(new Regex(@"\b83 car reviews found")));

            browser.Click(browser.FindSection("Seats"));
            browser.ClickButton("4");

            Assert.That(browser.HasContentMatch(new Regex(@"\b28 car reviews found")));
        }

        [Test]
        public void HtmlUnitDriver()
        {
            Configuration.AppHost = "www.google.com";
            Configuration.Browser = Drivers.Browser.HtmlUnit;

            try
            {
                using (Session browser = Browser.Session)
                {
                    browser.Visit("/");
                }
                Assert.Fail("Expected an exception attempting to connect to HtmlUnit driver");
            }
            catch (WebDriverException e)
            {
                Assert.That(e.Message, Is.StringContaining("No connection could be made because the target machine actively refused it 127.0.0.1:4444"));
            }
        }
    }
}