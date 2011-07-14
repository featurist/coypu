using System;
using System.Collections.Generic;
using System.Net;

namespace Coypu
{
    internal class WebClientWithCookies : WebClient
    {
        private readonly IEnumerable<Cookie> cookies;

        public WebClientWithCookies(IEnumerable<Cookie> cookies)
        {
            this.cookies = cookies;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            return request is HttpWebRequest 
                ? InjectCookies((HttpWebRequest)request)
                : request;
        }

        private HttpWebRequest InjectCookies(HttpWebRequest httpRequest)
        {
            httpRequest.CookieContainer = new CookieContainer();

            foreach (var cookie in cookies)
                httpRequest.CookieContainer.Add(cookie);

            return httpRequest;
        }
    }
}