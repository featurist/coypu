using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Coypu.AcceptanceTests.Sites
{
    public class SelfishSite : IDisposable
    {
        public static Action<IRouteBuilder> MatchesRequest
        {
            get
            {
                return rb => rb.MapGet("",
                    (req, resp, routeData) =>
                        resp.WriteAsync("<html><head><title>Selfish has taken the stage</title></head><body>Howdy</body></html>") 
                    );
                    
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



            //_server = new Server();
            //_server.OnGet("/").RespondWith("<html><head><title>Selfish has taken the stage</title></head><body>Howdy</body></html>");
            //_server.OnGet("/resource/bdd").RespondWith("bdd");
            //_server.OnGet("/auto_login").Respond((req, res) =>
            //{
            //    res.Headers["Set-Cookie"] = "username=bob";
            //});
            //_server.OnGet("/restricted_resource/bdd").Respond((req, res) =>
            //{
            //    if (req.Headers["Cookie"] == "username=bob")
            //        res.Body = "bdd";

            //});
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
