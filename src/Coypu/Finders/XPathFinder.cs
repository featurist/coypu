using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class XPathFinder : XPathQueryFinder
    {
        private readonly string text;
        private readonly Regex textPattern;

        protected string SelectorType => "xpath";

        public XPathFinder(IDriver driver, string locator, DriverScope scope, Options options)
            : base(driver, locator, scope, options)
        {
        }

        public XPathFinder(IDriver driver, string locator, DriverScope scope, Options options, Regex textPattern)
            : base(driver, locator, scope, options)
        {
            this.textPattern = textPattern;
        }

        public XPathFinder(IDriver driver, string locator, DriverScope scope, Options options, string text)
            : base(driver, locator, scope, options)
        {
            this.text = text;
        }

        public override bool SupportsSubstringTextMatching => true;

        internal override string QueryDescription
        {
            get
            {
                var queryDesciption = SelectorType + ": " + Locator;
                if (text != null)
                    queryDesciption += " with text " + text;
                if (textPattern != null)
                    queryDesciption += " with text matching /" + (text ?? textPattern.ToString()) + "/";

                return queryDesciption;
            }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return ((locator, options) =>
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        return Locator;
                    }
                    else
                    {
                        return Locator + XPath.Where(html.IsText(text, options));
                    }
                });
        }
    }
}
