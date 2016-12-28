using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Coypu.WebRequests
{
    internal class WebClientWithCookies : RestrictedResourceDownloader
    {
        private IEnumerable<Cookie> requestCookies;

        public WebClientWithCookies()
        {
        }

        public void DownloadFile(string resource, string saveAs)
        {
            
        }

        public void SetCookies(IEnumerable<Cookie> getBrowserCookies)
        {
            
        }
    }
}