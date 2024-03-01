using System;

namespace Coypu
{
    internal class FullyQualifiedUrlBuilder : UrlBuilder
    {
        public string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration sessionConfiguration)
    {
      var scheme = sessionConfiguration.SSL ? "https" : "http";
      string userInfoPart = GetUserInfoPart(sessionConfiguration);
      var baseUrl = sessionConfiguration.Port == 80
          ? $"{scheme}://{userInfoPart}{sessionConfiguration.AppHost}"
          : $"{scheme}://{userInfoPart}{sessionConfiguration.AppHost}:{sessionConfiguration.Port}";

      return new Uri(new Uri(baseUrl), virtualPath).AbsoluteUri;
    }

    private static string GetUserInfoPart(SessionConfiguration sessionConfiguration)
    {
      // PlaywrightDriver implements basic auth properly via the Authorization header
      if (sessionConfiguration.Driver == typeof(Drivers.Playwright.PlaywrightDriver) ||
          string.IsNullOrEmpty(sessionConfiguration.UserInfo))
      {
        return string.Empty;
      }
      return sessionConfiguration.UserInfo + "@";
    }
  }
}
