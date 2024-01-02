using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using Cookie = System.Net.Cookie;

namespace Coypu.Drivers.Selenium
{
    public class Cookies : Coypu.Cookies
    {
        private readonly IWebDriver _nativeDriver;

        public Cookies(IWebDriver nativeDriver)
        {
            _nativeDriver = nativeDriver;
        }

        public void AddCookie(Cookie cookie,
                              Options options = null)
        {
            try
            {
                _nativeDriver.Manage()
                             .Cookies.AddCookie(new OpenQA.Selenium.Cookie(
                                cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expires
                              ));
                _nativeDriver.Navigate()
                             .Refresh();
                WaitUntilCookieExists(cookie, options);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t-> Could not attach the cookie {cookie.Name} to the browser session. {e.Message}");
            }
        }

        public void DeleteAll()
        {
            try
            {
                _nativeDriver.Manage()
                             .Cookies.DeleteAllCookies();
                _nativeDriver.Navigate()
                             .Refresh();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t-> Could not delete the cookies from the browser session. {e.Message}");
            }
        }

        public void DeleteCookie(Cookie cookie)
        {
            try
            {
                _nativeDriver.Manage()
                             .Cookies.DeleteCookie(new OpenQA.Selenium.Cookie(
                                cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expires
                              ));
                _nativeDriver.Navigate()
                             .Refresh();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t-> Could not delete the cookie name '{cookie.Name}' from the browser session. {e.Message}");
            }
        }

        public void DeleteCookieNamed(string cookieName)
        {
            try
            {
                _nativeDriver.Manage()
                             .Cookies.DeleteCookieNamed(cookieName);
                _nativeDriver.Navigate()
                             .Refresh();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t-> Could not delete the cookie by name '{cookieName}' from the browser session. {e.Message}");
            }
        }

        public IEnumerable<Cookie> GetAll()
        {
            return _nativeDriver.Manage()
                                .Cookies.AllCookies.Select(c => new Cookie {
                                    Name = c.Name,
                                    Value = c.Value,
                                    Path = c.Path,
                                    Domain = c.Domain,
                                    Expires = DateTime.MaxValue
                                  });
        }

        public Cookie GetCookieNamed(string cookieName)
        {
            var cookie = _nativeDriver.Manage()
                                      .Cookies.GetCookieNamed(cookieName);
            if (cookie == null) Console.WriteLine($"\t-> Could not get cookie by name '{cookieName}' from the browser session.");
            return new Cookie(
                cookie.Name,
                cookie.Value,
                cookie.Path,
                cookie.Domain
            );
        }

        public void WaitUntilCookieExists(Cookie cookie,
                                          Options options)
        {
            var allCookies = _nativeDriver.Manage()
                                          .Cookies.AllCookies;
            var stopWatch = Stopwatch.StartNew();
            try
            {
                while (stopWatch.ElapsedMilliseconds < options.Timeout.TotalMilliseconds)
                {
                    if (allCookies.Any(x => x.Name.Trim() == cookie.Name))
                    {
                        Console.WriteLine($"\t-> Cookie name '{cookie.Name}' exists.");
                        break;
                    }

                    allCookies = _nativeDriver.Manage()
                                              .Cookies.AllCookies;
                    Thread.Sleep(options.RetryInterval);
                }

                stopWatch.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t-> Cookie name '{cookie.Name}' does NOT exist. After {stopWatch.Elapsed.TotalSeconds} seconds. {e.Message}");
            }
        }
    }
}
