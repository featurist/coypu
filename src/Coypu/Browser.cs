using System;
using Coypu.Robustness;

namespace Coypu
{
    public static class Browser
    {
        private static Session session;

        /// <summary>
        /// The current browser session. Will start a new session if one does not already exist.
        /// </summary>
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
            session = new Session(NewWebDriver(), new RetryUntilTimeoutRobustWrapper(), new ThreadSleepWaiter(), null, null, new ConfiguredHostUrlBuilder());
        }

        private static Driver NewWebDriver()
        {
            return (Driver) Activator.CreateInstance(Configuration.Driver);
        }

        /// <summary>
        /// End the current session, closing any open browser.
        /// </summary>
        public static void EndSession()
        {
            if (SessionIsActive) session.Dispose();
        }
        
        /// <summary>
        /// Whether there is an active session
        /// </summary>
        public static bool SessionIsActive
        {
            get {return session != null;}
        }
    }
}