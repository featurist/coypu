using Coypu.Actions;
using Coypu.Robustness;

namespace Coypu
{
    public class FillInWith
    {
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly Options options;
        private readonly ElementScope element;


        internal FillInWith(ElementScope element, Driver driver, RobustWrapper robustWrapper, Options options)
        {
            this.element = element;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.options = options;
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            robustWrapper.Robustly(new FillIn(driver, element, value, options));
        }
    }
}