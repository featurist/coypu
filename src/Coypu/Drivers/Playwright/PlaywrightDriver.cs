using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Cookie = System.Net.Cookie;
using Microsoft.Playwright;

#pragma warning disable 1591

namespace Coypu.Drivers.Playwright
{
    public class PlaywrightDriver : IDriver
    {
        private readonly Browser _browser;
        private readonly bool _headless;
    private readonly Dialogs _dialogs;
    private readonly IPlaywright _playwright;
        private readonly IBrowser _playwrightBrowser;
        private readonly IBrowserContext _context;

        public PlaywrightDriver(Browser browser, bool headless)
        {
            _dialogs = new Dialogs();
            _playwright = Microsoft.Playwright.Playwright.CreateAsync().Sync();
            _browser = browser;
            _headless = headless;
            var browserType = PlaywrightBrowserType(browser, _playwright); // TODO: map browser to playwright browser type

            _playwrightBrowser = browserType.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                Headless = headless,
                Channel = PlaywrightBrowserChannel(browser)
                }
            ).Sync();
            var page = _playwrightBrowser.NewPageAsync().Sync();
            _context = page.Context;

            Cookies = new Cookies(_context);
            _context.SetDefaultTimeout(10000);
    }

    private string PlaywrightBrowserChannel(Browser browser)
        {
            if (browser == Browser.Chrome)
                return "chrome";
            if (browser == Browser.Edge)
                return "msedge";

            return null;
        }

        private IBrowserType PlaywrightBrowserType(Browser browser, IPlaywright playwright)
        {
            if (browser == Browser.Chrome)
                return playwright.Chromium;
            if (browser == Browser.Edge)
                return playwright.Chromium;
            if (browser == Browser.Chromium)
                return playwright.Chromium;
            if (browser == Browser.Firefox)
                return playwright.Firefox;
            if (browser == Browser.Webkit)
                return playwright.Webkit;

            throw new NotSupportedException($"Browser {browser} is not supported by Playwright");
        }

        protected bool NoJavascript => !_browser.Javascript;

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
          return new Uri(PlaywrightPage(scope).Url);
        }

        public string Title(Scope scope)
        {
          return PlaywrightPage(scope).TitleAsync().Sync();
        }

        public Coypu.Cookies Cookies { get; set; }
        public object Native => _context;

        public Element Window => new PlaywrightWindow(_context.Pages.First());

      public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
    {
      IPage page = Page(scope);
      return new FrameFinder(page).FindFrame(
          locator,
          page.QuerySelectorAllAsync("iframe,frame").Sync(),
          options
      );
    }

    private static IPage Page(Scope scope)
    {
      var nativeScope = scope.Now().Native;
      var page = nativeScope as IPage ??
                    ((IElementHandle)nativeScope).OwnerFrameAsync().Sync().Page;
      return page;
    }

    public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            try {
              var results = Element(scope).QuerySelectorAllAsync($"css={cssSelector}").Sync()
                              .Where(ValidateTextPattern(options, textPattern))
                              .Where(e => IsDisplayed(e, options))
                              .Select(BuildElement);
                return results;
            }
            catch (AggregateException e)
            {
                throw new StaleElementException(e);
            }
        }

        private Func<IElementHandle, bool> ValidateTextPattern(Options options, Regex textPattern)
        {
          if (options == null) throw new ArgumentNullException(nameof(options));
          Func<IElementHandle, bool> textMatches = e =>
            {
              if (textPattern == null) return true;

              var text = e.InnerTextAsync().Sync();
              return text != null && textPattern.IsMatch(text.Trim());
            };

          if (textPattern != null && options.ConsiderInvisibleElements)
            throw new NotSupportedException("Cannot inspect the text of invisible elements.");
          return textMatches;
        }

        private bool IsDisplayed(IElementHandle e,
                                Options options)
        {
            return options.ConsiderInvisibleElements || e.IsVisibleAsync().Sync();
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            try {
              return Element(scope).QuerySelectorAllAsync($"xpath={xpath}").Sync()
                        .Where(e => IsDisplayed(e, options))
                        .Select(BuildElement);
            }
            catch (AggregateException e)
            {
                throw new StaleElementException(e);
            }
        }

        private Element BuildElement(IElementHandle element)
        {
            var tagName = element.EvaluateAsync("e => e.tagName").Sync()?.GetString();

            Element coypuElement = new[] {"iframe", "frame"}.Contains(tagName.ToLower())
                     ? (Element) new PlaywrightFrame(element) : new PlaywrightElement(element);

            return coypuElement;
        }

        public void AcceptAlert(string text, DriverScope scope, Action trigger)
        {
            _dialogs.ActOnDialog(text, Page(scope), trigger, DialogType.Alert, dialog => dialog.AcceptAsync());
        }

        public void AcceptConfirm(string text, DriverScope scope, Action trigger)
        {
            _dialogs.ActOnDialog(text, Page(scope), trigger, DialogType.Confirm, dialog => dialog.AcceptAsync());
        }

        public void CancelConfirm(string text, DriverScope scope, Action trigger)
        {
            _dialogs.ActOnDialog(text, Page(scope), trigger, DialogType.Confirm, dialog => dialog.DismissAsync());
        }

        public void AcceptPrompt(string text, string promptTalue, DriverScope scope, Action trigger)
        {
            _dialogs.ActOnDialog(text, Page(scope), trigger, DialogType.Prompt, dialog => dialog.AcceptAsync(promptTalue));
        }

        public void CancelPrompt(string text, DriverScope scope, Action trigger)
        {
            _dialogs.ActOnDialog(text, Page(scope), trigger, DialogType.Prompt, dialog => dialog.DismissAsync());
        }

        public void Visit(string url,
                          Scope scope)
        {
            IResponse response = PlaywrightPage(scope).GotoAsync(url).Sync();
            if (response != null && response.Status != 200)
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
            PlaywrightElement(element).ClickAsync().Sync();
        }

        public void Hover(Element element)
        {
            PlaywrightElement(element).HoverAsync().Sync();
        }

        public void SendKeys(Element element,
                             string keys)
        {
            var playwrightElement = PlaywrightElement(element);
            playwrightElement.FocusAsync().Sync();
            playwrightElement.EvaluateAsync("e => e.setSelectionRange(-1, -1)").Sync();
            keys.ToList().ForEach(key => playwrightElement.PressAsync(key.ToString()).Sync());
        }

        public void MaximiseWindow(Scope scope)
        {
            throw new NotSupportedException("MaximiseWindow is not currently supported by Playwright. https://github.com/microsoft/playwright/issues/4046");
        }

        public void Refresh(Scope scope)
        {
            ((IPage )scope.Now().Native).ReloadAsync().Sync();
        }

        public void ResizeTo(Size size,
                             Scope scope)
        {
            if (_playwrightBrowser.BrowserType == _playwright.Chromium && !_headless) {
                size = new Size(size.Width - 2, size.Height - 80);
            }
            PlaywrightPage(scope).SetViewportSizeAsync(size.Width, size.Height).Sync();
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
        {
            PlaywrightPage(scope).ScreenshotAsync(new PageScreenshotOptions
            {
                Path = fileName
            }).Sync();
        }

        public void GoBack(Scope scope)
        {
            PlaywrightPage(scope).GoBackAsync().Sync();
        }

        public void GoForward(Scope scope)
        {
            PlaywrightPage(scope).GoForwardAsync().Sync();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return Cookies.GetAll();
        }

        public IEnumerable<Element> FindWindows(string titleOrName,
                                                Scope scope,
                                                Options options)
        {
          try
          {
              return _context.Pages
                  .Select(p => new PlaywrightWindow(p))
                  .Where(window => {
                    return
                      options.TextPrecisionExact && (
                        window.Title == titleOrName
                      ) ||
                      !options.TextPrecisionExact && (
                        window.Title.Contains(titleOrName)
                      );
                  });
            }
            catch (PlaywrightException ex)
            {
                throw new MissingWindowException("The active window was closed.", ex);
            }

        }

        public void Set(Element element,
                        string value)
        {
            var input = PlaywrightElement(element);
            if (element["type"] == "file")
                input.SetInputFilesAsync(value).Sync();
            else
                input.FillAsync(value).Sync();
        }

        public void AcceptModalDialog(Scope scope)
        {
            throw new NotImplementedException("AcceptModalDialog is not supported by Playwright. Please wrap your action that will trigger the modal dialog within `Browser.AcceptAlert/AcceptConfirm/AcceptPrompt(...)`");
        }

        public void CancelModalDialog(Scope scope)
        {
            throw new NotImplementedException("AcceptModalDialog is not supported by Playwright. Please wrap your action that will trigger the modal dialog within `Browser.CancelConfirm/CancelPrompt(...)`");
        }

        public bool HasDialog(string withText,
                              Scope scope)
        {
            throw new NotImplementedException("HasModalDialog is not supported by Playwright. Please wrap your action that will trigger the modal dialog within `Browser.[Accepts/Cancels][Alert/Confirm/Prompt](...");
        }

        public void Check(Element field)
        {
            if (!field.Selected)
                PlaywrightElement(field).CheckAsync().Sync();
        }

        public void Uncheck(Element field)
        {
            if (field.Selected)
                PlaywrightElement(field).UncheckAsync().Sync();
        }

        public void Choose(Element field)
        {
            PlaywrightElement(field).CheckAsync().Sync();
        }

        public void SelectOption(Element select, Element option, string optionToSelect)
        {
            PlaywrightElement(select).SelectOptionAsync(optionToSelect).Sync();
        }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            var func = $"(arguments) => {Regex.Replace(javascript, "^return ", string.Empty)}";
            return
              PlaywrightPage(scope)
                .EvaluateAsync(func, ConvertScriptArgs(args)).Sync()
                .ToString();
        }

        // TODO: extract duplication between Drivers
        private static object[] ConvertScriptArgs(object[] args)
        {
            for (var i = 0; i < args.Length; ++i)
                if (args[i] is Element argAsElement)
                    args[i] = argAsElement.Native;

            return args;
        }

        private IElementHandle PlaywrightElement(Element element)
        {
            return (IElementHandle) element.Native;
        }

        private IPage PlaywrightPage(Scope scope)
        {
          return (IPage) scope.Now().Native;
        }

        private IElementHandle Element(Scope scope)
        {
            var scopeElement = scope.Now();
            var frame = scopeElement.Native as IFrame;
            if (frame != null) {
                return frame.QuerySelectorAsync("html").Sync();
            }
            var page = scopeElement.Native as IPage;
            if (page != null) {
                return page.QuerySelectorAsync("html").Sync();
            }
            return (IElementHandle) scopeElement.Native;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;

            _playwright.Dispose();
            Disposed = true;
        }
    }
}
