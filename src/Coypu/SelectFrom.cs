using Coypu.Actions;
using Coypu.Timing;

namespace Coypu
{
    public class SelectFrom
    {
        private readonly DisambiguationStrategy _disambiguationStrategy;
        private readonly IDriver _driver;
        private readonly string _option;
        private readonly Options _options;
        private readonly DriverScope _scope;
        private readonly TimingStrategy _timingStrategy;

        internal SelectFrom(string option,
                            IDriver driver,
                            TimingStrategy timingStrategy,
                            DriverScope scope,
                            Options options,
                            DisambiguationStrategy disambiguationStrategy)
        {
            _option = option;
            _driver = driver;
            _timingStrategy = timingStrategy;
            _scope = scope;
            _options = options;
            _disambiguationStrategy = disambiguationStrategy;
        }

        /// <summary>
        ///     Find the first matching select to appear within the SessionConfiguration.Timeout from which to select this option.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void From(string locator)
        {
            From(locator, null);
        }

        /// <summary>
        ///     Find the first matching select to appear within the SessionConfiguration.Timeout from which to select this option.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <param name="fromOptions">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait</para></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void From(string locator,
                         Options fromOptions)
        {
            _timingStrategy.Synchronise(new Select(_driver, _scope, locator, _option, _disambiguationStrategy, _options, fromOptions));
        }
    }
}