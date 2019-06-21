using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Coypu.Drivers.Selenium
{
    internal class FrameFinder
    {
        private readonly ElementFinder _elementFinder;
        private readonly IWebDriver _selenium;
        private readonly SeleniumWindowManager _seleniumWindowManager;
        private readonly XPath _xPath;

        public FrameFinder(IWebDriver selenium,
                           ElementFinder elementFinder,
                           XPath xPath,
                           SeleniumWindowManager seleniumWindowManager)
        {
            _selenium = selenium;
            _elementFinder = elementFinder;
            _xPath = xPath;
            _seleniumWindowManager = seleniumWindowManager;
        }

        public IEnumerable<IWebElement> FindFrame(string locator,
                                                  Scope scope,
                                                  Options options)
        {
            var frames = AllElementsByTag(scope, "iframe", options)
                .Union(AllElementsByTag(scope, "frame", options));

            return WebElement(locator, frames, options);
        }

        private IEnumerable<IWebElement> AllElementsByTag(Scope scope,
                                                          string tagNameToFind,
                                                          Options options)
        {
            return _elementFinder.FindAll(By.TagName(tagNameToFind), scope, options);
        }

        private IEnumerable<IWebElement> WebElement(string locator,
                                                    IEnumerable<IWebElement> webElements,
                                                    Options options)
        {
            return webElements.Where(e => e.GetAttribute("id") == locator ||
                                          e.GetAttribute("name") == locator ||
                                          (options.TextPrecisionExact
                                               ? e.GetAttribute("title") == locator
                                               : e.GetAttribute("title")
                                                  .Contains(locator)) ||
                                          FrameContentsMatch(e, locator, options));
        }

        private bool FrameContentsMatch(IWebElement e,
                                        string locator,
                                        Options options)
        {
            var currentHandle = _selenium.CurrentWindowHandle;
            try
            {
                var frame = _seleniumWindowManager.SwitchToFrame(e);
                return
                    frame.Title == locator ||
                    frame.FindElements(By.XPath($".//h1[{_xPath.IsText(locator, options)}]"))
                         .Any();
            }
            finally
            {
                _selenium.SwitchTo()
                         .Window(currentHandle);

                // Fix for https://bugzilla.mozilla.org/show_bug.cgi?id=1305822 
                if (_selenium is FirefoxDriver)
                {
                    _selenium.SwitchTo()
                        .DefaultContent();
                }
            }
        }
    }
}