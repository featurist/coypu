using System;
using System.Collections.Generic;
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
            builder.WebHost.UseUrls($"http://{IPAddress.Loopback}:0");
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
            _app.MapGet("/headers",
                 (HttpRequest req, HttpResponse res) =>
                {
                    if (string.IsNullOrEmpty(req.Headers["Authorization"])) {
                        res.Headers.Add("WWW-Authenticate", "Basic realm= \"Coypu Test\"");
                        return Results.Unauthorized();
                    };
                    return Results.Content(
                      "<HTML><BODY>" +
                      req.Headers.Select(h => $"{h.Key}: {h.Value}").Aggregate((a, b) => $"{a}: {b}</br>") +
                      "<img src=\"/235.gif\" />" +
                      "<img src=\"https://github.com/featurist/coypu/pull/235.gif\" />" +
                      "</BODY></HTML>",
                          MediaTypeNames.Text.Html, Encoding.UTF8);
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
