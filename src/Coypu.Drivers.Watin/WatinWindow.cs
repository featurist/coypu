using System;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    internal class WatiNWindow : ElementFound
    {
        private readonly WatiN.Core.Browser browser;
        private readonly Constraint windowHandle;

        public WatiNWindow(WatiN.Core.Browser browser)
        {
            this.browser = browser;
        }

        public WatiNWindow (Constraint windowHandle)
        {
            this.windowHandle = windowHandle;
            try
            {
                if (!WatiN.Core.Browser.Exists<IE>(windowHandle))
                    throw new MissingWindowException("No such window found: " + windowHandle);
                
                browser = WatiN.Core.Browser.AttachTo<IE>(windowHandle);
            }
            catch (WatiN.Core.Exceptions.BrowserNotFoundException)
            {
                throw new MissingWindowException("No such window found: " + windowHandle);
            }
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
                
        public string Text
        {
            get
            {
                return browser.Text;
            }
        }

        public string InnerHTML
        {
            get
            {
                return browser.XPath("./*").First().InnerHtml;
            }
        }

        public string OuterHTML
        {
            get
            {
                return browser.XPath("./*").First().OuterHtml;
            }
        }

        public string Value
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string SelectedOption
        {
            get { throw new NotImplementedException(); }
        }

        public bool Selected
        {
            get { throw new NotImplementedException(); }
        }

        public object Native
        {
            get
            {
                return browser;
            }
        }

        public bool Stale(Options options)
        {
            try
            {
                return !WatiN.Core.Browser.Exists<IE>(Find.By("hwnd", browser.hWnd.ToString()));
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return true;
            }
        }

        public string this[string attributeName]
        {
            get { throw new NotImplementedException(); }
        }
    }
}