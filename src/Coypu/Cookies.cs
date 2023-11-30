using System.Collections.Generic;
using System.Net;

namespace Coypu
{
    public interface Cookies
    {
        public void AddCookie(Cookie cookie, Options options = null);
        public void DeleteAll();
        public void DeleteCookie(Cookie cookie);
        public void DeleteCookieNamed(string cookieName);
        public IEnumerable<Cookie> GetAll();
        public Cookie GetCookieNamed(string cookieName);
        public void WaitUntilCookieExists(Cookie cookie, Options options);
    }
}
