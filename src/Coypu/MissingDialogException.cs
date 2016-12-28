using System;

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
    }
}