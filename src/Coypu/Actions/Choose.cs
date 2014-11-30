namespace Coypu.Actions
{
    internal class Choose : DriverAction
    {
        private readonly ElementScope elementScope;

        internal Choose(Driver driver, ElementScope elementScope, Options options)
            : base(driver, elementScope, options)
        {
            this.elementScope = elementScope;
        }

        public override void Act()
        {
            Driver.Choose(elementScope);
        }
    }
}