using System;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected readonly Driver Driver;
        protected readonly string Locator;
        protected readonly DriverScope Scope;

        protected ElementFinder(Driver driver, string locator, DriverScope scope)
        {
            Driver = driver;
            Locator = locator;
            Scope = scope;
        }

        internal abstract Element Find();
        internal TimeSpan Timeout { get; set; }
    }
}