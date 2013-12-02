namespace Coypu.Actions
{
    internal class ClickAction : DriverAction
    {
        private readonly ElementScope elementScope;

        internal ClickAction(ElementScope elementScope, Driver driver, Options options)
            : base(driver, options)
        {
            this.elementScope = elementScope;
        }

        public override void Act()
        {
            var element = elementScope.Find();
            Driver.Click(element);
        }
    }
}