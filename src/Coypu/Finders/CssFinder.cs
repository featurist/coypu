namespace Coypu.Finders
{
    internal class CssFinder : ElementFinder
    {
        internal CssFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override ElementFound Find()
        {
            return Driver.FindCss(Locator, Scope);
        }
    }
}