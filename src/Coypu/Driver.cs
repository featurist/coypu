using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Driver : IDisposable
    {
        Element FindButton(string locator);
        Element FindLink(string locator);
        Element FindField(string locator);
        void Click(Element element);
        void Visit(string url);
        void Set(Element element, string value);
        void Select(Element element, string option);
        object Native { get; }
        bool HasContent(string text);
        bool HasContentMatch(Regex pattern);
        bool HasCss(string cssSelector);
        bool HasXPath(string xpath);
        bool HasDialog(string withText);
        Element FindCss(string cssSelector);
        Element FindXPath(string xpath);
        IEnumerable<Element> FindAllCss(string cssSelector);
        IEnumerable<Element> FindAllXPath(string xpath);
        void Check(Element field);
        void Uncheck(Element field);
        void Choose(Element field);
        bool Disposed { get; }
        void AcceptModalDialog();
        void CancelModalDialog();
        void SetScope(Func<Element> find);
        void ClearScope();
        string ExecuteScript(string javascript);
        Element FindFieldset(string locator);
        Element FindSection(string locator);
        Element FindId(string id);
        Element FindIFrame(string locator);
        void Hover(Element element);
    }
}