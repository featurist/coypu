using System;
using System.Runtime.Serialization;

namespace Coypu
{
    public class AmbiguousException : Exception
    {
        public AmbiguousException(string message)
            : base(message)
        {
        }

        public AmbiguousException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AmbiguousException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}