using System.Collections.Generic;
using System.Net;
using Coypu.WebRequests;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRestrictedResourceDownloader : RestrictedResourceDownloader
    {
        private readonly IList<DownloadedFile> downloadedFiles = new List<DownloadedFile>();

        public IList<DownloadedFile> DownloadedFiles
        {
            get { return downloadedFiles; }
        }

        public void SetCookies(IEnumerable<Cookie> cookies)
        {
            this.Cookies = cookies;
        }

        protected IEnumerable<Cookie> Cookies { get; private set; }

        public void DownloadFile(string resource, string saveAs)
        {
            DownloadedFiles.Add(new DownloadedFile(resource, saveAs, Cookies));
        }
    }

    public class DownloadedFile
    {
        public string Resource { get; private set; }
        public string SaveAs { get; private set; }
        public IEnumerable<Cookie> Cookies { get; set; }

        public DownloadedFile(string resource, string saveAs, IEnumerable<Cookie> cookies)
        {
            Resource = resource;
            SaveAs = saveAs;
            Cookies = cookies;
        }
    }
}