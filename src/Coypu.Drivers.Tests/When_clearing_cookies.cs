using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Coypu.Drivers.Tests
{
    internal class When_clearing_cookies : DriverSpecs
    {
        [SetUp]
        public void SetUpCookies()
        {
            Driver.Visit(TestSiteUrl("/"), Root);
        }

        [Test]
        public void Delete_all_the_session_cookies()
        {
            Driver.Cookies.AddCookie(new Cookie("cookie1", "value1"));
            Driver.Cookies.AddCookie(new Cookie("cookie2", "value2"));

            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies.First(c => c.Name == "cookie1")
                               .Value,
                        Is.EqualTo("value1"));
            Assert.That(cookies.First(c => c.Name == "cookie2")
                               .Value,
                        Is.EqualTo("value2"));

            Driver.Cookies.DeleteAll();

            Assert.AreEqual(0,
                            Driver.Cookies.GetAll()
                                  .Count());
        }

        [Test]
        public void Delete_cookie()
        {
            var specialCookie = new Cookie("specialCookie", "specialValue");
            Driver.Cookies.AddCookie(specialCookie);
            Driver.Cookies.AddCookie(new Cookie("cookie2", "value2"));

            var cookieCount = Driver.Cookies.GetAll()
                                    .Count();
            var expectedCookieCount = cookieCount - 1;

            Driver.Cookies.DeleteCookie(specialCookie);
            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies, Does.Not.Contain(cookies.Any(c => c.Name == "specialCookie")));
            Assert.That(cookies.First(c => c.Name == "cookie2")
                               .Value,
                        Is.EqualTo("value2"));
            Assert.AreEqual(expectedCookieCount,
                            Driver.Cookies.GetAll()
                                  .Count());
        }

        [Test]
        public void Delete_cookie_by_name()
        {
            Driver.Cookies.AddCookie(new Cookie("cookie1", "value1"));
            Driver.Cookies.AddCookie(new Cookie("cookie2", "value2"));
            var cookieCount = Driver.Cookies.GetAll()
                                    .Count();
            var expectedCookieCount = cookieCount - 1;

            Driver.Cookies.DeleteCookieNamed("cookie1");
            var cookies = Driver.Cookies.GetAll()
                                .ToArray();

            Assert.That(cookies, Does.Not.Contain(cookies.Any(c => c.Name == "cookie1")));
            Assert.That(cookies.First(c => c.Name == "cookie2")
                               .Value,
                        Is.EqualTo("value2"));
            Assert.AreEqual(expectedCookieCount,
                            Driver.Cookies.GetAll()
                                  .Count());
        }
    }
}