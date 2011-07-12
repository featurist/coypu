using System;

namespace Coypu
{
    internal class ZeroTimeoutQueryBuilder
    {
        internal Func<bool> BuildZeroTimeoutHasElementQuery(Func<Element> findElement)
        {
            Func<bool> query =
                () =>
                    {
                        var outerTimeout = Configuration.Timeout;
                        Configuration.Timeout = TimeSpan.Zero;
                        try
                        {
                            findElement();
                            return true;
                        }
                        catch (MissingHtmlException)
                        {
                            return false;
                        }
                        finally
                        {
                            Configuration.Timeout = outerTimeout;
                        }
                    };
            return query;
        }
    }
}