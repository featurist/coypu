using System.Net;

namespace Coypu
{
    internal interface WebResources
    {
        WebResponse Get(string resource);
    }
}