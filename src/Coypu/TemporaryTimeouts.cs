using System;

namespace Coypu
{
    internal class TemporaryTimeouts
    {
        internal T WithIndividualTimeout<T>(TimeSpan individualTimeout, Func<T> function)
        {
            var defaultTimeout = Configuration.Timeout;
            Configuration.Timeout = individualTimeout;
            try
            {
                return function();
            }
            finally
            {
                Configuration.Timeout = defaultTimeout;
            }
        }
    }
}