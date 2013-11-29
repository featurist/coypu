using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu.Tests.TestDoubles
{
    public class StubDriver : Driver
    {
        public StubDriver() {}

        public StubDriver(Drivers.Browser browser){}

        public void Dispose()
        {
        }

        public ElementFound FindLink(string linkText, Scope scope)
        {
            return null;
        }

        public ElementFound FindField(string locator, Scope scope)
        {
            return null;
        }

        public void Click(Element element)
        {
        }

        public void Visit(string url, Scope scope)
        {
        }

        public void Set(Element element, string value)
        {
        }

        public void Select(Element element, string option)
        {
        }

        public object Native
        {
            get { return "Native driver on stub driver"; }
        }

        public bool HasContent(string text, Scope scope)
        {
            return false;
        }

        public bool HasContentMatch(Regex pattern, Scope scope)
        {
            return false;
        }

        public bool HasXPath(string xpath, Scope scope)
        {
            return false;
        }

        public bool HasDialog(string withText, Scope scope)
        {
            return false;
        }

        public ElementFound FindCss(string cssSelector, Scope scope, Regex textPattern = null)
        {
            return null;
        }

        public ElementFound FindXPath(string xpath, Scope scope)
        {
            return null;
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope)
        {
            return Enumerable.Empty<ElementFound>();
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope)
        {
            return Enumerable.Empty<ElementFound>();
        }

        public void Check(Element field)
        {
            
        }

        public void Uncheck(Element field)
        {
            
        }

        public void Choose(Element field)
        {
            
        }

        public bool Disposed
        {
            get { return false; }
        }

        Uri Driver.Location(Scope scope)
        {
            return null;
        }

        public string Title(Scope scope)
        {
            return null;
        }

        public ElementFound Window
        {
            get { return null; }
        }

        public void AcceptModalDialog(Scope scope)
        {
        }

        public void CancelModalDialog(Scope scope)
        {
        }

        public void SetScope(ElementFound findScope)
        {
            
        }

        public void ClearScope()
        {
            
        }

        public string ExecuteScript(string javascript, Scope scope)
        {
            return null;
        }

        public ElementFound FindFieldset(string locator, Scope scope)
        {
            return null;
        }

        public ElementFound FindSection(string locator, Scope scope)
        {
            return null;
        }

        public ElementFound FindId(string id, Scope scope)
        {
            return null;
        }

        public ElementFound FindIFrame(string locator, Scope scope)
        {
            return null;
        }

        public void Hover(Element element)
        {   
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return new List<Cookie>();
        }

        public ElementFound FindWindow(string locator, Scope scope)
        {
            return null;
        }

        public ElementFound FindFrame(string locator, Scope root)
        {
            return null;
        }

        public void SendKeys(Element element, string keys)
        {
        }

        public void MaximiseWindow(Scope scope)
        {
        }

        public void Refresh(Scope scope)
        {
        }

        public void ResizeTo(Size size, Scope Scope)
        {
        }

        public void SaveScreenshot(string fileName, Scope scope)
        {
        }

        public void GoBack(Scope scope)
        {
        }

        public void GoForward(Scope scope)
        {
        }

        public void SetBrowserCookies(Cookie cookie)
        {
        }
    }
}