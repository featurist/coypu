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

        public void Click(Element element)
        {
        }

        public void Visit(string url, Scope scope)
        {
        }

        public void Set(Element element, string value)
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

        public bool HasDialog(string withText, Scope scope)
        {
            return false;
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope, Options options, Regex textPattern = null)
        {
            return Enumerable.Empty<ElementFound>();
        }


        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope, Options options)
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

        public object ExecuteScript(string javascript, Scope scope)
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

        public IEnumerable<ElementFound> FindWindows(string locator, Scope scope, Options options)
        {
            return Enumerable.Empty<ElementFound>();
        }

        public IEnumerable<ElementFound> FindFrames(string locator, Scope scope, Options options)
        {
            return Enumerable.Empty<ElementFound>();
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