using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Cookie = System.Net.Cookie;

#pragma warning disable 1591

namespace Coypu
{
    public interface IDriver : IDisposable
    {
        Cookies Cookies { get; set; }
        object Native { get; }
        bool Disposed { get; }
        IEnumerable<Element> FindAllCss(string cssSelector, Scope scope, Options options, Regex textPattern = null);
        IEnumerable<Element> FindAllXPath(string xpath, Scope scope, Options options);
        IEnumerable<Element> FindWindows(string locator, Scope scope, Options options);
        IEnumerable<Element> FindFrames(string locator, Scope scope, Options options);
        Element Window { get; }
        Uri Location(Scope scope);
        String Title(Scope scope);
        object ExecuteScript(string javascript, Scope scope, params object[] args);

        [Obsolete("Please use instead: _browserSession.Driver.Cookies.GetAll()")]
        IEnumerable<Cookie> GetBrowserCookies();

        [Obsolete("Please use instead: _browserSession.Driver.Cookies.DeleteAll()")]
        void ClearBrowserCookies();

        void Click(Element element);
        void AcceptModalDialog(Scope scope);
        void CancelModalDialog(Scope scope);
        bool HasDialog(string withText, Scope scope);
        void Choose(Element field);
        void Check(Element field);
        void Uncheck(Element field);
        void Set(Element element, string value);
        void Visit(string url, Scope scope);
        void GoBack(Scope scope);
        void GoForward(Scope scope);
        void Hover(Element element);
        void MaximiseWindow(Scope scope);
        void Refresh(Scope scope);
        void ResizeTo(Size size, Scope Scope);
        void SaveScreenshot(string fileName, Scope scope);
        void SendKeys(Element element, string keys);
    }
}