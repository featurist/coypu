using System.Collections.Generic;
using System.Net;

namespace Coypu.Tests.TestDoubles
{
    public class SpyResourceDownloader : ResourceDownloader
    {
        private readonly IList<DownloadedFile> downloadedFiles = new List<DownloadedFile>();

        public IList<DownloadedFile> DownloadedFiles
        {
            get { return downloadedFiles; }
        }

        public void DownloadFile(string resource, string saveAs, IEnumerable<Cookie> cookies)
        {
            DownloadedFiles.Add(new DownloadedFile(resource, saveAs, cookies));
        }
    }

    public class DownloadedFile
    {
        public string Resource { get; private set; }
        public string SaveAs { get; private set; }
        public IEnumerable<Cookie> Cookies { get; private set; }

        public DownloadedFile(string resource, string saveAs, IEnumerable<Cookie> cookies)
        {
            Cookies = cookies;
            Resource = resource;
            SaveAs = saveAs;
        }
    }
}