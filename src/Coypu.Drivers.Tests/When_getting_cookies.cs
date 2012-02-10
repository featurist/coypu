using System;
using System.Linq;

using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_getting_cookies : DriverSpecs {
        internal override void Specs()
        {
            before = () => 
            {
                driver.Visit("http://localhost:4567/resource/cookie_test");
                driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '");
                driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC;  path=/resource'");
                driver.ExecuteScript("document.cookie = 'cookie2=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '");
                driver.Visit("http://localhost:4567/resource/cookie_test");
            };
            
            it["gets all the session cookies"] = () =>
            {
                driver.ExecuteScript("document.cookie = 'cookie1=value1; '");
                driver.ExecuteScript("document.cookie = 'cookie2=value2; '");
                
                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.First(c => c.Name == "cookie1").Value, Is.EqualTo("value1"));
                Assert.That(cookies.First(c => c.Name == "cookie2").Value, Is.EqualTo("value2"));
            };

            it["gets all the persistent cookies"] = () =>
            {
                var expires = DateTime.UtcNow.AddDays(2);

                driver.ExecuteScript(string.Format("document.cookie = 'cookie1=value11; expires={0} '", expires.ToString("R")));
                driver.ExecuteScript(string.Format("document.cookie = 'cookie2=value22; expires={0} '", expires.ToString("R")));

                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.First(c => c.Name == "cookie1").Value, Is.EqualTo("value11"));
                Assert.That(cookies.First(c => c.Name == "cookie2").Value, Is.EqualTo("value22"));
            };

            // Internet Explorer fails this test - cookie information with path isn't available,
            // unless it's a persistent cookie that's been retrieved from the cache (and even then
            // the path value seems to be wrong?)
            it["gets the cookie path"] = () =>
            {
                driver.ExecuteScript("document.cookie = 'cookie1=value1; path=/resource'");

                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.First(c => c.Name == "cookie1").Path, Is.EqualTo("/resource"));
            };
        }
    }
}