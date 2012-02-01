namespace Coypu.Queries
{
    internal class ElementExistsQuery : ElementQuery
    {
        public ElementExistsQuery(DriverScope driverScope) : base(driverScope)
        {
        }

        public override bool ExpectingResult
        {
            get { return true; }
        }
    }
}