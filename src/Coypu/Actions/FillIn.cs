using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string locator;
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string value;
        private readonly Element element;

        internal FillIn(Driver driver, DriverScope scope, string locator, Element element, string value)
        {
            this.locator = locator;
            this.element = element;
            this.driver = driver;
            this.scope = scope;
            this.value = value;
        }

        internal Element Field
        {
            get { return element ?? driver.FindField(locator, scope); }
        }

        private void BringIntoFocus()
        {
            driver.Click(Field);
        }

        internal void Set()
        {
            driver.Set(Field, value);
        }

        internal void Focus()
        {
            if (Field["type"] != "file")
                BringIntoFocus();
        }

        public void Act()
        {
            Focus();
            Set();
        }
    }
}