namespace Coypu.Queries
{
    internal class WindowMissingQuery : DriverScopeQuery<bool>
    {
        protected internal WindowMissingQuery(DriverScope driverScope, Options options)
            : base(driverScope, options)
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
                return false;
            }
            catch (MissingWindowException)
            {
                return true;
            }
        }
    }
}