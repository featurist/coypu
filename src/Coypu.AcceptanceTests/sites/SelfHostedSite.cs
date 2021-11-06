using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Coypu.AcceptanceTests.Sites
{
    public class SelfHostedSite : IDisposable
    {
        private readonly WebApplication _app;
        
        public SelfHostedSite()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls($"https://{IPAddress.Loopback}:0");
            _app = builder.Build();
            
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
