using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu
{
    public interface DriverFactory
    {
        /// <summary>
        /// Creates a new web driver using the specified driver type, browser and browser options.
        /// </summary>
        /// <param name="driverType">The <see cref="Type"/> of the web driver.</param>
        /// <param name="browser">The type of browser to be driven by the web driver.</param>
        /// <returns>The newly initialized instance of a <see cref="Driver"/> implementation.</returns>
        Driver NewWebDriver(Type driverType, Drivers.Browser browser);

        /// <summary>
        /// Creates a new web driver using the specified driver type, browser and browser options.
        /// </summary>
        /// <param name="driverType">The <see cref="Type"/> of the web driver.</param>
        /// <param name="browser">The type of browser to be driven by the web driver.</param>
        /// <param name="browserOptions">A dictionary of native browser-specific options for the various types of browsers.</param>
        /// <returns>The newly initialized instance of a <see cref="Driver"/> implementation.</returns>
        Driver NewWebDriver(Type driverType, Browser browser, IDictionary<Browser, object> browserOptions);
    }
}