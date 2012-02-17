using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu.Tests.TestDoubles
{
    public class StubDriver : Driver
    {
        public void Dispose()
        {
        }

        public Element FindButton(string locator,DriverScope scope)
        {
            return null;
        }

        public Element FindLink(string linkText, DriverScope scope)
        {
            return null;
        }

        public Element FindField(string locator, DriverScope scope)
        {
            return null;
        }

        public void Click(Element element)
        {
        }

        public void Visit(string url)
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

        public bool HasDialog(string withText)
        {
            return false;
        }

        public Element FindCss(string cssSelector, DriverScope scope)
        {
            return null;
        }

        public Element FindXPath(string xpath, DriverScope scope)
        {
            return null;
        }

        public IEnumerable<Element> FindAllCss(string cssSelector, DriverScope scope)
        {
            return Enumerable.Empty<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath, DriverScope scope)
        {
            return Enumerable.Empty<Element>();
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

        public Element Window
        {
            get { return null; }
        }

        public void AcceptModalDialog()
        {
            
        }

        public void CancelModalDialog()
        {
            
        }

        public void SetScope(Element findScope)
        {
            
        }

        public void ClearScope()
        {
            
        }

        public string ExecuteScript(string javascript)
        {
            return null;
        }

        public Element FindFieldset(string locator, DriverScope scope)
        {
            return null;
        }

        public Element FindSection(string locator, DriverScope scope)
        {
            return null;
        }

        public Element FindId(string id, DriverScope scope)
        {
            return null;
        }

        public Element FindIFrame(string locator, DriverScope scope)
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

        public void SetBrowserCookies(Cookie cookie)
        {
            
        }
    }
}