using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture, Explicit]
    public class Coypu
    {
        
        [Test]
        public void Retries_Autotrader()
        {
            var browser = Browser.Session;
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
            var browser = Browser.Session;
            browser.Visit("http://www.twitter.com");

        }















        [Test]
        public void FindingStuff_CarBuzz()
        {
            var browser = Browser.Session;
            browser.Visit("http://carbuzz.heroku.com/car_search");
        }
    }
}