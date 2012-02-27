namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string locator;
        private readonly DriverScope scope;
        private readonly string value;
        private readonly Element element;

        internal FillIn(Driver driver, DriverScope scope, string locator, Element element, string value) : base(driver,scope.IndividualTimeout)
        {
            this.locator = locator;
            this.element = element;
            this.scope = scope;
            this.value = value;
        }

        internal Element Field
        {
            get { return element ?? Driver.FindField(locator, scope); }
        }

        private void BringIntoFocus()
        {
            Driver.Click(Field);
        }

        internal void Set()
        {
            Driver.Set(Field, value);
        }

        internal void Focus()
        {
            if (Field["type"] != "file")
                BringIntoFocus();
        }

        public override void Act()
        {
            Focus();
            Set();
        }
    }
}