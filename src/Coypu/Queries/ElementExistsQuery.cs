namespace Coypu.Queries
{
    internal class ElementExistsQuery : ElementPresenceQuery
    {
        public ElementExistsQuery(DriverScope driverScope)
            : base(driverScope)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}