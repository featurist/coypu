using System;
using Coypu.Robustness;

namespace Coypu
{
    public static class Browser
    {
        private static Session session;
        public static Session Session
        {
            get
            {
                if (session == null || session.WasDisposed)
                {
                    StartNewSession();
                }
                return session;
            }
        }

        private static void StartNewSession()
        {
            session = new Session(NewWebDriver(), new RetryUntilTimeoutRobustWrapper());
        }

        private static Driver NewWebDriver()
        {
            return (Driver) Activator.CreateInstance(Configuration.Driver);
        }

        public static void EndSession()
        {
            if (session != null) session.Dispose();
        }
    }
}