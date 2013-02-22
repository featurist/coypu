using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coypu.Drivers
{
    /// <summary>
    /// The browser that will be used by your chosen driver
    /// </summary>
    public class Browser
    {
        private Browser(){}

        public bool Javascript { get; private set; }

        public static Browser Firefox                = new Browser { Javascript = true };
        public static Browser InternetExplorer       = new Browser { Javascript = true };
        public static Browser Chrome                 = new Browser { Javascript = true };
        public static Browser Safari                 = new Browser { Javascript = true };
        public static Browser Android                = new Browser { Javascript = true };
        public static Browser HtmlUnit               = new Browser { Javascript = true };
        public static Browser HtmlUnitWithJavaScript = new Browser { Javascript = false };
        public static Browser PhantomJS              = new Browser { Javascript = true };

        public static Browser Parse(string browserName)
        {
            var fieldInfo = BrowserFields().FirstOrDefault(f => f.Name.Equals(browserName, StringComparison.InvariantCultureIgnoreCase));
            if (fieldInfo == null)
                throw new NoSuchBrowserException(browserName);

            return (Browser) fieldInfo.GetValue(null);
        }

        private static IEnumerable<FieldInfo> BrowserFields()
        {
            return typeof(Browser).GetFields(BindingFlags.Public | BindingFlags.Static);
        }
    }
}