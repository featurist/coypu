using System;
using System.Runtime.Serialization;

namespace Coypu.Drivers
{
    public class MissingHtmlException : Exception
    {
        public MissingHtmlException(string message) : base(message)
        {
        }

        public MissingHtmlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingHtmlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}