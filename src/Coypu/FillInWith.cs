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
            With(value,false);
        }
        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <param name="forceAllEvents">By default Coypu will use Javascript to set fields for speed where possible. If you need all the usual key up/down etc events fired then this setting will use the driver's native method of typing in an input.</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value, bool forceAllEvents)
        {
            robustWrapper.Robustly(new FillIn(driver, scope, locator, value, options));
        }
    }
}