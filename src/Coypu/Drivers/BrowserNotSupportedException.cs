using System;

namespace Coypu.Drivers
{
    /// <summary>
    /// Thrown when your chosen browser is not supported by your chosen driver
    /// </summary>
    public class BrowserNotSupportedException : Exception
    {
        public BrowserNotSupportedException(Browser browser, Type driverType)
            : this( browser,driverType, null)
        {
        }

        public BrowserNotSupportedException(Browser browser, Type driverType, Exception inner)
            : base(string.Format("{0} is not supported by {1}", browser, driverType.Name), inner)
        {
        }
    }
}