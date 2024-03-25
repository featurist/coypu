using System;
using System.Linq;
using System.Threading.Tasks;
using Coypu.Drivers;
using Coypu.Drivers.Playwright;
using Coypu.Drivers.Selenium;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebDriverManager.DriverConfigs.Impl;

namespace Coypu.Tests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ProxyTests
{
    private WebApplication _app;
    private string _proxyServer;

    [SetUp]
    public void SetUp()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddJsonFile("proxysettings.json");
        
        builder.Services.AddReverseProxy()
            .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
        
        _app = builder.Build();
        
        _app.UseRouting();
        _app.MapReverseProxy();
        
        _app.RunAsync();

        _proxyServer = _app.Services.GetRequiredService<IServer>()
            .Features.Get<IServerAddressesFeature>()
            .Addresses
            .First();
    }

    [TearDown]
    public async Task TearDown()
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
            Driver = driverType
        };
        
        using var browser = new BrowserSession(sessionConfiguration);
        
        // Proxy turns this example.com into github.com
        browser.Visit("http://www.example.com");

        // So we then assert we can find the GitHub Octo Icon
        var icon = browser.FindCss(".octicon-mark-github");
        
        Assert.That(icon.Exists(), Is.True);
    }
}