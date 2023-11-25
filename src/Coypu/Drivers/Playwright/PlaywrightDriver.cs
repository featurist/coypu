using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Cookie = System.Net.Cookie;
using Microsoft.Playwright;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace Coypu.Drivers.Selenium
{
    public class PlaywrightDriver : IDriver
    {
        private readonly Browser _browser;
        private readonly IPlaywright _playwright;
        private readonly IBrowser _playwrightBrowser;
        private readonly IPage _playwrightPage;

        public PlaywrightDriver(Browser browser)
        {
            _browser = browser;

            Task<IPlaywright> createPlaywrightTask = Task.Run(() => Playwright.CreateAsync());
            createPlaywrightTask.Wait();
            _playwright = createPlaywrightTask.Result;

            var browserType = _playwright.Chromium; // TODO: map browser to playwright browser type

            Task<IBrowser> browserTask = Task.Run(() => browserType.LaunchAsync(
              new BrowserTypeLaunchOptions{Headless = false, }
            ));
            browserTask.Wait();
            _playwrightBrowser = browserTask.Result;

            Task<IPage> pageTask = Task.Run(() => _playwrightBrowser.NewPageAsync());
            pageTask.Wait();
            _playwrightPage = pageTask.Result;
        }
        protected bool NoJavascript => !_browser.Javascript;

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
          throw new Exception("Not implemented");
          // _elementFinder.SeleniumScope(scope);
          // return new Uri(_page.Url);
        }

        public string Title(Scope scope)
        {
          throw new Exception("Not implemented");
          // _elementFinder.SeleniumScope(scope);
          // return _page.Title;
        }

        public Cookies Cookies { get; set; }
        public object Native => _playwright;

    public Element Window => throw new NotImplementedException();

    public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            throw new NotImplementedException();
        }

        public bool HasDialog(string withText,
                              Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(string url,
                          Scope scope)
        {
          Task<IResponse> responseTask = Task.Run(() => _playwrightPage.GotoAsync(url));
          responseTask.Wait();
          IResponse response = responseTask.Result;
          if (response.Status != 200)
          {
            throw new Exception("Failed to load page");
          }
        }

        public void ClearBrowserCookies()
        {
            Cookies.DeleteAll();
        }

        public void Click(Element element)
        {
            throw new NotImplementedException();
        }

        public void Hover(Element element)
        {
            throw new NotImplementedException();
        }

        public void SendKeys(Element element,
                             string keys)
        {
            throw new NotImplementedException();
        }

        public void MaximiseWindow(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Refresh(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void ResizeTo(Size size,
                             Scope scope)
        {
            throw new NotImplementedException();
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
        {
            throw new NotImplementedException();
        }

        public void GoBack(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void GoForward(Scope scope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Element> FindWindows(string titleOrName,
                                                Scope scope,
                                                Options options)
        {
            throw new NotImplementedException();
        }

        public void Set(Element element,
                        string value)
        {
            throw new NotImplementedException();
        }

        public void AcceptModalDialog(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void CancelModalDialog(Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Check(Element field)
        {
            throw new NotImplementedException();
        }

        public void Uncheck(Element field)
        {
            throw new NotImplementedException();
        }

        public void Choose(Element field)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing)
            {
                // AcceptAnyAlert(); ??
            }

            Disposed = true;
        }
    }
}
