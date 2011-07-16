using System.Collections.Generic;
using System.Net;

namespace Coypu.WebRequests
{
    internal interface RequestCookieInjector
    {
        WebRequest InjectCookies(WebRequest httpRequest, IEnumerable<Cookie> enumerable);
    }
}