using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Driver : IDisposable
    {
        IEnumerable<ElementFound> FindLinks(string linkText, Scope scope, bool exact);
        void Click(Element element);
        void Visit(string url, Scope scope);
        void Set(Element element, string value);
        void Select(Element element, string option);
        object Native { get; }
        bool HasDialog(string withText, Scope scope);
        IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope, Regex textPattern = null);
        IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope);
        void Check(Element field);
        void Uncheck(Element field);
        void Choose(Element field);
        bool Disposed { get; }
        Uri Location(Scope scope);
        String Title(Scope scope);
        ElementFound Window { get; }
        void AcceptModalDialog(Scope scope);
        void CancelModalDialog(Scope scope);
        string ExecuteScript(string javascript, Scope scope);
        IEnumerable<ElementFound> FindId(string id, Scope scope);
        void Hover(Element element);
        IEnumerable<Cookie> GetBrowserCookies();
        IEnumerable<ElementFound> FindWindows(string locator, Scope scope, bool exact);
        IEnumerable<ElementFound> FindFrames(string locator, Scope scope, bool exact);
        void SendKeys(Element element, string keys);
        void MaximiseWindow(Scope scope);
        void Refresh(Scope scope);
        void ResizeTo(Size size, Scope Scope);
        void SaveScreenshot(string fileName, Scope scope);
        void GoBack(Scope scope);
        void GoForward(Scope scope);
    }
}