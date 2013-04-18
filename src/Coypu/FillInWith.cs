using Coypu.Actions;
using Coypu.Robustness;

namespace Coypu
{
    public class FillInWith
    {
        private readonly string locator;
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly DriverScope scope;
        private readonly Options options;
        private readonly ElementFound element;

        internal FillInWith(string locator, Driver driver, RobustWrapper robustWrapper, DriverScope scope, Options options)
        {
            this.locator = locator;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
            this.options = options;
        }

        internal FillInWith(ElementFound element, Driver driver, RobustWrapper robustWrapper, DriverScope scope, Options options)
        {
            this.element = element;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
            this.options = options;
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            robustWrapper.Robustly(new FillIn(driver, scope, locator, value, options));
        }
    }
}