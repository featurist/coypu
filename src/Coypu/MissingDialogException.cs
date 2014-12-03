using System;
using System.Runtime.Serialization;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever some expected HTML cannot be found
    /// </summary>
    public class MissingDialogException : FinderException
    {
        public MissingDialogException(string message)
            : base(message)
        {
        }

        public MissingDialogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public MissingDialogException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}