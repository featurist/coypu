namespace Coypu.Finders
{
    internal class SectionFinder : ElementFinder
    {
        internal SectionFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override ElementFound Find()
        {
            return Driver.FindSection(Locator, Scope);
        }
    }
}