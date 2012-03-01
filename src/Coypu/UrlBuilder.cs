namespace Coypu
{
    internal interface UrlBuilder
    {
        string GetFullyQualifiedUrl(string virtualPath, Configuration configuration);
    }
}