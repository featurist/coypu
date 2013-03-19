namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string locator;
        private readonly DriverScope scope;
        private readonly string value;
        private ElementScope element;
        private readonly bool forceAllEvents;

        internal FillIn(Driver driver, ElementScope element, string value, bool forceAllEvents, Options options)
            : this(driver,null,null,value,forceAllEvents,options) {
            this.element = element;
        }

        internal FillIn(Driver driver, DriverScope scope, string locator, string value, bool forceAllEvents, Options options)
            : base(driver,options)
        {
            this.locator = locator;
            this.scope = scope;
            this.value = value;
            this.forceAllEvents = forceAllEvents;
        }

        internal Element Field
        {
            get
            {
                if (element == null)
                    return Driver.FindField(locator, scope);
                
                return element.Now();
            }
        }

        private void BringIntoFocus()
        {
            if (forceAllEvents)
                Driver.Click(Field);
        }

        internal void Set()
        {
            Driver.Set(Field, value, forceAllEvents);
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