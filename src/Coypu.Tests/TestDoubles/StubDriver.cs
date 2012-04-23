using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Coypu.Drivers;

namespace Coypu.Tests.TestDoubles
{
    public class StubDriver : Driver
    {
        public StubDriver() {}

        public StubDriver(Drivers.Browser browser){}

        public void Dispose()
        {
        }

        public ElementFound FindButton(string locator,DriverScope scope)
        {
            return null;
        }

        public ElementFound FindLink(string linkText, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindField(string locator, DriverScope scope)
        {
            return null;
        }

        public void Click(Element element)
        {
        }

        public void Visit(string url)
        {
        }

        public void Set(Element element, string value, bool forceAllEvents)
        {
        }

        public void Select(Element element, string option)
        {
        }

        public object Native
        {
            get { return "Native driver on stub driver"; }
        }

        public bool HasContent(string text, DriverScope scope)
        {
            return false;
        }

        public bool HasContentMatch(Regex pattern, DriverScope scope)
        {
            return false;
        }

        public bool HasCss(string cssSelector, DriverScope scope)
        {
            return false;
        }

        public bool HasXPath(string xpath, DriverScope scope)
        {
            return false;
        }

        public bool HasDialog(string withText, DriverScope scope)
        {
            return false;
        }

        public ElementFound FindCss(string cssSelector, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindXPath(string xpath, DriverScope scope)
        {
            return null;
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, DriverScope scope)
        {
            return Enumerable.Empty<ElementFound>();
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, DriverScope scope)
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

        public Uri Location
        {
            get { return null; }
        }

        public ElementFound Window
        {
            get { return null; }
        }

        public void AcceptModalDialog(DriverScope scope)
        {
        }

        public void CancelModalDialog(DriverScope scope)
        {
        }

        public void SetScope(ElementFound findScope)
        {
            
        }

        public void ClearScope()
        {
            
        }

        public string ExecuteScript(string javascript, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindFieldset(string locator, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindSection(string locator, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindId(string id, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindIFrame(string locator, DriverScope scope)
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

        public ElementFound FindWindow(string locator, DriverScope scope)
        {
            return null;
        }

        public ElementFound FindFrame(string locator, DriverScope root)
        {
            return null;
        }

        public void SetBrowserCookies(Cookie cookie)
        {
        }
    }
}