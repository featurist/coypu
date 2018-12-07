using Coypu.Actions;
using Coypu.Timing;

namespace Coypu
{
    public class FillInWith
    {
        private readonly IDriver driver;
        private readonly TimingStrategy timingStrategy;
        private readonly Options options;
        private readonly ElementScope element;


        internal FillInWith(ElementScope element, IDriver driver, TimingStrategy timingStrategy, Options options)
        {
            this.element = element;
            this.driver = driver;
            this.timingStrategy = timingStrategy;
            this.options = options;
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            timingStrategy.Synchronise(new FillIn(driver, element, value, options));
        }
    }
}