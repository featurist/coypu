using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    public class SeleniumScoping
    {
        private readonly RemoteWebDriver selenium;
        private Func<Element> findScope;
        private bool findingScope;
        private ISearchContext cachedScope;

        public SeleniumScoping(RemoteWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public void SetScope(Func<Element> find)
        {
            findScope = find;
        }

        public void ClearScope()
        {
            findScope = null;
            cachedScope = null;
        }

        public bool ScopeDefined()
        {
            return findScope != null;
        }

        public ISearchContext Scope
        {
            get
            {
                return findScope == null || findingScope
                           ? selenium
                           : FindScope();
            }
        }

        private ISearchContext FindScope()
        {
            findingScope = true;
            try
            {
                return cachedScope == null
                           ? (cachedScope = FindFreshScope())
                           : (cachedScope = CheckForStaleElement());
            }
            finally
            {
                findingScope = false;
            }
        }

        private ISearchContext FindFreshScope()
        {
            var findFreshScope = (IWebElement) findScope().Native;
            if (findFreshScope.TagName == "iframe")
            {
                selenium.SwitchTo().Frame(findFreshScope);
                return selenium;
            }

            return findFreshScope;
        }

        private ISearchContext CheckForStaleElement()
        {
            try
            {
                cachedScope.FindElement(By.XPath("."));
                return cachedScope;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("Stale Element: " + cachedScope);
                return FindFreshScope();
            }
        }
    }
}