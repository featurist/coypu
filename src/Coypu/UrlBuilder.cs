namespace Coypu
{
    public interface UrlBuilder
    {
        string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration sessionConfiguration);
    }
}