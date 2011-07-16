using System.Collections.Generic;
using System.Net;

namespace Coypu.WebRequests
{
    internal interface RestrictedResourceDownloader
    {
        void SetCookies(IEnumerable<Cookie> getBrowserCookies);
        void DownloadFile(string resource, string saveAs);
    }
}