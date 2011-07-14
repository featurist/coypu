using System;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_getting_cookies : DriverSpecs
    {
        internal override void Specs()
        {
            it["gets all the cookies"] = () =>
            {
                driver.ExecuteScript("document.cookie = 'cookie1=value1; expires=Thu, 2 Aug 2001 20:47:11 UTC; path=/'");
                driver.ExecuteScript("document.cookie = 'cookie2=value2; expires=Thu, 23 Oct 2011 20:47:11 UTC; path=/hello/'");
                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.Length, Is.EqualTo(2), "Missing cookies");

                CheckCookie(cookies[0], "cookie1", "value1", DateTime.Parse("Thu, 2 Aug 2001 20:47:11 UTC"), "/");
                CheckCookie(cookies[1], "cookie2", "value2", DateTime.Parse("Thu, 23 Oct 2011 20:47:11 UTC"), "/hello/");
            };
        }

        private void CheckCookie(Cookie cookie, string name, string value, DateTime expires, string path)
        {
            Assert.That(cookie.Name, Is.EqualTo(name));
            Assert.That(cookie.Value,Is.EqualTo(value));
            Assert.That(cookie.Expires, Is.EqualTo(expires));
            Assert.That(cookie.Path,Is.EqualTo(path));
        }
    }
}