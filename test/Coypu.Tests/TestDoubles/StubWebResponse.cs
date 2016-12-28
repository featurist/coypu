using System;
using System.IO;
using System.Net;

namespace Coypu.Tests.TestDoubles
{
    public class StubWebResponse : WebResponse
    {
        private readonly Stream responseStream = new MemoryStream();

        public override long ContentLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ContentType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Uri ResponseUri
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Stream GetResponseStream()
        {
            return responseStream;
        }


        //public override void Close()
        //{
        //    responseStream.Dispose();
        //    base.Close();
        //}
    }
}