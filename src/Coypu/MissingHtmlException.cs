using System;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever some expected HTML cannot be found
    /// </summary>
    public class MissingHtmlException : FinderException
    {
        public MissingHtmlException(string message)
            : base(message)
        {
        }

        public MissingHtmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}