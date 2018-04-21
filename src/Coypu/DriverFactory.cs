using System;

namespace Coypu
{
    public interface DriverFactory
    {
        IDriver NewWebDriver(Type driverType, Drivers.Browser browser);
    }
}