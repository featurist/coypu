using System;
using System.Linq;
using System.Reflection;

namespace Coypu
{
    /// <summary>
    /// Options for how Coypu interacts with the browser.
    /// </summary>
    public class Options
    {
        private const bool DEFAULT_CONSIDER_INVISIBLE_ELEMENTS = false;
        private const TextPrecision DEFAULT_PRECISION = TextPrecision.PreferExact;
        private const Match DEFAULT_MATCH = Match.Single;
        private static readonly TimeSpan DEFAULT_TIMEOUT = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan DEFAULT_RETRY_INTERVAL = TimeSpan.FromSeconds(0.05);
        private static readonly TimeSpan DEFAULT_WAIT_BEFORE_CLICK = TimeSpan.Zero;

        protected bool? considerInvisibleElements;
        private TextPrecision? textPrecision;
        private Match? match;
        private TimeSpan? retryInterval;
        private TimeSpan? timeout;
        private TimeSpan? waitBeforeClick;


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            //if (obj.GetType() != this.GetType()) return false;
            return Equals((Options) obj);
        }

        /// <summary>
        /// Will not wait for asynchronous updates to the page
        /// </summary>
        public static Options NoWait = new Options
        {
            Timeout = TimeSpan.Zero
        };

        /// <summary>
        /// Include invisible elements when finding
        /// </summary>
        public static Options Invisible = new Options
        {
            ConsiderInvisibleElements = true
        };

        /// <summary>
        /// Just picks the first element that matches
        /// </summary>
        public static Options First
        {
            get { return new Options {Match = Match.First}; }
        }

        /// <summary>
        /// Raises an error if more than one element match
        /// </summary>
        public static Options Single
        {
            get { return new Options { Match = Match.Single }; }
        }


        /// <summary>
        /// Match by exact visible text
        /// </summary>
        public static Options Exact
        {
            get { return new Options { TextPrecision = TextPrecision.Exact }; }
        }

        /// <summary>
        /// Match by substring in visible text
        /// </summary>
        public static Options Substring
        {
            get { return new Options { TextPrecision = TextPrecision.Substring }; }
        }

        /// <summary>
        /// If multiple matches are found, some of which are exact, and some of which are not, then the first exactly matching element is returned
        /// </summary>
        public static Options PreferExact
        {
            get { return new Options { TextPrecision = TextPrecision.PreferExact }; }
        }

        /// <summary>
        /// Match exact visible text; Just picks the first element that matches
        /// </summary>
        public static Options FirstExact = Merge(First, Exact);

        /// <summary>
        /// Match substring in visible text; Just picks the first element that matches
        /// </summary>
        public static Options FirstSubstring = Merge(First, Substring);

        /// <summary>
        /// Prefer exact text matches to substring matches; Just picks the first element that matches
        /// </summary>
        public static Options FirstPreferExact = Merge(First, PreferExact);

        /// <summary>
        /// Match exact visible text; Raises an error if more than one element match
        /// </summary>
        public static Options SingleExact = Merge(Single, Substring);

        /// <summary>
        /// Match by substring in visible text; Raises an error if more than one element match
        /// </summary>
        public static Options SingleSubstring = Merge(Single, Substring);

        /// <summary>
        /// Prefer exact text matches to substring matches; Raises an error if more than one element match
        /// </summary>
        public static Options SinglePreferExact = Merge(Single, PreferExact);

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
        /// <para>Whether to consider substrings when finding elements by text, or just an exact match.</para>
        /// </summary>
        public TextPrecision TextPrecision
        {
            get { return textPrecision ?? DEFAULT_PRECISION; }
            set { textPrecision = value; }
        }

        internal bool TextPrecisionExact
        {
            get { return textPrecision == TextPrecision.Exact; }
        }

        /// <summary>
        /// <para>With PreventAmbiguousMatches you can control whether Coypu should throw an exception when multiple elements match a query.</para>
        /// </summary>
        public Match Match
        {
            get { return match ?? DEFAULT_MATCH; }
            set { match = value; }
        }

        internal string BuildAmbiguousMessage(string queryDescription, int count)
        {
            var message = string.Format(@"Ambiguous match, found {0} elements matching {1}

Coypu does this by default from v2.0. Your options:

 * Look for something more specific",
                    count, queryDescription);


            if (TextPrecision != TextPrecision.Exact)
                message += Environment.NewLine + " * Set the Options.TextPrecision option to Exact to exclude substring text matches";

            if (Match != Match.First)
                message += Environment.NewLine + " * Set the Options.Match option to Match.First to just take the first matching element";

            return message;
        }

        /// <summary>
        /// Merge any unset Options from another set of Options.
        /// </summary>
        /// <param name="preferredOptions">The preferred set of options</param>
        /// <param name="defaultOptions">Any unset preferred options will be copied from this</param>
        /// <returns>The new merged Options</returns>
        public static Options Merge(Options preferredOptions, Options defaultOptions)
        {
            preferredOptions = preferredOptions ?? new Options();
            defaultOptions = defaultOptions ?? new Options();

            return new Options
                {
                    considerInvisibleElements = Default(preferredOptions.considerInvisibleElements, defaultOptions.considerInvisibleElements),
                    textPrecision = Default(preferredOptions.textPrecision, defaultOptions.textPrecision),
                    match = Default(preferredOptions.match, defaultOptions.match),
                    retryInterval = Default(preferredOptions.retryInterval, defaultOptions.retryInterval),
                    timeout = Default(preferredOptions.timeout, defaultOptions.timeout),
                    waitBeforeClick = Default(preferredOptions.waitBeforeClick, defaultOptions.waitBeforeClick)
                };
        }

     

        protected static T? Default<T>(T? value, T? defaultValue) where T : struct
        {
            return value.HasValue 
                       ? value
                       : defaultValue;
        }

        public override string ToString()
        {
			return string.Join(Environment.NewLine, GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Select(p => p.Name + ": " + p.GetValue(this, null)).ToArray());
        }

        protected bool Equals(Options other)
        {
            return considerInvisibleElements.Equals(other.considerInvisibleElements) && textPrecision.Equals(other.textPrecision) && match == other.match && retryInterval.Equals(other.retryInterval) && timeout.Equals(other.timeout) && waitBeforeClick.Equals(other.waitBeforeClick);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = considerInvisibleElements.GetHashCode();
                hashCode = (hashCode * 397) ^ textPrecision.GetHashCode();
                hashCode = (hashCode * 397) ^ match.GetHashCode();
                hashCode = (hashCode * 397) ^ retryInterval.GetHashCode();
                hashCode = (hashCode * 397) ^ timeout.GetHashCode();
                hashCode = (hashCode * 397) ^ waitBeforeClick.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Options left, Options right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Options left, Options right)
        {
            return !Equals(left, right);
        }
    }
}