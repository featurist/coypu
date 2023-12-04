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
        private readonly IPlaywright _playwright;
        private readonly IBrowser _playwrightBrowser;
        private readonly IBrowserContext _context;

        public PlaywrightDriver(Browser browser)
        {
            _browser = browser;
            _playwright = Async.WaitForResult(Microsoft.Playwright.Playwright.CreateAsync());

            var browserType = _playwright.Chromium; // TODO: map browser to playwright browser type

            _playwrightBrowser = Async.WaitForResult(browserType.LaunchAsync(
              new BrowserTypeLaunchOptions{
                  Headless = false,
                }
            ));
            var page = Async.WaitForResult(_playwrightBrowser.NewPageAsync());
            _context = page.Context;
            _context.SetDefaultTimeout(1000); // TODO: Work out how to set actionTimeout only and remove this

            Cookies = new Cookies(_context);

            // page.Dialog += async (_, dialog) =>
            // {
            //     // TODO: wait here async for something to happen
            //     // Record open dialog for HasDialog
            //     // When user calls AcceptDialog or CancelDialog
            //     //   set something so we know to DismissAsync or AcceptAsync
            //     // NB Need to do this for every page that is opened somehow
            //     await dialog.DismissAsync();
            // };
        }
        protected bool NoJavascript => !_browser.Javascript;

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
          return new Uri(PlaywrightPage(scope).Url);
        }

        public string Title(Scope scope)
        {
          return Async.WaitForResult((PlaywrightPage(scope)).TitleAsync());
        }

        public Coypu.Cookies Cookies { get; set; }
        public object Native => _playwright;

        public Element Window => new PlaywrightWindow(_playwrightBrowser, _context.Pages.First());

      public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
        {
            var nativeScope = scope.Now().Native;
            var page = nativeScope as IPage ??
                       Async.WaitForResult(
                          ((IElementHandle) nativeScope).OwnerFrameAsync()
                        ).Page;
            return new FrameFinder(page).FindFrame(
                locator,
                Async.WaitForResult(page.QuerySelectorAllAsync("iframe,frame")),
                options
            );
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            var results = Async.WaitForResult(Element(scope).QuerySelectorAllAsync($"css={cssSelector}"))
                          .Where(ValidateTextPattern(options, textPattern))
                          .Where(e => IsDisplayed(e, options))
                          .Select(BuildElement);
            return results;
        }

        private Func<IElementHandle, bool> ValidateTextPattern(Options options, Regex textPattern)
        {
          if (options == null) throw new ArgumentNullException(nameof(options));
          Func<IElementHandle, bool> textMatches = e =>
            {
              if (textPattern == null) return true;

              var text = Async.WaitForResult(e.InnerTextAsync());
              return text != null && textPattern.IsMatch(text.Trim());
            };

          if (textPattern != null && options.ConsiderInvisibleElements)
            throw new NotSupportedException("Cannot inspect the text of invisible elements.");
          return textMatches;
        }

        private bool IsDisplayed(IElementHandle e,
                                Options options)
        {
            return options.ConsiderInvisibleElements || Async.WaitForResult(e.IsVisibleAsync());
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            return Async.WaitForResult(Element(scope).QuerySelectorAllAsync($"xpath={xpath}"))
                        .Where(e => IsDisplayed(e, options))
                        .Select(BuildElement);
        }

        private Element BuildElement(IElementHandle element)
        {
            var tagName = Async.WaitForResult(element.EvaluateAsync("e => e.tagName"))?.GetString();

            Element coypuElement = new[] {"iframe", "frame"}.Contains(tagName.ToLower())
                     ? (Element) new PlaywrightFrame(element) : new PlaywrightElement(element);

            return coypuElement;
        }

        public bool HasDialog(string withText,
                              Scope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(string url,
                          Scope scope)
        {
            IResponse response = Async.WaitForResult(PlaywrightPage(scope).GotoAsync(url));
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
            Async.WaitForResult(PlaywrightElement(element).ClickAsync());
        }

        public void Hover(Element element)
        {
            Async.WaitForResult(PlaywrightElement(element).HoverAsync());
        }

        public void SendKeys(Element element,
                             string keys)
        {
            var playwrightElement = PlaywrightElement(element);
            Async.WaitForResult(playwrightElement.FocusAsync());
            Async.WaitForResult(playwrightElement.EvaluateAsync("e => e.setSelectionRange(-1, -1)"));
            keys.ToList().ForEach(key => Async.WaitForResult(playwrightElement.PressAsync(key.ToString())));
        }

        public void MaximiseWindow(Scope scope)
        {
            throw new NotSupportedException("MaximiseWindow is not currently supported by Playwright. https://github.com/microsoft/playwright/issues/4046");
        }

        public void Refresh(Scope scope)
        {
            Async.WaitForResult(((IPage )scope.Now().Native).ReloadAsync());
        }

        public void ResizeTo(Size size,
                             Scope scope)
        {
            if (_browser == Browser.Chrome) {
                size = new Size(size.Width - 2, size.Height - 80);
            }
            Async.WaitForResult(PlaywrightPage(scope).SetViewportSizeAsync(size.Width, size.Height));
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
        {
            Async.WaitForResult(PlaywrightPage(scope).ScreenshotAsync(new PageScreenshotOptions
            {
                Path = fileName
            }));
        }

        public void GoBack(Scope scope)
        {
            Async.WaitForResult(PlaywrightPage(scope).GoBackAsync());
        }

        public void GoForward(Scope scope)
        {
            Async.WaitForResult(PlaywrightPage(scope).GoForwardAsync());
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return Cookies.GetAll();
        }

        public IEnumerable<Element> FindWindows(string titleOrName,
                                                Scope scope,
                                                Options options)
        {
            return _context.Pages
                   .Select(p => new PlaywrightWindow(_playwrightBrowser, p))
                   .Where(window => {
                      return
                        options.TextPrecisionExact && (
                          window.Title == titleOrName
                        ) ||
                        !options.TextPrecisionExact && (
                          window.Title.Contains(titleOrName)
                        );
                      }
                   );
        }

        public void Set(Element element,
                        string value)
        {
            var input = PlaywrightElement(element);
            if (element["type"] == "file")
                Async.WaitForResult(input.SetInputFilesAsync(value));
            else
                Async.WaitForResult(input.FillAsync(value));
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
            if (!field.Selected)
                Async.WaitForResult(PlaywrightElement(field).CheckAsync());
        }

        public void Uncheck(Element field)
        {
            if (field.Selected)
                Async.WaitForResult(PlaywrightElement(field).UncheckAsync());
        }

        public void Choose(Element field)
        {
            Async.WaitForResult(PlaywrightElement(field).CheckAsync());
        }

        public void SelectOption(Element select, Element option, string optionToSelect)
        {
            Async.WaitForResult(PlaywrightElement(select).SelectOptionAsync(optionToSelect));
        }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            var func = $"(arguments) => {Regex.Replace(javascript, "^return ", string.Empty)}";
            return Async.WaitForResult(
              (PlaywrightPage(scope))
                .EvaluateAsync(func, ConvertScriptArgs(args)))
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
                return Async.WaitForResult(frame.QuerySelectorAsync("html"));
            }
            var page = scopeElement.Native as IPage;
            if (page != null) {
                return Async.WaitForResult(page.QuerySelectorAsync("html"));
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
            if (disposing)
            {
                // AcceptAnyAlert(); ??
            }

            Disposed = true;
        }
    }
}
