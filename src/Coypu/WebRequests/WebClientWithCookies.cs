using System;
using System.Collections.Generic;
using System.Net;

namespace Coypu.WebRequests
{
    internal class WebClientWithCookies : WebClient, RestrictedResourceDownloader
    {
        private IEnumerable<Cookie> requestCookies;
        private readonly WebRequestCookieInjector webRequestCookieInjector;

        public WebClientWithCookies()
        {
            webRequestCookieInjector = new WebRequestCookieInjector();
        }

        public void SetCookies(IEnumerable<Cookie> cookies)
        {
            requestCookies = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            return webRequestCookieInjector.InjectCookies(base.GetWebRequest(address), requestCookies);
        }
    }
}