namespace Coypu.Actions
{
    internal class Check : DriverAction
    {
        private readonly ElementScope element;

        internal Check(Driver driver, ElementScope element, Options options)
            : base(driver, options)
        {
            this.element = element;
        }

        public override void Act()
        {
            Driver.Check(element);
        }
    }
}