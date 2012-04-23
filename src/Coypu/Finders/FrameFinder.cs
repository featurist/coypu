namespace Coypu.Finders
{
    internal class FrameFinder : ElementFinder
    {
        internal FrameFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override ElementFound Find()
        {
            return Driver.FindFrame(Locator, Scope);
        }
    }
}