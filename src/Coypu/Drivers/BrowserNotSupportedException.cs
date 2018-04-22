using System;
#pragma warning disable 1591

namespace Coypu.Drivers
{
    /// <summary>
    ///     Thrown when your chosen browser is not supported by your chosen driver
    /// </summary>
    public class BrowserNotSupportedException : Exception
    {
        public BrowserNotSupportedException(Browser browser,
                                            Type driverType)
            : this(browser, driverType, null) { }

        public BrowserNotSupportedException(Browser browser,
                                            Type driverType,
                                            Exception inner)
            : base($"{browser} is not supported by {driverType.Name}", inner) { }
    }
}