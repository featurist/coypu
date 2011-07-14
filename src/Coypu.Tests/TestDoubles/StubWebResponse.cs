using System.IO;
using System.Net;

namespace Coypu.Tests.TestDoubles
{
    public class StubWebResponse : WebResponse
    {
        private readonly Stream responseStream = new MemoryStream();

        public override Stream GetResponseStream()
        {
            return responseStream;
        }

        public override void Close()
        {
            responseStream.Dispose();
            base.Close();
        }
    }
}