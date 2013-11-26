using System;
using System.Runtime.Serialization;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever some expected HTML cannot be found
    /// </summary>
    public class MissingHtmlException : Exception
    {
        public MissingHtmlException(string message) : base(message)
        {
        }

        public MissingHtmlException(string message, Exception innerException) : base(message, innerException)
        {                         
        }

        public MissingHtmlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}