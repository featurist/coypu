namespace Coypu.Queries
{
    internal class WindowExistsQuery : DriverScopeQuery<bool>
    {
        protected internal WindowExistsQuery(DriverScope driverScope, Options options)
            : base(driverScope,options)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }

        public override bool Run()
        {
            try
            {
                Scope.Stale = true;
                Scope.FindElement();
                return true;
            }
            catch (MissingWindowException)
            {
                return false;
            }
        }
    }
}