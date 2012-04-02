using System;

namespace Coypu
{
    internal class FullyQualifiedUrlBuilder : UrlBuilder
    {
        public string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration SessionConfiguration)
        {
            if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
                return virtualPath;

            virtualPath = virtualPath.TrimStart('/');
            var scheme = SessionConfiguration.SSL ? "https" : "http";

            return SessionConfiguration.Port == 80
                       ? String.Format("{0}://{1}/{2}", scheme, SessionConfiguration.AppHost, virtualPath)
                       : String.Format("{0}://{1}:{2}/{3}", scheme, SessionConfiguration.AppHost, SessionConfiguration.Port, virtualPath);
        }
    }
}