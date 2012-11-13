using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

using SHDocVw;

using WatiN.Core;
using WatiN.Core.Comparers;
using WatiN.Core.Constraints;
using mshtml;

namespace Coypu.Drivers.Watin
{
    public class WatiNDriver : Driver
    {
        private readonly ElementFinder elementFinder;

        private DialogHandler watinDialogHandler;

        static WatiNDriver()
        {
            ElementFactory.RegisterElementType(typeof(Fieldset));
            ElementFactory.RegisterElementType(typeof(Section));
        }

        public WatiNDriver(Browser browser)
        {
            if (browser != Browser.InternetExplorer)
                throw new BrowserNotSupportedException(browser, GetType());

            Settings.AutoMoveMousePointerToTopLeft = false;

            Watin = CreateBrowser();
            elementFinder = new ElementFinder();
        }

        private WatiN.Core.Browser CreateBrowser()
        {
            var browser = new IEWithDialogWaiter();

            watinDialogHandler = new DialogHandler();
            browser.AddDialogHandler(watinDialogHandler);

            return browser;
        }

        public void SetBrowser(WatiN.Core.Browser browser)
        {
            Watin.Dispose();
            browser.AddDialogHandler(watinDialogHandler);
            Watin = browser;
        }

        internal WatiN.Core.Browser Watin { get; private set; }

        private static WatiN.Core.Element WatiNElement(Element element)
        {
            return WatiNElement<WatiN.Core.Element>(element);
        }

        private static T WatiNElement<T>(Element element)
            where T : WatiN.Core.Element
        {
            return element.Native as T;
        }

        private static ElementFound BuildElement(WatiN.Core.Element element, string description)
        {
            if (element == null)
                throw new MissingHtmlException(description);
            return BuildElement(element);
        }

        private static ElementFound BuildElement(WatiN.Core.Element element)
        {
            return new WatiNElement(element);
        }

        private static ElementFound BuildElement(WatiN.Core.Browser browser)
        {
            return new WatiNBrowser(browser);
        }

        private static ElementFound BuildElement(Frame frame, string description)
        {
            if (frame == null)
                throw new MissingHtmlException(description);
            return new WatiNFrame(frame);
        }

        public string ExecuteScript(string javascript, Scope scope)
        {
            // TODO: scope is the current window in which to accept a dialog

            var stripReturn = Regex.Replace(javascript, @"^\s*return ", "");
            var retval = Watin.Eval(stripReturn);
            Watin.WaitForComplete();
            return retval;
        }

        public ElementFound FindFieldset(string locator, Scope scope)
        {
            return BuildElement(elementFinder.FindFieldset(locator, scope), "Failed to find fieldset: " + locator);
        }

        public ElementFound FindSection(string locator, Scope scope)
        {
            return BuildElement(elementFinder.FindSection(locator, scope), "Failed to find section: " + locator);
        }

        public ElementFound FindId(string id, Scope scope)
        {
            return BuildElement(elementFinder.FindElement(id, scope), "Failed to find id: " + id);
        }

        public void Hover(Element element)
        {
            WatiNElement(element).FireEvent("onmouseover");
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            var ieBrowser = Watin as IE;
            if (ieBrowser == null)
                throw new NotSupportedException("Only supported for Internet Explorer");

            var persistentCookies = GetPersistentCookies(ieBrowser).ToList();
            var documentCookies = GetCookiesFromDocument(ieBrowser);

            var sessionCookies = documentCookies.Except(persistentCookies, new CookieNameEqualityComparer());

            return persistentCookies.Concat(sessionCookies).ToList();
        }

        public ElementFound FindWindow(string locator, Scope scope)
        {
            var by =
                Find.ByTitle(locator) |
                Find.By("hwnd", locator);

            if (Uri.IsWellFormedUriString(locator,UriKind.Absolute))
                by |= Find.ByUrl(locator);

            try
            {
                var window = WatiN.Core.Browser.AttachTo<IE>(by);
                return new WatiNBrowser(window);
            }
            catch (WatiN.Core.Exceptions.BrowserNotFoundException)
            {
                throw new MissingHtmlException("No such window found: " + locator);
            }
        }

        public ElementFound FindFrame(string locator, Scope scope)
        {
            return BuildElement(elementFinder.FindFrame(locator, scope), "Failed to find frame: " + locator);
        }

        public void SendKeys(Element element, string keys)
        {
            foreach (var key in keys)
            {
                WatiNElement<WatiN.Core.Element>(element).KeyPressNoWait(key);
            }
        }

        private IEnumerable<Cookie> GetPersistentCookies(IE ieBrowser)
        {
            return ieBrowser.GetCookiesForUrl(Watin.Uri).Cast<Cookie>();
        }

