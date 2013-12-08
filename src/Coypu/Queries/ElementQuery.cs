namespace Coypu.Queries
{
    internal class ElementQuery : DriverScopeQuery<ElementFound>
    {
        public ElementQuery(DriverScope driverScope, Options options) : base(driverScope, options)
        {
        }

        public override ElementFound ExpectedResult
        {
            get { return null; }
        }

        public override ElementFound Run()
        {
            return DriverScope.FindElement();
        }
    }
}