using System;

namespace Coypu.Drivers
{
    public class BrowserNotSupportedException : Exception
    {
        public BrowserNotSupportedException(Browser browser, Type driverType)
            : base(string.Format("{0} is not supported by {1}", browser, driverType.Name))
        {
        }
    }
}