using System;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever an expected browser window cannot be found
    /// </summary>
    public class MissingWindowException : FinderException
    {
        public MissingWindowException(string message)
            : base(message)
        {
        }

        public MissingWindowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}