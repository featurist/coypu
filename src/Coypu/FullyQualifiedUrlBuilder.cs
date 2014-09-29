using System;

namespace Coypu
{
    internal class FullyQualifiedUrlBuilder : UrlBuilder
    {
        public string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration sessionConfiguration)
        {
            var scheme = sessionConfiguration.SSL ? "https" : "http";
            var userInfoPart = string.IsNullOrEmpty(sessionConfiguration.UserInfo) ? "" : sessionConfiguration.UserInfo + "@";
            var baseUrl = sessionConfiguration.Port == 80
                ? String.Format("{0}://{1}{2}", scheme, userInfoPart, sessionConfiguration.AppHost)
                : String.Format("{0}://{1}{2}:{3}", scheme, userInfoPart, sessionConfiguration.AppHost, sessionConfiguration.Port);

            return new Uri(new Uri(baseUrl), virtualPath).AbsoluteUri;
        }
    }
}