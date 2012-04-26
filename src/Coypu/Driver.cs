using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Driver : IDisposable
    {
        ElementFound FindButton(string locator, DriverScope scope);
        ElementFound FindLink(string linkText, DriverScope scope);
        ElementFound FindField(string locator, DriverScope scope);
        void Click(Element element);
        void Visit(string url);
        void Set(Element element, string value, bool forceAllEvents);
        void Select(Element element, string option);
        object Native { get; }
        bool HasContent(string text, DriverScope scope);
        bool HasContentMatch(Regex pattern, DriverScope scope);
        bool HasCss(string cssSelector, DriverScope scope);
        bool HasXPath(string xpath, DriverScope scope);
        bool HasDialog(string withText, DriverScope scope);
        ElementFound FindCss(string cssSelector, DriverScope scope);
        ElementFound FindXPath(string xpath, DriverScope scope);
        IEnumerable<ElementFound> FindAllCss(string cssSelector, DriverScope scope);
        IEnumerable<ElementFound> FindAllXPath(string xpath, DriverScope scope);
        void Check(Element field);
        void Uncheck(Element field);
        void Choose(Element field);
        bool Disposed { get; }
        String Title(DriverScope scope);
        Uri Location(DriverScope scope);
        ElementFound Window { get; }
        void AcceptModalDialog(DriverScope scope);
        void CancelModalDialog(DriverScope scope);
        string ExecuteScript(string javascript, DriverScope scope);
        ElementFound FindFieldset(string locator, DriverScope scoper);
        ElementFound FindSection(string locator, DriverScope scope);
        ElementFound FindId(string id, DriverScope scope);
        void Hover(Element element);
        IEnumerable<Cookie> GetBrowserCookies();
        ElementFound FindWindow(string locator, DriverScope scope);
        ElementFound FindFrame(string locator, DriverScope scope);
    }
}