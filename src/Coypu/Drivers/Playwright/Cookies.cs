using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
{
  // Implement the interface using a PlaywrightContext
    public class Cookies : Coypu.Cookies
    {
        private readonly IPage _page;

        public Cookies(IPage page)
        {
            _page = page;
        }

    public void AddCookie(System.Net.Cookie cookie, Options options = null)
    {
        var expires = ((DateTimeOffset)cookie.Expires).ToUnixTimeMilliseconds();
        Async.WaitForResult(_page.Context.AddCookiesAsync(new[] {PlaywrightCookie(cookie)}));
    }

    public async void DeleteAll()
    {
        Async.WaitForResult(_page.Context.ClearCookiesAsync());
    }

    public void DeleteCookie(System.Net.Cookie cookie)
    {
      DeleteCookieNamed(cookie.Name);
    }

    public void DeleteCookieNamed(string cookieName)
    {
        var filteredCookies = GetAll().Where((cookie) => cookie.Name != cookieName);
        DeleteAll();
        _page.Context.AddCookiesAsync(filteredCookies.Select(PlaywrightCookie));
    }

    private Cookie PlaywrightCookie(System.Net.Cookie cookie)
    {
        var expires = ((DateTimeOffset)cookie.Expires).ToUnixTimeMilliseconds();
        return new Cookie
        {
            Name = cookie.Name,
            Value = cookie.Value,
            Domain = cookie.Domain,
            Path = cookie.Path,
            Url = string.IsNullOrEmpty(cookie.Domain) ? _page.Url : null,
            Expires = expires < 1 ? -1 : expires,
            HttpOnly = cookie.HttpOnly,
            Secure = cookie.Secure
        };
    }

    public IEnumerable<System.Net.Cookie> GetAll()
    {
        return Async.WaitForResult(_page.Context.CookiesAsync()).Select(
            c => new System.Net.Cookie{
                Name = c.Name,
                Value = c.Value,
                Domain = c.Domain,
                Path = c.Path,
                Expires = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(c.Expires)).DateTime,
                HttpOnly = c.HttpOnly,
                Secure = c.Secure
            }
        );
    }

    public System.Net.Cookie GetCookieNamed(string cookieName)
    {
        return GetAll().Where((cookie) => cookie.Name == cookieName).FirstOrDefault();
    }

    public void WaitUntilCookieExists(System.Net.Cookie cookie,
                                          Options options)
    {
          var stopWatch = Stopwatch.StartNew();
          try
          {
              while (stopWatch.ElapsedMilliseconds < options.Timeout.TotalMilliseconds)
              {
                  var allCookies = GetAll();
                  if (allCookies.Any(x => x.Name.Trim() == cookie.Name))
                  {
                      Console.WriteLine($"\t-> Cookie name '{cookie.Name}' exists.");
                      break;
                  }

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
