using System;
using System.Reflection;

namespace Coypu
{
    public class ActivatorDriverFactory : DriverFactory
    {
        public static int OpenDrivers { get; set; }

        public IDriver NewWebDriver(Type driverType, Drivers.Browser browser, bool headless)
        {
            try
            {
                var driver = (IDriver)Activator.CreateInstance(driverType, browser, headless);
                OpenDrivers++;
                return driver;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}
