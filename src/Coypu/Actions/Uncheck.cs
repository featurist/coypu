namespace Coypu.Actions
{
    internal class Uncheck : DriverAction
    {
        private readonly ElementScope element;
        private readonly DriverScope scope;
        private readonly string locator;

        internal Uncheck(Driver driver, ElementScope element, Options options) 
            : base(driver, options)
        {
            this.element = element;
        }

        internal Uncheck(Driver driver, DriverScope scope, string locator, Options options)
            : base(driver, options)
        {
            this.scope = scope;
            this.locator = locator;
        }

        public override void Act()
        {
            Driver.Uncheck(Field);
        }

        private ElementFound Field {
            get {
                if (locator != null)
                    return Driver.FindField(locator, scope);

                return element.Find();
            }
        }
    }
}