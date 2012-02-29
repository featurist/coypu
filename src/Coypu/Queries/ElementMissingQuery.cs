namespace Coypu.Queries
{
    internal class ElementMissingQuery : ElementPresenceQuery
    {
        public ElementMissingQuery(DriverScope driverScope)
            : base(driverScope, false)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}