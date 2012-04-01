namespace Coypu.Finders
{
    internal class DocumentElementFinder : ElementFinder
    {
        public DocumentElementFinder(Driver driver) : base(driver, "Window",null)
        {
        }

        internal override ElementFound Find()
        {
            return Driver.Window;
        }
    }
}