namespace Coypu.Actions {
    internal class CheckElement : DriverAction
    {
        private readonly ElementScope element;

        internal CheckElement(Driver driver, ElementScope element, Options options)
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