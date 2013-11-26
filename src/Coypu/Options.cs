using System;

namespace Coypu
{
    /// <summary>
    /// Options for how Coypu interacts with the browser.
    /// </summary>
    public class Options
    {
        const double DEFAULT_TIMEOUT_SECONDS = 1;
        const double DEFAULT_INTERVAL_SECONDS = 0.01;

        /// <summary>
        /// New default options
        /// </summary>
        public Options()
        {
            Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT_SECONDS);
            RetryInterval = TimeSpan.FromSeconds(DEFAULT_INTERVAL_SECONDS);
            WaitBeforeClick = TimeSpan.Zero;
        }

        private TimeSpan _timeout;

        /// <summary>
        /// <para>When retrying, how long to wait for elements to appear or actions to complete without error.</para>
        /// <para>Default: 1sec</para>
        /// </summary>
        public TimeSpan Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        private TimeSpan _retryInterval;

        /// <summary>
        /// <para>How long to wait between retries</para>
        /// <para>Default: 100ms</para>
        /// </summary>
        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
            set { _retryInterval = value; }
        }

        /// <summary>
        /// <para>How long to wait between finding an element and clicking it.</para>
        /// <para>Default: zero</para>
        /// </summary>
        public TimeSpan WaitBeforeClick { get; set; }

        /// <summary>
        /// <para>By default Coypu will exclude any invisible elements, this allows you to override that behaviour</para>
        /// <para>Default: true</para>
        /// </summary>
        public bool ConsiderInvisibleElements { get; set; }

        /// <summary>
        /// Whether to consider a partial match of the text of labels when finding a field, or just an exact match.
        /// </summary>
        public FieldFinderPrecision FieldFinderPrecision { get; set; }
    }
}