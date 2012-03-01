using System;
using Coypu.Drivers.Selenium;

namespace Coypu
{
    /// <summary>
    /// Global configuration settings
    /// </summary>
    public class Configuration
    {
        const string DEFAULT_APP_HOST = "localhost";
        const int DEFAULT_PORT = 80;
        const double DEFAULT_TIMEOUT_SECONDS = 1;
        const double DEFAULT_INTERVAL_SECONDS = 0.1;
        
        private static string appHost;
 
        internal Configuration() {}

        /// <summary>
        /// Reset all configuration settings to their default values
        /// </summary>
        public static Configuration Default()
        {
            return new Configuration
                             {
                                 AppHost = DEFAULT_APP_HOST,
                                 Port = DEFAULT_PORT,
                                 SSL = false,
                                 Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS),
                                 RetryInterval = TimeSpan.FromSeconds(DEFAULT_INTERVAL_SECONDS),
                                 Browser = Drivers.Browser.Firefox,
                                 Driver = typeof (SeleniumWebDriver),
                                 WaitBeforeClick = TimeSpan.Zero
                             };
        }

        /// <summary>
        /// <para>When retrying, how long to wait for elements to appear or actions to complete without error.</para>
        /// <para>Default: 10sec</para>
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// <para>How long to wait between retries</para>
        /// <para>Default: 100ms</para>
        /// </summary>
        public TimeSpan RetryInterval { get; set; }

        /// <summary>
        /// <para>Specifies the browser you would like to control</para>
        /// <para>Default: Firefox</para>
        /// </summary>
        public Drivers.Browser Browser { get; set; }

        /// <summary>
        /// <para>Specifies the driver you would like to use to control the browser</para> 
        /// <para>Default: SeleniumWebDriver</para>
        /// </summary>
        public Type Driver { get; set; }

        /// <summary>
        /// <para>How long to wait between finding an element and clicking it.</para>
        /// <para>Default: zero</para>
        /// </summary>
        public TimeSpan WaitBeforeClick { get; set; }


        /// <summary>
        /// <para>The host of the website you are testing, e.g. 'github.com'</para>
        /// <para>Default: localhost</para>
        /// </summary>
        public string AppHost
        {
            get { return appHost;}
            set { appHost = value == null ? null : value.TrimEnd('/'); }
        }


        /// <summary>
        /// <para>The port of the website you are testing</para>
        /// <para>Default: 80</para>
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// <para>Whether to use the HTTPS protocol to connect to website you are testing</para>
        /// <para>Default: false</para>
        /// </summary>
        public bool SSL { get; set; }
    }
}