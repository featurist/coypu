namespace Coypu.Queries
{
    internal class ElementQuery : DriverScopeQuery<Element>
    {
        public ElementQuery(DriverScope driverScope, Options options) : base(driverScope, options)
        {
        }

        public override Element ExpectedResult
        {
            get { return null; }
        }

        public override Element Run()
        {
            return Scope.FindElement();
        }
    }
}