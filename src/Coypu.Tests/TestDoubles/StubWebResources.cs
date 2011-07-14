using System.Collections.Generic;
using System.Net;

namespace Coypu.Tests.TestDoubles
{
    public class StubWebResources : IWebResources
    {
        private readonly Dictionary<string, WebResponse> resources = new Dictionary<string, WebResponse>();

        public void StubResource(string resource, StubWebResponse stubWebResponse)
        {
            resources[resource] = stubWebResponse;
        }

        public WebResponse Get(string resource)
        {
            if (!resources.ContainsKey(resource))
                throw new System.ArgumentException("There was no resource at " + resource, "resource");

            return resources[resource];
        }
    }
}