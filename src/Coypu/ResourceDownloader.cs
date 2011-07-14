using System.Collections.Generic;
using System.Net;

namespace Coypu
{
    internal interface ResourceDownloader
    {
        void DownloadFile(string resource, string saveAs, IEnumerable<Cookie> cookies);
    }
}