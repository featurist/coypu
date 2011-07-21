using System.Linq;
using Coypu.Drivers.Tests.Sites;
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
            
            it["gets all the cookies"] = () =>
            {
                driver.ExecuteScript("document.cookie = 'cookie1=value1; '");
                driver.ExecuteScript("document.cookie = 'cookie2=value2; '");
                
                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.First(c => c.Name == "cookie1").Value, Is.EqualTo("value1"));
                Assert.That(cookies.First(c => c.Name == "cookie2").Value, Is.EqualTo("value2"));
            };

            it["gets the cookie path"] = () =>
            {
                driver.ExecuteScript("document.cookie = 'cookie1=value1; path=/resource'");

                var cookies = driver.GetBrowserCookies().ToArray();

                Assert.That(cookies.First(c => c.Name == "cookie1").Path, Is.EqualTo("/resource"));
            };
        }

    }
}