using System;
using System.Collections.Generic;
using System.Reflection;
using Coypu.Drivers;

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

        public Driver NewWebDriver(Type driverType, Drivers.Browser browser, IDictionary<Browser, object> browserOptions)
        {
            // Look for constructor on target driver type that accepts browserOptions argument
            var constructor = driverType.GetConstructor(new Type[] { typeof(Type), typeof(Browser), typeof(IDictionary<Browser, object>) });

            // If no matching constructor exists, use other factory method
            if (constructor == null)
                return NewWebDriver(driverType, browser);

            try
            {
                var driver = (Driver)Activator.CreateInstance(driverType, browser, browserOptions);
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