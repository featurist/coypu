using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Finders;

namespace Coypu
{
    /// <summary>
    /// Options for how Coypu interacts with the browser.
    /// </summary>
    public class Options
    {
        private const bool DEFAULT_CONSIDER_INVISIBLE_ELEMENTS = false;
        private const bool DEFAULT_EXACT = false;
        private const Match DEFAULT_MATCH = Match.Single;
        private static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan DEFAULT_RETRY_INTERVAL = TimeSpan.FromSeconds(0.05);
        private static readonly TimeSpan DEFAULT_WAIT_BEFORE_CLICK = TimeSpan.Zero;

        private bool? considerInvisibleElements;
        private bool? exact;
        private Match? match;
        private TimeSpan? retryInterval;
        private TimeSpan? timeout;
        private TimeSpan? waitBeforeClick;

        /// <summary>
        /// <para>When retrying, how long to wait for elements to appear or actions to complete without error.</para>
        /// <para>Default: 1sec</para>
        /// </summary>
        public TimeSpan Timeout
        {
            get { return timeout ?? DEFAULT_TIMEOUT; }
            set { timeout = value; }
        }

        /// <summary>
        /// <para>How long to wait between retries</para>
        /// <para>Default: 100ms</para>
        /// </summary>
        public TimeSpan RetryInterval
        {
            get { return retryInterval ?? DEFAULT_RETRY_INTERVAL; }
            set { retryInterval = value; }
        }

        /// <summary>
        /// <para>How long to wait between finding an element and clicking it.</para>
        /// <para>Default: zero</para>
        /// </summary>
        public TimeSpan WaitBeforeClick
        {
            get { return waitBeforeClick ?? DEFAULT_WAIT_BEFORE_CLICK; }
            set { waitBeforeClick = value; }
        }

        /// <summary>
        /// <para>By default Coypu will exclude any invisible elements, this allows you to override that behaviour</para>
        /// <para>Default: true</para>
        /// </summary>
        public bool ConsiderInvisibleElements
        {
            get { return considerInvisibleElements ?? DEFAULT_CONSIDER_INVISIBLE_ELEMENTS; }
            set { considerInvisibleElements = value; }
        }

        /// <summary>
        /// <para>Whether to consider a partial match when finding elements by text, or just an exact match.</para>
        /// <para>The following elements currently support partial matching:</para>
        /// <para>FillIn (label text)</para>
        /// <para>FindField (label text)</para>
        /// <para></para>
        /// <para>ClickLink (link text)</para>
        /// <para>FindLink (link text)</para>
        /// <para></para>
        /// <para>ClickButton (link text)</para>
        /// <para>FindButton (link text)</para>
        /// </summary>
        public bool Exact
        {
            get { return exact ?? DEFAULT_EXACT; }
            set { exact = value; }
        }

        /// <summary>
        /// <para>With Match you can control how Coypu behaves when multiple elements all match a query. There are currently two different strategies:</para>
        /// <para>Match.First: The default strategy. If multiple matches are found, some of which are exact, and some of which are not, then the first exactly matching element is returned.</para>
        /// <para>Match.Single: If the Exact option is true, raises an error if more than one element matches, just like one. If Exact is false, it will first try to find an exact match. An error is raised if more than one element is found. If no element is found, a new search is performed which allows partial matches. If that search returns multiple matches, an error is raised.</para>
        /// </summary>
        public Match Match
        {
            get { return match ?? DEFAULT_MATCH; }
            set { match = value; }
        }

        public T FilterWithMatchStrategy<T>(IEnumerable<T> elements, string queryDescription)
        {
            var element = elements.FirstOrDefault();
            if (Match == Match.First && element != null)
                return element;

            var count = elements.Count();

            if (Match == Match.Single && count > 1)
            {
                throw new AmbiguousHtmlException(BuildAmbiguousMessage(queryDescription, count));
            }

            if (count == 0)
                throw new MissingHtmlException("Unable to find " + queryDescription);

            return element;
        }

        internal ElementFound Find(QueryFinder query)
        {
            var results = query.FindAll(exact: true);
            if (Match == Match.First && results.Any())
                return results.First();

            var count = results.Count();

            if (Match == Match.Single && count == 0)
                results = query.FindAll(exact: false);
            
            count = results.Count();
            if (count > 1)
                throw new AmbiguousHtmlException(BuildAmbiguousMessage(query.QueryDescription, count));

            if (count == 0)
                throw new MissingHtmlException("Unable to find " + query.QueryDescription);

            return results.First();
        }

        private string BuildAmbiguousMessage(string queryDescription, int count)
        {
            var message = string.Format(@"Ambiguous match, found {0} elements matching {1}

Coypu does this by default from v1.0

Your options:
 * Look for something more specific",
                    count, queryDescription);

            if (Match == Match.Single)
                message += Environment.NewLine + " * Set the Options.Match option to Match.First (Coypu 0.* behaviour) to get the first matching element (prefers exact matches to partial on text comparisons)";

            if (!Exact)
                message += Environment.NewLine + " * Set the Options.Exact option to True to exclude partial text matches";
            
            return message;
        }

        /// <summary>
        /// Merge with another instance of Options to produce a new instance of Options. If any options on this instance are unset they will be taken from the supplied Options.
        /// </summary>
        /// <param name="with">Any options unset will be copied from this</param>
        /// <returns>The new merged Options</returns>
        public Options Merge(Options with)
        {
            return new Options
                {
                    considerInvisibleElements = Default(considerInvisibleElements, with.considerInvisibleElements),
                    exact = Default(exact, with.exact),
                    match = Default(match, with.match),
                    retryInterval = Default(retryInterval, with.retryInterval),
                    timeout = Default(timeout, with.timeout),
                    waitBeforeClick = Default(waitBeforeClick, with.waitBeforeClick)
                };
        }

        private static T? Default<T>(T? value, T? defaultValue) where T : struct
        {
            return value.HasValue 
                       ? value
                       : defaultValue;
        }
    }
}