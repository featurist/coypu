using System;
using System.Linq;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_getting_cookies : DriverSpecs
    {
        [SetUp]
        public void SetUpCookies()
        {
            Driver.Visit(TestSiteUrl("/resource/cookie_test"), Root);
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC;  path=/resource'", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '", Root);
            Driver.Visit(TestSiteUrl("/resource/cookie_test"), Root);
        }

        [Test]
        public void Gets_all_the_session_cookies()
        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=value2; '", Root);

            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1")
                               .Value,
                        Is.EqualTo("value1"));
            Assert.That(cookies.First(c => c.Name == "cookie2")
                               .Value,
                        Is.EqualTo("value2"));
        }

        [Test]
        public void Gets_all_the_persistent_cookies()
        {
            var expires = DateTime.UtcNow.AddDays(2);

            Driver.ExecuteScript($"document.cookie = 'cookie1=value11; expires={expires:R} '", Root);
            Driver.ExecuteScript($"document.cookie = 'cookie2=value22; expires={expires:R} '", Root);


            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1")
                               .Value,
                        Is.EqualTo("value11"));
            Assert.That(cookies.First(c => c.Name == "cookie2")
                               .Value,
                        Is.EqualTo("value22"));
        }

        [Test]
        public void Gets_the_cookie_by_name()

        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=value2; '", Root);

            var cookies = Driver.Cookies.GetCookieNamed("cookie1");

            Assert.That(cookies.Value == "value1");
        }

        // Internet Explorer fails this test - cookie information with path isn't available,
        // unless it's a persistent cookie that's been retrieved from the cache (and even then
        // the path value seems to be wrong?)
        [Test]
        public void Gets_the_cookie_path()

        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; path=/resource'", Root);

            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1")
                               .Path,
                        Is.EqualTo("/resource"));
        }
    }
}