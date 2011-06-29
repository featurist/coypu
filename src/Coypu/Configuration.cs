using System;
using Coypu.Drivers.Selenium;

namespace Coypu
{
    /// <summary>
    /// Global configuration settings
    /// </summary>
    public static class Configuration
    {
        const string DEFAULT_APP_HOST = "localhost";
        const int DEFAULT_PORT = 80;
        const double DEFAULT_TIMEOUT_SECONDS = 10;
        const double DEFAULT_INTERVAL_SECONDS = 0.1;

        static Configuration()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS);
            RetryInterval = TimeSpan.FromSeconds(DEFAULT_INTERVAL_SECONDS);
            Browser = Drivers.Browser.Firefox;
            Driver = typeof(SeleniumWebDriver);
        }

        /// <summary>
        /// <para>When retrying, how long to wait for elements to appear or actions to complete without error.</para>
        /// <para>Default: 10sec</para>
        /// </summary>
        public static TimeSpan Timeout { get; set; }

        /// <summary>
        /// <para>How long to wait between retries</para>
        /// <para>Default: 100ms</para>
        /// </summary>
        public static TimeSpan RetryInterval { get; set; }

        /// <summary>
        /// <para>Specifies the browser you would like to control</para>
        /// <para>Default: Firefox</para>
        /// </summary>
        public static Drivers.Browser Browser { get; set; }

        /// <summary>
        /// <para>Specifies the driver you would like to use to control the browser</para> 
        /// <para>Default: SeleniumWebDriver</para>
        /// </summary>
        public static Type Driver { get; set; }

        /// <summary>
        /// <para>How long to wait between finding an element and clicking it.</para>
        /// <para>Default: zero</para>
        /// </summary>
        public static TimeSpan WaitBeforeClick { get; set; }

        private static string appHost;

        /// <summary>
        /// <para>The host of the website you are testing, e.g. 'github.com'</para>
        /// <para>Default: localhost</para>
        /// </summary>
        public static string AppHost
        {
            get { return appHost == default(string) ? DEFAULT_APP_HOST : appHost;}
            set { appHost = value == null ? null : value.TrimEnd('/'); }
        }

        private static int port;

        /// <summary>
        /// <para>The port of the website you are testing</para>
        /// <para>Default: 80</para>
        /// </summary>
        public static int Port
        {
            get { return port == default(int) ? DEFAULT_PORT : port;}
            set { port = value; }
        }

        /// <summary>
        /// <para>Whether to use the HTTPS protocol to connect to website you are testing</para>
        /// <para>Default: false</para>
        /// </summary>
        public static bool SSL { get; set; }
    }
}