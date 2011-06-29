using System;

namespace Coypu.Drivers
{
    /// <summary>
    /// Thrown when your chosen browser is not supported by your chosen driver
    /// </summary>
    public class BrowserNotSupportedException : Exception
    {
        public BrowserNotSupportedException(Browser browser, Type driverType)
            : base(string.Format("{0} is not supported by {1}", browser, driverType.Name))
        {
        }
    }
}