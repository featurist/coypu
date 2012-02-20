using System.Linq;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_getting_cookies : DriverSpecs
    {
        [SetUp]
        internal void SetUpCookies()
        {
            Driver.Visit("http://localhost:4567/resource/cookie_test");
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '");
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC;  path=/resource'");
            Driver.ExecuteScript("document.cookie = 'cookie2=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '");
            Driver.Visit("http://localhost:4567/resource/cookie_test");
        }


        [Test]
        public void Gets_all_the_cookies()

        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; '");
            Driver.ExecuteScript("document.cookie = 'cookie2=value2; '");

            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1").Value, Is.EqualTo("value1"));
            Assert.That(cookies.First(c => c.Name == "cookie2").Value, Is.EqualTo("value2"));
        }


        [Test]
        public void Gets_the_cookie_path()

        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; path=/resource'");

            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1").Path, Is.EqualTo("/resource"));
        }
    }
}