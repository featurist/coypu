using System;

namespace Coypu
{
    internal class ZeroTimeoutQueryBuilder
    {
        internal Func<bool> BuildZeroTimeoutHasElementQuery(ElementScope elementScope)
        {
            Func<bool> query =
                () =>
                    {
                        var outerTimeout = Configuration.Timeout;
                        Configuration.Timeout = TimeSpan.Zero;
                        try
                        {
                            elementScope.Now();
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