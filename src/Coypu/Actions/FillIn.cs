namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string locator;
        private readonly DriverScope scope;
        private readonly string value;
        private ElementScope element;

        internal FillIn(Driver driver, ElementScope element, string value, Options options)
            : this(driver,null,null,value,options) {
            this.element = element;
        }

        internal FillIn(Driver driver, DriverScope scope, string locator, string value, Options options)
            : base(driver,options)
        {
            this.locator = locator;
            this.scope = scope;
            this.value = value;
        }

        internal Element Field
        {
            get
            {
                if (element == null)
                    return Driver.FindField(locator, scope);
                
                return element.Find();
            }
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