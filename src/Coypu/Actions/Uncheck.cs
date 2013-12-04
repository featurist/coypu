namespace Coypu.Actions
{
    internal class Uncheck : DriverAction
    {
        private readonly ElementScope element;

        internal Uncheck(Driver driver, ElementScope element, Options options) 
            : base(driver, options)
        {
            this.element = element;
        }

        public override void Act()
        {
            Driver.Uncheck(element);
        }

    }
}