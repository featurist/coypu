using System;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    internal class WatiNBrowser : ElementFound
    {
        private readonly WatiN.Core.Browser browser;
        private readonly Constraint windowHandle;

        public WatiNBrowser(WatiN.Core.Browser browser)
        {
            this.browser = browser;
        }

        public WatiNBrowser (Constraint windowHandle)
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