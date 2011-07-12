using Coypu.Robustness;

namespace Coypu
{
    public class FillInWith
    {
        private readonly string locator;
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly Element element;

        internal FillInWith(string locator, Driver driver, RobustWrapper robustWrapper)
        {
            this.locator = locator;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
        }

        internal FillInWith(Element element, Driver driver, RobustWrapper robustWrapper)
        {
            this.element = element;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            robustWrapper.Robustly(
                () =>
                {
                    if (Field["type"] != "file")
                        BringIntoFocus();

                    driver.Set(Field, value);
                });
        }

        private void BringIntoFocus() 
        {
            driver.Click(Field);
        }

        private Element Field
        {
            get { return element ?? driver.FindField(locator); }
        }
    }
}