using System;

namespace Coypu
{
    public interface DriverFactory
    {
        Driver NewWebDriver(Type driverType, Drivers.Browser browser);
    }
}