using System;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture,Explicit]
	public class Coypu
	{
	    [SetUp]
		public void SetUp()
		{
			Configuration.Timeout = TimeSpan.FromSeconds(5);
		}

	    public Session browser { get { return Browser.Session; } }

	    [TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

	    [Test]
	    public void FindingStuff_CarBuzz()
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
            
	        Assert.That(browser.HasContent(" 30 car reviews found"));

	        browser.WithinSection("Seats", () => browser.ClickButton("4"));

            browser.WithinSection("Features",
                () =>
                {
                    browser.ClickButton("Hybrid Engine");
                    browser.ClickButton("Off-Road Capability");
                });

            Assert.That(browser.HasContent(" 8 car reviews found"));
	    }

	    [Test]
		public void Retries_AutotraderCurrentAPI()
		{
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
	        browser.Visit("http://www.twitter.com");

	        browser.FillIn("session[username_or_email]").With("coyputester2");
	        browser.FillIn("session[password]").With("Nappybara");

	        browser.ClickButton("Sign in");
	    }






	    [Test]
        public void FollowMeOnTwitter()
        {
            browser.Visit("http://www.twitter.com");

            browser.FillIn("session[username_or_email]").With("coyputester2");
            browser.FillIn("session[password]").With("Nappybara");

            browser.ClickButton("Sign in");

            browser.FillIn("find users by name").With("adrianlongley");
            browser.ClickButton("Search");

            browser.ClickLink("adrianlongley");
            browser.ClickLink("@adrianlongley view full profile →");

            browser.ClickButton("Follow");

            browser.FindButton("Unfollow");
        }

	    [Test]
		public void NewTwitterGojko()
		{
			Configuration.AppHost = "www.twitter.com";
			browser.Visit("/");

			browser.FillIn("session[username_or_email]").With("coyputester2");
			browser.FillIn("session[password]").With("Nappybara");
			browser.ClickButton("Sign in");

			browser.FillIn("find users by name").With("gojko");
            browser.ClickButton("Search");
		}
	}
}