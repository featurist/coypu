namespace Coypu.Queries
{
    internal class ElementQuery : DriverScopeQuery<ElementFound>
    {
        public ElementQuery(DriverScope driverScope, Options options) : base(driverScope,options)
        {
        }

        public override object ExpectedResult
        {
            get { return null; }
        }

        public override void Run()
        {
            Result = DriverScope.FindElement();
        }
    }
}