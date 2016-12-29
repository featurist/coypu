using System;
using System.Text.RegularExpressions;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using Xunit;
using OpenQA.Selenium;

namespace Coypu.AcceptanceTests
{
    public class ExternalExamples : IDisposable
    {
        private SessionConfiguration SessionConfiguration;
        private BrowserSession browser;

        public ExternalExamples()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.AppHost = "www.google.com";
            SessionConfiguration.Driver = typeof(SeleniumWebDriver);

            SessionConfiguration.Timeout = TimeSpan.FromSeconds(10);

            browser = new BrowserSession(SessionConfiguration);
        }

        public void Dispose()
        {
            browser.Dispose();
        }

        [Fact]
        public void AppHostContainsScheme()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.AppHost = "https://www.google.co.uk/";
            SessionConfiguration.Driver = typeof(SeleniumWebDriver);

            browser = new BrowserSession(SessionConfiguration);

            browser.Visit("/");
            Assert.Equal("https://www.google.co.uk/", browser.Location.ToString());

        }

        [Fact]
        public void Retries_Autotrader()
        {
            browser.Visit("http://www.autotrader.co.uk/used-cars");

            browser.FillIn("postcode").With("N1 1AA");
            
            browser.FindField("make").Click();
            
            browser.Select("citroen").From("make");
            browser.Select("c4_grand_picasso").From("model");

            browser.Select("National").From("radius");
            browser.Select("diesel").From("fuel-type");
            browser.Select("up_to_7_years_old").From("maximum-age");
            browser.Select("up_to_60000_miles").From("maximum-mileage");
            
            browser.FillIn("Add keyword").With("vtr");
        }


        [Fact]
        public void Visibility_NewTwitterLogin()
        {
            browser.Visit("http://www.twitter.com");

            browser.FillIn("session[username_or_email]").With("coyputester2");
            browser.FillIn("session[password]").With("Nappybara");
        }

        [Fact(Skip = "Make checkboxes on carbuzz are jumping around after you click each one. Re-enable when that is fixed")]
        public void FindingStuff_CarBuzz()
        {
            browser.Visit("http://carbuzz.heroku.com/car_search");

            Console.WriteLine(browser.FindSection("Make").Exists());
            Console.WriteLine(browser.FindSection("Bake").Exists());

            browser.Check("Audi");
            browser.Check("BMW");
            browser.Check("Mercedes");

            Assert.True(browser.HasContentMatch(new Regex(@"\b83 car reviews found")));

            browser.FindSection("Seats").Click();
            browser.ClickButton("4");

            Assert.True(browser.HasContentMatch(new Regex(@"\b28 car reviews found")));
        }

        [Fact]
        public void HtmlUnitDriver()
        {
            SessionConfiguration.AppHost = "www.google.com";
            SessionConfiguration.Browser = Drivers.Browser.HtmlUnit;

            try
            {
                using (var htmlUnit = new BrowserSession(SessionConfiguration))
                {
                    htmlUnit.Visit("/");
                }
                Assert.True(false, "Expected an exception attempting to connect to HtmlUnit driver");
            }
            catch (WebDriverException e)
            {
                Assert.Contains("No connection could be made because the target machine actively refused it 127.0.0.1:4444", e.Message);
            }
        }
    }
}