using Coypu.Actions;
using Coypu.Timing;

namespace Coypu
{
    public class SelectFrom
    {
        private readonly string option;
        private readonly Driver driver;
        private readonly TimingStrategy timingStrategy;
        private readonly DriverScope scope;
        private readonly Options options;

        internal SelectFrom(string option, Driver driver, TimingStrategy timingStrategy, DriverScope scope, Options options)
        {
            this.option = option;
            this.driver = driver;
            this.timingStrategy = timingStrategy;
            this.scope = scope;
            this.options = options;
        }

        /// <summary>
        /// Find the first matching select to appear within the SessionConfiguration.Timeout from which to select this option.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void From(string locator)
        {
            timingStrategy.Synchronise(new Select(driver, scope, locator, option, options));
        }

    }
}