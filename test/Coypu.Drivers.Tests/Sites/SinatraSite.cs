using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Coypu.Drivers.Tests.Sites
{
    public class SelfishSite : IDisposable
    {
        public static Action<IRouteBuilder> MatchesRequest
        {
            get
            {
                return rb => rb
                    .MapGet("", (req, resp, routeData) =>
                        resp.WriteAsync("<html><head><title>Selfish has taken the stage</title></head><body>Howdy</body></html>"))
                    .MapGet("resource/bdd", (req, resp, routeData) => resp.WriteAsync("bdd"))
                    .MapGet("auto_login", (req, resp, routeData) => {
                        resp
                            .Headers
                            .Add("Set-Cookie", new Microsoft.Extensions.Primitives.StringValues("username=bob"));

                        return Task.CompletedTask;
                    })
                    .MapGet("restricted_resource/bdd", (req, resp, routeData) => {
                        if (resp.Headers["Set-Cookie"] == "username=bob")
                        {
                            return resp.WriteAsync("bdd");
                        }
                        return Task.CompletedTask;
                    });

                //resp.WriteAsync($"Hello! {routeData.Values["name"]}")),
                //    new HttpRequestMessage(HttpMethod.Get, "greeting/James"),
                //    "Hello! James"
            }
        }


        private readonly TestServer _server;

        public HttpClient Client { get; }

        public SelfishSite()
        {
            var webhostbuilder = new WebHostBuilder();
            webhostbuilder
                .ConfigureServices(services => services.AddRouting())
                .Configure(app =>
                {
                    app.UseRouter(MatchesRequest);
                });

            _server = new TestServer(webhostbuilder);
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost");
        }

        public Uri BaseUri
        {
            get { return Client.BaseAddress; }
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}