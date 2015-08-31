using System;
using System.Runtime.Serialization;

namespace Coypu.Should
{
    [Serializable]
    public class ShouldException : Exception
    {
        public ShouldException()
        {
        }

        public ShouldException(string message)
            : base(message)
        {
        }

        public ShouldException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ShouldException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}