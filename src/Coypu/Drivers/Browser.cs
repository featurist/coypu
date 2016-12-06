using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Drivers
{
    /// <summary>
    /// The browser that will be used by your chosen driver
    /// </summary>
    public class Browser
    {
        private Browser(){}

        public bool     Javascript          { get; private set; }
        public bool     UppercaseTagNames   { get; private set; }
        private string   _name;

        public static Browser Parse(string browserName)
        {
            var match = Browsers().FirstOrDefault(b => b._name.Equals(browserName.Replace(" ", String.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (match == null)
                throw new NoSuchBrowserException(browserName);            
                
            return match;
        }

        private static IEnumerable<Browser> Browsers()
        {   
            yield return new Browser { _name = "Firefox",                 Javascript = true, UppercaseTagNames = true };
            yield return new Browser { _name = "InternetExplorer",        Javascript = true };
            yield return new Browser { _name = "Chrome",                  Javascript = true };
            yield return new Browser { _name = "Safari",                  Javascript = true };
            yield return new Browser { _name = "HtmlUnit",                Javascript = false };
            yield return new Browser { _name = "HtmlUnitWithJavaScript",  Javascript = true };
            yield return new Browser { _name = "PhantomJS",               Javascript = true };            
        }
    }
}
