namespace Coypu.Queries
{
    internal class ElementExistsQuery : ElementPresenceQuery
    {
        public ElementExistsQuery(DriverScope driverScope)
            : base(driverScope, true)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}