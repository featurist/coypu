namespace Coypu.Actions
{
    internal class Uncheck : DriverAction
    {
        private readonly ElementScope element;

        internal Uncheck(IDriver driver, ElementScope element, Options options)
            : base(driver, element, options)
        {
            this.element = element;
        }

        public override void Act()
        {
            Driver.Uncheck(element);
        }

    }
}