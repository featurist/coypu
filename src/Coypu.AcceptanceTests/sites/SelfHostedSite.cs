using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Coypu.AcceptanceTests.Sites
{
    public class SelfHostedSite : IDisposable
    {
        private readonly WebApplication _app;
        
        public SelfHostedSite()
        {
            _app = WebApplication.Create();
            _app.MapGet("/",
                () =>
                    Results.Content(
                        "<html><head><title>Selfish has taken the stage</title></head><body>Howdy</body></html>",
                        MediaTypeNames.Text.Html, Encoding.UTF8));
            _app.MapGet("/resource/bdd",
                () => "bdd");
            _app.MapGet("/auto_login",
                (HttpResponse res) =>
                {
                    res.Headers["Set-Cookie"] = "username=bob";
                });
            _app.MapGet("/restricted_resource/bdd",
                (HttpRequest req) =>
                {
                    if (req.Headers["Cookie"] == "username=bob")
                        return Results.Text("bdd");
                    return Results.NoContent();
                });

            _app.StartAsync().Wait();
        }

        public Uri BaseUri =>
            new(_app.Urls.First(url => url.StartsWith("http"))
                .Replace("localhost", "127.0.0.1"));

        public void Dispose()
        {
            _app.StopAsync().Wait();
        }
    }
}
