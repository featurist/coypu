using System;

namespace Coypu
{
    public interface DriverFactory
    {
        IDriver NewWebDriver(SessionConfiguration sessionConfiguration);
    }
}
