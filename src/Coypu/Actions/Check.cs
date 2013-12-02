namespace Coypu.Actions
{
    internal class Check : DriverAction
    {
        private readonly string locator;
        private readonly Scope scope;
        private readonly ElementScope elementScope;

        internal Check(Driver driver, string locator, Scope scope, Options options)
            : base(driver, options) {
            this.locator = locator;
            this.scope = scope;
        }

        internal Check(Driver driver, ElementScope elementScope, Options options)
            : base(driver, options)
        {
            this.elementScope = elementScope;
        }

        protected Element Element {
            get {
                if (elementScope == null)
                    return Driver.FindField(locator, scope);

                return elementScope.Find();
            }
        }

        public override void Act()
        {
            Driver.Check(Element);
        }
    }
}