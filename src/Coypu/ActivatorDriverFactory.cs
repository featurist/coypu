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
                Console.Write("New  driver...");

                var driver = (Driver)Activator.CreateInstance(driverType, browser);

                Console.WriteLine("open.");
                OpenDrivers++;
                Console.WriteLine(OpenDrivers + " drivers open.");

                return driver;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }
    }
}