        private IEnumerable<Cookie> GetCookiesFromDocument(IE ieBrowser)
        {
            var document = ((IWebBrowser2)ieBrowser.InternetExplorer).Document as IHTMLDocument2;
            if (document == null)
                throw new InvalidOperationException("Cannot get IE document for cookies");

            return from untrimmedCookie in document.cookie.Split(';')
                   let cookie = untrimmedCookie.Trim()
                   let index = cookie.IndexOf('=')
                   let name = cookie.Substring(0, index)
                   let value = cookie.Substring(index + 1, cookie.Length - index - 1)
                   select new Cookie(name, value);
        }

        public ElementFound FindButton(string locator, Scope scope)
        {
            return BuildElement(elementFinder.FindButton(locator, scope), "Failed to find button with text, id or name: " + locator);
        }

        public ElementFound FindLink(string linkText, Scope scope)
        {
            return BuildElement(elementFinder.FindLink(linkText, scope), "Failed to find link with text: " + linkText);
        }

        public ElementFound FindField(string locator, Scope scope)
        {
            return BuildElement(elementFinder.FindField(locator, scope), "Failed to find field with label, id, name or placeholder: " + locator);
        }

        public void Click(Element element)
        {
            // If we use Click, then we can get a deadlock if IE is displaying a modal dialog.
            // (Yay COM STA!) Our override of the IE class makes sure the WaitForComplete will
            // check to see if the main window is disabled before continuing with the normal wait
            var nativeElement = WatiNElement(element);
            nativeElement.ClickNoWait();
            nativeElement.WaitForComplete();
        }

        public void Visit(string url)
        {
            Watin.GoTo(url);
        }

        public void Set(Element element, string value, bool forceAllEvents)
        {
            var textField = WatiNElement<TextField>(element);
            if (textField != null)
            {
                textField.Value = value;
                return;
            }
            var fileUpload = WatiNElement<FileUpload>(element);
            if (fileUpload != null)
                fileUpload.Set(value);
        }

        public void Select(Element element, string option)
        {
            WatiNElement<SelectList>(element).SelectByTextOrValue(option);
        }

        public object Native
        {
            get { return Watin; }
        }

        public bool HasContent(string text, Scope scope)
        {
            return scope.Now().Text.Contains(text);
        }

        public bool HasContentMatch(Regex pattern, Scope scope)
        {
            var watiNScope = ElementFinder.WatiNScope(scope);
            if (watiNScope == Window.Native)
            {
                return ((WatiN.Core.Browser)Window.Native).ContainsText(pattern);
            }
            return pattern.IsMatch(((WatiN.Core.Element)watiNScope).Text);
        }

        public void Check(Element field)
        {
            WatiNElement<CheckBox>(field).Checked = true;
        }

        public void Uncheck(Element field)
        {
            WatiNElement<CheckBox>(field).Checked = false;
        }

        public void Choose(Element field)
        {
            WatiNElement<RadioButton>(field).Checked = true;
        }

        public bool HasDialog(string withText, Scope scope)
        {
            // TODO: scope is the current window in which to look for a dialog
            return watinDialogHandler.Exists() && watinDialogHandler.Message == withText;
        }

        public ElementFound Window
        {
            get { return BuildElement(Watin); }
        }

        public void AcceptModalDialog(Scope scope)
        {
            // TODO: scope is the current window in which to accept a dialog
            watinDialogHandler.ClickOk();
        }

        public void CancelModalDialog(Scope scope)
        {
            // TODO: scope is the current window in which to accept a dialog
            watinDialogHandler.ClickCancel();
        }

        public bool HasCss(string cssSelector, Scope scope)
        {
            return elementFinder.HasCss(cssSelector, scope);
        }

        public bool HasXPath(string xpath, Scope scope)
        {
            return elementFinder.HasXPath(xpath, scope);
        }

        public ElementFound FindCss(string cssSelector, Scope scope)
        {
            return BuildElement(elementFinder.FindCss(cssSelector, scope), "No element found by css: " + cssSelector);
        }

        public ElementFound FindXPath(string xpath, Scope scope)
        {
            return BuildElement(elementFinder.FindXPath(xpath, scope), "No element found by xpath: " + xpath);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope)
        {
            return from element in elementFinder.FindAllCss(cssSelector, scope)
                   select BuildElement(element);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope)
        {
            return from element in elementFinder.FindAllXPath(xpath, scope)
                   select BuildElement(element);
        }

        public Uri Location(Scope scope)
        {
                //return Watin.Uri;
                throw new NotImplementedException("Consider scope");
        }

        public string Title(Scope scope)
        {
            //return Watin.Title;
            throw new NotImplementedException("Consider scope");
        }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            watinDialogHandler.Dispose();
            Watin.Dispose();
            Disposed = true;
        }
    }
}
