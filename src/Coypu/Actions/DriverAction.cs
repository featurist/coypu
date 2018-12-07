namespace Coypu.Actions
{
    public abstract class DriverAction : BrowserAction
    {
        protected readonly IDriver Driver;

        protected DriverAction(IDriver driver, DriverScope scope, Options options)
            : base(scope, options)
        {
            Driver = driver;
        }
    }
}