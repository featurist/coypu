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

        public Element FindButton(string locator)
        {
            return null;
        }

        public Element FindLink(string linkText)
        {
            return null;
        }

        public Element FindField(string locator)
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

        public bool HasContent(string text)
        {
            return false;
        }
        
        public bool HasContentMatch(Regex pattern)
        {
            return false;
        }

        public bool HasCss(string cssSelector)
        {
            return false;
        }

        public bool HasXPath(string xpath)
        {
            return false;
        }

        public bool HasDialog(string withText)
        {
            return false;
        }

        public Element FindCss(string cssSelector)
        {
            return null;
        }

        public Element FindXPath(string xpath)
        {
            return null;
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return Enumerable.Empty<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
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

        public bool ConsiderInvisibleElements
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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

        public Element FindFieldset(string locator)
        {
            return null;
        }

        public Element FindSection(string locator)
        {
            return null;
        }

        public Element FindId(string id)
        {
            return null;
        }

        public Element FindIFrame(string locator)
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