using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Coypu.Drivers;
using OpenQA.Selenium;
using Cookie = System.Net.Cookie;

namespace Coypu.Tests.TestDoubles
{
    public class StubDriver : IDriver
    {
        public StubDriver() {
          Cookies = new StubCookies();
        }

        public StubDriver(Browser browser, bool headless) : this() { }

        public void Dispose() { }

        public void ClearBrowserCookies() { }

        public void Click(Element element) { }

        public void DblClick(Element element) { }

        public void Visit(string url,
                          Scope scope) { }

        public void Set(Element element,
                        string value) { }

        public Cookies Cookies { get; set; }

        public object Native => "Native driver on stub driver";

        public bool HasDialog(string withText,
                              Scope scope)
        {
            return false;
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            return Enumerable.Empty<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            return Enumerable.Empty<Element>();
        }

        public void Check(Element field) { }

        public void Uncheck(Element field) { }

        public void Choose(Element field) { }

        public void SelectOption(Element select, Element option, string optionToSelect) { }

        public bool Disposed => false;

        Uri IDriver.Location(Scope scope)
        {
            return null;
        }

        public string Title(Scope scope)
        {
            return null;
        }

        public Element Window => null;

        public void AcceptModalDialog(Scope scope) { }

        public void CancelModalDialog(Scope scope) { }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            return null;
        }

        public void Hover(Element element) { }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return new List<Cookie>();
        }

        public IEnumerable<Element> FindWindows(string locator,
                                                Scope scope,
                                                Options options)
        {
            return Enumerable.Empty<Element>();
        }

        public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
        {
            return Enumerable.Empty<Element>();
        }

        public void SendKeys(Element element,
                             string keys) { }

        public void MaximiseWindow(Scope scope) { }

        public void Refresh(Scope scope) { }

        public void ResizeTo(Size size,
                             Scope Scope) { }

        public void SaveScreenshot(string fileName,
                                   Scope scope) { }

        public void GoBack(Scope scope) { }

        public void GoForward(Scope scope) { }

        public bool HasContent(string text,
                               Scope scope)
        {
            return false;
        }

        public bool HasContentMatch(Regex pattern,
                                    Scope scope)
        {
            return false;
        }

        public void SetScope(Element findScope) { }

        public void ClearScope() { }

        public Element FindIFrame(string locator,
                                  Scope scope)
        {
            return null;
        }

        public void SetBrowserCookies(Cookie cookie) { }

    public void AcceptAlert(string text, DriverScope root, Action trigger)
    {
        trigger.Invoke();
    }

    public void AcceptConfirm(string text, DriverScope root, Action trigger)
    {
        trigger.Invoke();
    }

    public void CancelConfirm(string text, DriverScope root, Action trigger)
    {
        trigger.Invoke();
    }

    public void AcceptPrompt(string text, string promptValue, DriverScope root, Action trigger)
    {
        trigger.Invoke();
    }

    public void CancelPrompt(string text, DriverScope root, Action trigger)
    {
        trigger.Invoke();
    }
  }

    // Implementation of Cookies interface that has no behaviour
    public class StubCookies : Cookies
    {
        public void AddCookie(Cookie cookie,
                              Options options = null) { }

        public void DeleteAll() { }

        public void DeleteCookie(Cookie cookie) { }

        public void DeleteCookieNamed(string cookieName) { }

        public IEnumerable<Cookie> GetAll()
        {
            return new List<Cookie>();
        }

        public Cookie GetCookieNamed(string cookieName)
        {
            return null;
        }

        public void WaitUntilCookieExists(Cookie cookie,
                                          Options options) {
        }
    }
}
