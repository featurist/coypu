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
                ? $"{scheme}://{userInfoPart}{sessionConfiguration.AppHost}"
                : $"{scheme}://{userInfoPart}{sessionConfiguration.AppHost}:{sessionConfiguration.Port}";

            return new Uri(new Uri(baseUrl), virtualPath).AbsoluteUri;
        }
    }
}