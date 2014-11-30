using System;
using System.Runtime.Serialization;

namespace Coypu
{
    public class FinderException : Exception
    {
        public FinderException(string message) : base(message)
        {
        }

        public FinderException(string message, Exception innerException) : base(message, innerException)
        {                         
        }

        public FinderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}