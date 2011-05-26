using System;

namespace Coypu.Drivers
{
    public class DriverNotSupportedException : Exception
    {
        public DriverNotSupportedException(Type driver)
            : base(string.Format("{0} is not supported", driver.Name))
        {
        }
    }
}