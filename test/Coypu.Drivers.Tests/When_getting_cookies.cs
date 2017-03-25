using System;
using System.Linq;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_getting_cookies : DriverSpecs
    {
        public When_getting_cookies()
        {
            Driver.Visit(TestSiteUrl("/resource/cookie_test"), Root);
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie1=; expires=Fri, 27 Jul 2001 02:47:11 UTC;  path=/resource'", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=; expires=Fri, 27 Jul 2001 02:47:11 UTC; '", Root);
            Driver.Visit(TestSiteUrl("/resource/cookie_test"), Root);
        }
        
        [Fact]
        public void Gets_all_the_session_cookies()
        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=value2; '", Root);

            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.Equal("value1", cookies.First(c => c.Name == "cookie1").Value);
            Assert.Equal("value2", cookies.First(c => c.Name == "cookie2").Value);
        }

        [Fact]
        public void Gets_all_the_persistent_cookies()
        {
            var expires = DateTime.UtcNow.AddDays(2);

            Driver.ExecuteScript(string.Format("document.cookie = 'cookie1=value11; expires={0} '", expires.ToString("R")), Root);
            Driver.ExecuteScript(string.Format("document.cookie = 'cookie2=value22; expires={0} '", expires.ToString("R")), Root);


            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.Equal("value11", cookies.First(c => c.Name == "cookie1").Value);
            Assert.Equal("value22", cookies.First(c => c.Name == "cookie2").Value);
        }

        // Internet Explorer fails this test - cookie information with path isn't available,
        // unless it's a persistent cookie that's been retrieved from the cache (and even then
        // the path value seems to be wrong?)
        [Fact]
        public void Gets_the_cookie_path()

        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; path=/resource'", Root);

            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.Equal("/resource", cookies.First(c => c.Name == "cookie1").Path);
        }
    }
}