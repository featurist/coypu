namespace Coypu.Queries
{
    internal class ElementQuery : DriverScopeQuery<Element>
    {
        public ElementQuery(DriverScope driverScope, Options options) : base(driverScope, options)
        {
        }

        public override object ExpectedResult => null;

        public override Element Run()
        {
            return Scope.FindElement();
        }
    }
}