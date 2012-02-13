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
        private readonly Element element;

        internal FillInWith(string locator, Driver driver, RobustWrapper robustWrapper, DriverScope scope)
        {
            this.locator = locator;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
        }

        internal FillInWith(Element element, Driver driver, RobustWrapper robustWrapper, DriverScope scope)
        {
            this.element = element;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            robustWrapper.RobustlyDo(new FillIn(driver,scope,locator,element, value));
        }
    }
}