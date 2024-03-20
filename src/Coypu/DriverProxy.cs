using System.Collections.Generic;

namespace Coypu
{
    /// <summary>
    /// Proxy information for a Driver
    /// </summary>
    public class DriverProxy 
    {
        /// <summary>
        /// The Username for the proxy
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// The Password for the proxy
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// The Server of the proxy
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Use proxy for SSL
        /// </summary>
        public bool Ssl { get; set; } = true;
        
        /// <summary>
        /// Use type of proxy
        /// </summary>
        public DriverProxyType Type { get; set; } = DriverProxyType.Http;
        
        /// <summary>
        /// Domains to bypass
        /// </summary>
        public IEnumerable<string> BypassAddresses { get; set; }
    }
}