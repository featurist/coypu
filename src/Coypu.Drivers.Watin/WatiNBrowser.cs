using System;

namespace Coypu.Drivers.Watin
{
    internal class WatiNBrowser : ElementFound
    {
        private readonly WatiN.Core.Browser browser;

        public WatiNBrowser(WatiN.Core.Browser browser)
        {
            this.browser = browser;
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Text
        {
            get { throw new NotImplementedException(); }
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
            get { return browser; }
        }

        public bool Stale
        {
            get { return false; }
        }

        public string this[string attributeName]
        {
            get { throw new NotImplementedException(); }
        }
    }
}