using System.Collections.Generic;
using System.Net;

namespace Coypu
{
    internal class WebClientResourceDownloader : ResourceDownloader
    {
        public void DownloadFile(string resource, string saveAs, IEnumerable<Cookie> cookies)
        {
            new WebClientWithCookies(cookies).DownloadFile(resource, saveAs);
        }

    }
}