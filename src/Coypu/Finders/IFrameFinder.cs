namespace Coypu.Finders
{
    internal class IFrameFinder : ElementFinder
    {
        internal IFrameFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindIFrame(Locator, Scope);
        }
    }
}