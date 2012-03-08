namespace Coypu.Actions
{
    public abstract class DriverAction : BrowserAction
    {
        protected readonly Driver Driver;

        protected DriverAction(Driver driver, Options options) : base(options)
        {
            Driver = driver;
        }
    }
}