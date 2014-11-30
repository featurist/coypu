namespace Coypu.Queries
{
    internal class HasNoDialogQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        protected internal HasNoDialogQuery(Driver driver, string text, DriverScope driverScope, Options options) : base(driverScope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override bool Run()
        {
            return !driver.HasDialog(text,Scope);
        }
    }
}