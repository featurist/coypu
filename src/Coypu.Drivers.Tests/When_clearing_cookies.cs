using System;
using System.Linq;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_clearing_cookies : DriverSpecs
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
        public void Clear_all_the_session_cookies()
        {
            Driver.ExecuteScript("document.cookie = 'cookie1=value1; '", Root);
            Driver.ExecuteScript("document.cookie = 'cookie2=value2; '", Root);

            var cookies = Driver.GetBrowserCookies().ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1").Value, Is.EqualTo("value1"));
            Assert.That(cookies.First(c => c.Name == "cookie2").Value, Is.EqualTo("value2"));

            Driver.ClearBrowserCookies();
            
            Assert.AreEqual(0, Driver.GetBrowserCookies().Count());
        }
    }
}