using System;
using SelfishHttp;

namespace Coypu.AcceptanceTests.Sites
{
    public class SelfishSite : IDisposable
    {
        private readonly Server _server;

        public SelfishSite()
        {
            _server = new Server();
            _server.OnGet("/").RespondWith("<html><head><title>Selfish has taken the stage</title></head><body>Howdy</body></html>");
            _server.OnGet("/resource/bdd").RespondWith("bdd");
            _server.OnGet("/auto_login").Respond((req, res) =>
            {
                res.Headers["Set-Cookie"] = "username=bob";
            });
            _server.OnGet("/restricted_resource/bdd").Respond((req, res) =>
            {
                if (req.Headers["Cookie"] == "username=bob")
                    res.Body = "bdd";

            });
        }

        public Uri BaseUri
        {
            get { return new Uri(_server.BaseUri); }
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
