namespace Coypu.Queries
{
    internal class ElementQuery : DriverScopeQuery<ElementFound>
    {
        public ElementQuery(DriverScope driverScope) : base(driverScope)
        {
        }

        public override object ExpectedResult
        {
            get { return null; }
        }

        public override void Run()
        {
            Result = DriverScope.Now();
        }
    }
}