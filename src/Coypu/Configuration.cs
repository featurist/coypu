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
        
        private static string appHost;

        static Configuration()
        {
            Reset();
        }

        /// <summary>
        /// Reset all configuration settings to their default values
        /// </summary>
        public static void Reset()
        {
            AppHost = DEFAULT_APP_HOST;
            Port = DEFAULT_PORT;
            SSL = false;
            Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS);
            RetryInterval = TimeSpan.FromSeconds(DEFAULT_INTERVAL_SECONDS);
            Browser = Drivers.Browser.Firefox;
            Driver = typeof(SeleniumWebDriver);
            WaitBeforeClick = TimeSpan.Zero;
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


        /// <summary>
        /// <para>The host of the website you are testing, e.g. 'github.com'</para>
        /// <para>Default: localhost</para>
        /// </summary>
        public static string AppHost
        {
            get { return appHost;}
            set { appHost = value == null ? null : value.TrimEnd('/'); }
        }


        /// <summary>
        /// <para>The port of the website you are testing</para>
        /// <para>Default: 80</para>
        /// </summary>
        public static int Port { get; set; }

        /// <summary>
        /// <para>Whether to use the HTTPS protocol to connect to website you are testing</para>
        /// <para>Default: false</para>
        /// </summary>
        public static bool SSL { get; set; }
    }
}