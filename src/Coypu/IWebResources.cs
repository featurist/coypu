using System.Net;

namespace Coypu
{
    internal interface IWebResources
    {
        WebResponse Get(string resource);
    }

    internal class WebResources : IWebResources
    {
        public WebResponse Get(string resource)
        {
            return WebRequest.Create(resource).GetResponse();
        }
    }
}