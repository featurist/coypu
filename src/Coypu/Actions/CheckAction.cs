namespace Coypu.Actions
{
    internal class CheckAction : DriverAction
    {
        private readonly ElementScope element;

        internal CheckAction(Driver driver, ElementScope element, Options options)
            : base(driver, element, options)
        {
            this.element = element;
        }

        public override void Act()
        {
            Driver.Check(element);
        }
    }
}