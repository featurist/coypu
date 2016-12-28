using System;

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
    }
}