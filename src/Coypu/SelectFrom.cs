using Coypu.Robustness;

namespace Coypu
{
    public class SelectFrom
    {
        private readonly string option;
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;

        public SelectFrom(string option, Driver driver, RobustWrapper robustWrapper)
        {
            this.option = option;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
        }

        /// <summary>
        /// Find the element from which to select this option
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        public void From(string locator)
        {
            robustWrapper.Robustly(() => driver.Select(driver.FindField(locator), option));
        }
    }
}