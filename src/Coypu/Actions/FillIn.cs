namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string locator;
        private readonly DriverScope scope;
        private readonly string value;
        private Element element;
        private readonly bool forceAllEvents;

        internal FillIn(Driver driver, DriverScope scope, string locator, ElementScope element, string value, bool forceAllEvents, Options options)
            : this(driver,scope,locator,element.Now(),value,forceAllEvents,options)
        {
        }

        internal FillIn(Driver driver, DriverScope scope, string locator, ElementFound element, string value, bool forceAllEvents, Options options)
            : base(driver,options)
        {
            this.locator = locator;
            this.element = element;
            this.scope = scope;
            this.value = value;
            this.forceAllEvents = forceAllEvents;
        }

        internal Element Field
        {
            get
            {
                if (element == null)
                    element = Driver.FindField(locator, scope);
                return element;
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