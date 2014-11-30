namespace Coypu.Actions
{
    internal class ClickAction : DriverAction
    {
        private readonly ElementScope elementScope;

        internal ClickAction(ElementScope elementScope, Driver driver, Options options)
            : base(driver, elementScope, options)
        {
            this.elementScope = elementScope;
        }

        public override void Act()
        {
            Driver.Click(elementScope);
        }
    }
}