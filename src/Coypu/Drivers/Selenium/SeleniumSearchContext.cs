using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumSearchContext : ElementFound
    {
        private readonly ISearchContext searchContext;

        public SeleniumSearchContext(ISearchContext searchContext)
        {
            this.searchContext = searchContext;
        }

        public string Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Text
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Value
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public string SelectedOption
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Selected
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Native
        {
            get { return searchContext; }
        }

        public bool Stale
        {
            get { return false; }
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}