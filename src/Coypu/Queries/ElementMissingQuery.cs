namespace Coypu.Queries
{
    internal class ElementMissingQuery : ElementPresenceQuery
    {
        public ElementMissingQuery(DriverScope driverScope)
            : base(driverScope)
        {
        }

        public override bool ExpectedResult
        {
            get { return false; }
        }
    }
}