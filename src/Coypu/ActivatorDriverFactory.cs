using System;
using System.Reflection;

namespace Coypu
{
    public class ActivatorDriverFactory : DriverFactory
    {
        public static int OpenDrivers { get; set; }

        public IDriver NewWebDriver(SessionConfiguration sessionConfiguration)
        {
            try
            {
                var driver = (IDriver)Activator.CreateInstance(sessionConfiguration.Driver, sessionConfiguration);
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
