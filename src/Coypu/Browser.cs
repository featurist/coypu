    using System;
    using System.Reflection;
    using Coypu.Robustness;
using Coypu.WebRequests;

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
            session = NewSession();
        }

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <returns>The new session</returns>
        public static Session NewSession()
        {
            return new Session(NewWebDriver(),
                               new RetryUntilTimeoutRobustWrapper(),
                               new ThreadSleepWaiter(),
                               new WebClientWithCookies(),
                               new CurrentConfigurationUrlBuilder());
        }

        public static int OpenDrivers { get; set; }

        private static Driver NewWebDriver()
        {
            try
            {
                Console.Write("New  driver...");

                var driver = (Driver) Activator.CreateInstance(Configuration.Driver);

                Console.WriteLine("open.");
                OpenDrivers++;
                Console.WriteLine(OpenDrivers + " drivers open.");

                return driver;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
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
            get { return session != null; }
        }
    }
}