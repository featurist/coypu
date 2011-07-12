using System;
using System.Runtime.Serialization;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever some expected HTML cannot be found
    /// </summary>
    public class MissingHtmlException : Exception
    {
        internal MissingHtmlException(string message) : base(message)
        {
        }

        internal MissingHtmlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal MissingHtmlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}