using System;
using System.Runtime.Serialization;

namespace Coypu
{
    internal class AmbiguousHtmlException : Exception
    {
        public AmbiguousHtmlException(string message)
            : base(message)
        {
        }

        public AmbiguousHtmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AmbiguousHtmlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}