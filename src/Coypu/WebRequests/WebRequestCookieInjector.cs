using System.Collections.Generic;
using System.Net;

namespace Coypu.WebRequests
{
    internal class WebRequestCookieInjector
    {
        internal WebRequest InjectCookies(WebRequest webRequest, IEnumerable<Cookie> cookies)
        {
            WebRequest request = webRequest;

            return request is HttpWebRequest httpRequest
                       ? AddCookiesToCookieContainer(httpRequest, cookies)
                       : request;
        }

        internal static HttpWebRequest AddCookiesToCookieContainer(HttpWebRequest httpRequest,
                                                                   IEnumerable<Cookie> cookies)
        {
            httpRequest.CookieContainer = new CookieContainer();

            foreach (Cookie cookie in cookies)
                httpRequest.CookieContainer.Add(cookie);

            return httpRequest;
        }
    }
}