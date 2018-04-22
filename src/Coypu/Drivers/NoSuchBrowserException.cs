using System;

namespace Coypu.Drivers
{
    /// <summary>
    /// There is no such browser
    /// </summary>
    public class NoSuchBrowserException : Exception
    {
        internal NoSuchBrowserException(string browserName) : base($"No such browser: {browserName}")
        {
        }
    }
}