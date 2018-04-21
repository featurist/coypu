using Coypu.Drivers;

namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Hover(DriverScope driverScope, IDriver driver, Options options)
            : base(driver, driverScope, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Now();

            // In Firefox, scrolling to an element in firefox is broken.
            // Bug: https://github.com/mozilla/geckodriver/issues/901
            if (driverScope.Browser == Browser.Firefox)
            {
                Driver.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);", driverScope);
            }
            Driver.Hover(element);
        }
    }
}