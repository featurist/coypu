using System;

namespace Coypu
{
    internal class FullyQualifiedUrlBuilder : UrlBuilder
    {
        public string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration sessionConfiguration)
        {
            var scheme = sessionConfiguration.SSL ? "https" : "http";
            var baseUrl = sessionConfiguration.Port == 80
                ? String.Format("{0}://{1}", scheme, sessionConfiguration.AppHost)
                : String.Format("{0}://{1}:{2}", scheme, sessionConfiguration.AppHost, sessionConfiguration.Port);

            return new Uri(new Uri(baseUrl), virtualPath).AbsoluteUri;
        }
    }
}