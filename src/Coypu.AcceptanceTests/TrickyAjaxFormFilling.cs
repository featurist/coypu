using System;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture,Explicit]
	public class TrickyAjaxFormFilling
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.Driver = typeof (SeleniumWebDriver);
			Configuration.Browser = Drivers.Browser.Firefox;
			Configuration.Timeout = TimeSpan.FromSeconds(5);
			Configuration.RetryInterval = TimeSpan.FromSeconds(0.5);
		}

        public Session browser { get { return Browser.Session; } }

		[TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

		[Test]
		public void AutotraderCurrentAPI()
		{
			Configuration.AppHost = "www.autotrader.co.uk";
			browser.Visit("/used-cars");

            browser.Within(() => browser.FindCss("form.searchForm"), () =>
            {
                browser.FillIn("postcode").With("N1 1AA");

                browser.Select("citroen").From("make");
                browser.Select("c4_grand_picasso").From("model");

                browser.Select("National").From("radius");
                browser.Select("diesel").From("fuel-type");
                browser.Select("up_to_7_years_old").From("maximum-age");
                browser.Select("up_to_60000_miles").From("maximum-mileage");

                browser.FillIn("Add keyword:").With("vtr");

                browser.ClickButton("search-used-vehicles");
            });
		}

		[Test]
		public void AutotraderDesiredAPI()
		{
			// Would like this to worked with this script, written entirely by eye -- no firebug.
			// Mostly possible I think, by falling back to looser matches if the exact match does not exist.

			Configuration.AppHost = "www.autotrader.co.uk";

			browser.Visit("/used-cars");

			browser.FillIn("Postcode").With("N1 1AA"); // Match label text before ':'

			browser.Select("CITROEN").From("Make"); // If no exact match, match value starts with; ignore case on id/name;
			browser.Select("C4 GRAND PICASSO").From("Model"); // If no exact Match, match value starts with; ignore case on id/name;

			browser.Select("National").From("Distance"); // Find labels with display:none / ignore case on id/name;
			browser.Select("Diesel").From("Fuel type"); // ignore case AND try match with '-' or '_' replaced with whitespace on id/name;
			browser.Select("Up to 7 years old").From("Age"); // ignore case on id/name;
			browser.Select("Up to 60,000 miles").From("Mileage"); //ignore case on id/name;

			browser.FillIn("Add keyword").With("VTI"); // Match label text before ':'

			browser.ClickButton("Search"); // // If no exact match, match name starts with (this may be too loose to be helpful)
		}

		[Test]
		public void NewTwitter()
		{
			Configuration.AppHost = "www.twitter.com";
			browser.Visit("/");

			browser.FillIn("session[username_or_email]").With("coyputester");
			browser.FillIn("session[password]").With("nappybara");

			browser.ClickButton("Sign in");
			browser.ClickLink("find some interesting people");
			browser.ClickLink("Technology");
			browser.ClickLink("dickc");
		}

        [Test]
        public void CarBuzz()
        {
            Configuration.AppHost = "carbuzz.heroku.com";

            browser.Visit("/car_search");
            browser.WithinSection("Make",
                                  () =>
                                      {
                                          browser.Check("Audi");
                                          browser.Check("Ford");
                                          browser.Check("Maserati");
                                      });
            browser.WithinSection("Seats", () => browser.ClickButton("4"));

            Assert.That(browser.HasContent("11 car reviews found"));
        }
	}
}