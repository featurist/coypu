using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Coypu.Drivers;
using Coypu.Drivers.Playwright;
using Coypu.Drivers.Selenium;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebDriverManager.DriverConfigs.Impl;

namespace Coypu.Tests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ProxyTests
{
    private static WebApplication _app;
    private static string _proxyServer;

    [OneTimeSetUp]
    public static void SetUp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile("proxysettings.json");

        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

        _app = builder.Build();

        _app.UseRouting();
        _app.MapReverseProxy();

        _app.MapGet("/acceptance-test", context =>
            {
                const string html = "<!DOCTYPE html><html><head></head><body><h1 id=\"title\">This page has been proxied successfully.</h1></body></html>";

                context.Response.ContentType = MediaTypeNames.Text.Html;
                context.Response.ContentLength = Encoding.UTF8.GetByteCount(html);
                return context.Response.WriteAsync(html);
            }
        );

        _app.RunAsync();

        _proxyServer = _app.Services.GetRequiredService<IServer>()
            .Features.Get<IServerAddressesFeature>()
            .Addresses
            .First();
    }

    [OneTimeTearDown]
    public static async Task TearDown()
    {
        await _app.DisposeAsync();
    }

    [TestCase(typeof(PlaywrightDriver))]
    [TestCase(typeof(SeleniumWebDriver))]
    public void Driver_Uses_Proxy(Type driverType)
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
        var sessionConfiguration = new SessionConfiguration
        {
            AcceptInsecureCertificates = true,
            Proxy = new DriverProxy
            {
                Server = _proxyServer,
            },
            Browser = Browser.Chrome,
            Driver = driverType,
        };

        using var browser = new BrowserSession(sessionConfiguration);

        // Proxy turns this example.com into localhost, which has an endpoint configured for this URL in the setup
        browser.Visit("http://www.example.com/acceptance-test");

        // So we then assert we can find the title in the HTML we've served
        var title = browser.FindId("title");

        Assert.That(title.Exists(), Is.True);
        Assert.That(title.Text, Is.EqualTo("This page has been proxied successfully."));
    }
}