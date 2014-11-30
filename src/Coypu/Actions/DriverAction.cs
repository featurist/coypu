namespace Coypu.Actions
{
    public abstract class DriverAction : BrowserAction
    {
        protected readonly Driver Driver;

        protected DriverAction(Driver driver, DriverScope scope, Options options)
            : base(scope, options)
        {
            Driver = driver;
        }
    }
}