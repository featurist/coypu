namespace Coypu.Finders
{
    internal class IdFinder : ElementFinder
    {
        internal IdFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override ElementFound Find()
        {
            return Driver.FindId(Locator, Scope);
        }
    }
}