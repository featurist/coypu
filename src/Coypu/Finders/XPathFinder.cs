namespace Coypu.Finders
{
    internal class XPathFinder : ElementFinder
    {
        internal XPathFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindXPath(Locator, Scope);
        }
    }
}