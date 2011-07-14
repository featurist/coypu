using System.Collections.Generic;
using System.Net;

namespace Coypu.Tests.TestDoubles
{
    public class StubWebResources : WebResources
    {
        private readonly Dictionary<string, WebResponse> resources = new Dictionary<string, WebResponse>();

        public void StubResource(string resource, StubWebResponse stubWebResponse)
        {
            resources[resource] = stubWebResponse;
        }

        public WebResponse Get(string resource)
        {
            return resources[resource];
        }
    }
}