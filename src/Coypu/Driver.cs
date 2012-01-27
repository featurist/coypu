using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Driver : IDisposable
    {
        Element FindButton(string locator, DriverScope scope);
        Element FindLink(string linkText, DriverScope scope);
        Element FindField(string locator, DriverScope scope);
        void Click(Element element);
        void Visit(string url);
        void Set(Element element, string value);
        void Select(Element element, string option);
        object Native { get; }
        bool HasContent(string text, DriverScope scope);
        bool HasContentMatch(Regex pattern, DriverScope scope);
        bool HasCss(string cssSelector, DriverScope scope);
        bool HasXPath(string xpath, DriverScope scope);
        bool HasDialog(string withText);
        Element FindCss(string cssSelector, DriverScope scope);
        Element FindXPath(string xpath, DriverScope scope);
        IEnumerable<Element> FindAllCss(string cssSelector, DriverScope scope);
        IEnumerable<Element> FindAllXPath(string xpath, DriverScope scope);
        void Check(Element field);
        void Uncheck(Element field);
        void Choose(Element field);
        bool Disposed { get; }
        Uri Location { get; }
        bool ConsiderInvisibleElements { get; set; }
        void AcceptModalDialog();
        void CancelModalDialog();
        string ExecuteScript(string javascript);
        Element FindFieldset(string locato, DriverScope scoper);
        Element FindSection(string locator, DriverScope scope);
        Element FindId(string id, DriverScope scope);
        Element FindIFrame(string locator, DriverScope scope);
        void Hover(Element element);
        IEnumerable<Cookie> GetBrowserCookies();
    }
}