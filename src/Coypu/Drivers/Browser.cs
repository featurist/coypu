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
        public string   Name                { private get; private set; }

        public static Browser Parse(string browserName)
        {
            var match = Browsers().FirstOrDefault(b => b.Name.Equals(browserName.Replace(" ", String.Empty), StringComparison.InvariantCultureIgnoreCase));
            if (match == null)
                throw new NoSuchBrowserException(browserName);            
                
            return match;
        }

        private static IEnumerable<Browser> Browsers()
        {   
            yield return new Browser { Name = "Firefox",                 Javascript = true, UppercaseTagNames = true };
            yield return new Browser { Name = "InternetExplorer",        Javascript = true };
            yield return new Browser { Name = "Chrome",                  Javascript = true };
            yield return new Browser { Name = "Safari",                  Javascript = true };
            yield return new Browser { Name = "HtmlUnit",                Javascript = false };
            yield return new Browser { Name = "HtmlUnitWithJavaScript",  Javascript = true };
            yield return new Browser { Name = "PhantomJS",               Javascript = true };            
        }
    }
}
