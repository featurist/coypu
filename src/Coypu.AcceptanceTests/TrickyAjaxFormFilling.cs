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
			Configuration.RegisterDriver = typeof (SeleniumWebDriver);
			Configuration.Browser = Drivers.Browser.Firefox;
			Configuration.Timeout = TimeSpan.FromSeconds(5);
			Configuration.RetryInterval = TimeSpan.FromSeconds(0.5);
		}

		[TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

		[Test]
		public void AutotraderCurrentAPI()
		{
			var session = Browser.Session;
			session.Visit("http://www.autotrader.co.uk/used-cars");

            session.Within(() => session.FindCss("form.searchForm"), () =>
            {
                session.FillIn("postcode").With("N1 1AA");

                session.Select("citroen").From("make");
                session.Select("c4_grand_picasso").From("model");

                session.Select("National").From("radius");
                session.Select("diesel").From("fuel-type");
                session.Select("up_to_7_years_old").From("maximum-age");
                session.Select("up_to_60000_miles").From("maximum-mileage");

                session.FillIn("Add keyword:").With("vtr");

                session.ClickButton("search-used-vehicles");
            });
		}

		[Test]
		public void AutotraderDesiredAPI()
		{
			// Would like this to worked with this script, written entirely by eye -- no firebug.
			// Mostly possible I think, by falling back to looser matches if the exact match does not exist.

			// Configuration.AppHost = "http://www.autotrader.co.uk";

			var session = Browser.Session;
			session.Visit("used-cars"); // Add AppHost setting

			session.FillIn("Postcode").With("N1 1AA"); // Match label text before ':'

			session.Select("CITROEN").From("Make"); // If no exact match, match value starts with; ignore case on id/name;
			session.Select("C4 GRAND PICASSO").From("Model"); // If no exact Match, match value starts with; ignore case on id/name;

			session.Select("National").From("Distance"); // Find labels with display:none / ignore case on id/name;
			session.Select("Diesel").From("Fuel type"); // ignore case AND try match with '-' or '_' replaced with whitespace on id/name;
			session.Select("Up to 7 years old").From("Age"); // ignore case on id/name;
			session.Select("Up to 60,000 miles").From("Mileage"); //ignore case on id/name;

			session.FillIn("Add keyword").With("VTI"); // Match label text before ':'

			session.ClickButton("Search"); // // If no exact match, match name starts with (this may be too loose to be helpful)
		}

		[Test]
		public void NewTwitter()
		{
			var session = Browser.Session;
			session.Visit("http://www.twitter.com");

			session.FillIn("session[username_or_email]").With("coyputester");
			session.FillIn("session[password]").With("nappybara");

			session.ClickButton("Sign in");
			session.ClickLink("find some interesting people");
			session.ClickLink("Technology");
			session.ClickLink("dickc");

		}
	}
}