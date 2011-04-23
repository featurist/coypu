using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
	[TestFixture]
	public class TrickyAjaxFormFilling
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.Browser = Drivers.Browser.Firefox;
			Configuration.WebDriver = typeof (SeleniumWebDriver);
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

			session.FillIn("postcode").With("N1 1AA\t");

			session.Click(session.FindField("make"));

			session.Select("citroen").From("make");
			session.Select("c4_grand_picasso").From("model");

			session.Select("National").From("radius");
			session.Select("diesel").From("fuel-type");
			session.Select("up_to_7_years_old").From("maximum-age");
			session.Select("up_to_60000_miles").From("maximum-mileage");

			session.FillIn("Add keyword:").With("VTI");

			session.ClickButton("search-used-vehicles");
		}

		[Test]
		public void AutotraderDesiredAPI()
		{
			// Configuration.AppHost = "http://www.autotrader.co.uk";

			var session = Browser.Session;
			session.Visit("used-cars");

			session.FillIn("Postcode").With("N1 1AA\t");

			session.Select("CITROEN").From("Make");
			session.Select("C4 GRAND PICASSO").From("Model");

			session.Select("National").From("Distance");
			session.Select("Diesel").From("Fuel");
			session.Select("Up to 7 years old").From("Age");
			session.Select("Up to 60,000 miles").From("Mileage");

			session.FillIn("Add keyword").With("VTI");

			session.ClickButton("Search");
		}
	}
}