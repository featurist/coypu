using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
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

        ReturnType ExecuteScript<ReturnType>(string javascript, Scope scope, params object[] args);

        [Obsolete("Please use instead: _browserSession.Driver.Cookies.GetAll()")]
        IEnumerable<Cookie> GetBrowserCookies();

        [Obsolete("Please use instead: _browserSession.Driver.Cookies.DeleteAll()")]
        void ClearBrowserCookies();
        void Click(Element element);
        [Obsolete("Please use instead: AcceptAlert/AcceptConfirm/AcceptPrompt")]
        void AcceptModalDialog(Scope scope);
        [Obsolete("Please use instead: CancelAlert/CancelConfirm/CancelPrompt")]
        void CancelModalDialog(Scope scope);
        [Obsolete("Please use instead: [Accepts/Cancels][Alert/Confirm/Prompt]")]
        bool HasDialog(string withText, Scope scope);
        void AcceptAlert(string text, DriverScope root, Action trigger);
        void AcceptConfirm(string text, DriverScope root, Action trigger);
        void CancelConfirm(string text, DriverScope root, Action trigger);
        void AcceptPrompt(string text, string value, DriverScope root, Action trigger);
        void CancelPrompt(string text, DriverScope root, Action trigger);
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
        void SelectOption(Element select, Element option, string optionToSelect);
  }
}
