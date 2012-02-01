namespace Coypu.Queries
{
    internal class ElementMissingQuery : ElementQuery
    {
        public ElementMissingQuery(DriverScope driverScope) : base(driverScope)
        {
        }

        public override bool ExpectingResult
        {
            get { return false; }
        }
    }
}