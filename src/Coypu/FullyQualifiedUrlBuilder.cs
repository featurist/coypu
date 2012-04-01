using System;

namespace Coypu
{
    internal class FullyQualifiedUrlBuilder : UrlBuilder
    {
        public string GetFullyQualifiedUrl(string virtualPath, Configuration configuration)
        {
            if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
                return virtualPath;

            virtualPath = virtualPath.TrimStart('/');
            var scheme = configuration.SSL ? "https" : "http";

            return configuration.Port == 80
                       ? String.Format("{0}://{1}/{2}", scheme, configuration.AppHost, virtualPath)
                       : String.Format("{0}://{1}:{2}/{3}", scheme, configuration.AppHost, configuration.Port, virtualPath);
        }
    }
}