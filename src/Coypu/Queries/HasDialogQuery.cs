namespace Coypu.Queries
{
    internal class HasDialogQuery : DriverScopeQuery<bool>
    {
        private readonly IDriver driver;
        private readonly string text;
        public override object ExpectedResult => true;

        protected internal HasDialogQuery(IDriver driver, string text, DriverScope driverScope, Options options) : base(driverScope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override bool Run()
        {
            return driver.HasDialog(text,Scope);
        }
    }
}