using System;

namespace Coypu
{
	public class UrlBuilder
	{
		public string GetFullyQualifiedUrl(string virtualPath)
		{
			if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
				return virtualPath;

			virtualPath = virtualPath.TrimStart('/');
			var scheme = Configuration.SSL ? "https" : "http";

			return Configuration.Port == 80
			       	? String.Format("{0}://{1}/{2}", scheme, Configuration.AppHost, virtualPath)
			       	: String.Format("{0}://{1}:{2}/{3}", scheme, Configuration.AppHost, Configuration.Port, virtualPath);
		}
	}
}