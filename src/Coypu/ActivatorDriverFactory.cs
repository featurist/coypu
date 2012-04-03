using System;
using System.Reflection;

namespace Coypu
{
    public class ActivatorDriverFactory : DriverFactory
    {
        public static int OpenDrivers { get; set; }

        public Driver NewWebDriver(Type driverType, Drivers.Browser browser)
        {
            try
            {
                var driver = (Driver)Activator.CreateInstance(driverType, browser);
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