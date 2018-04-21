namespace Coypu.Actions
{
    internal class SendKeys : DriverAction
    {
        private readonly string keys;
        private readonly DriverScope driverScope;

        internal SendKeys(string keys, DriverScope driverScope, IDriver driver, Options options)
            : base(driver, driverScope, options)
        {
            this.keys = keys;
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            Driver.SendKeys(element,keys);
        }
    }
}