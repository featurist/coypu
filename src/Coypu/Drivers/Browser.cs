﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#pragma warning disable 1591

namespace Coypu.Drivers
{
    /// <summary>
    ///     The browser that will be used by your chosen driver
    /// </summary>
    public class Browser
    {
        public static Browser Firefox = new Browser
                                        {
                                            Javascript = true,
                                            UppercaseTagNames = true
                                        };

        public static Browser InternetExplorer = new Browser {Javascript = true, Name = "InternetExplorer"};
        public static Browser Chrome = new Browser {Javascript = true, Name = "Chrome"};
        public static Browser Chromium = new Browser {Javascript = true, Name = "Chromium"};
        public static Browser Edge = new Browser {Javascript = true, Name = "Edge"};
        public static Browser Opera = new Browser {Javascript = true, Name = "Opera"};
        public static Browser Safari = new Browser {Javascript = true, Name = "Safari"};
        public static Browser Webkit = new Browser {Javascript = true, Name = "Webkit"};
        private Browser() { }

        public bool Javascript { get; private set; }

        public string Name { get; private set; }
        public bool UppercaseTagNames { get; private set; }

        public static Browser Parse(string browserName)
        {
            var fieldInfo = BrowserFields()
                .FirstOrDefault(f => f.Name.Equals(browserName.Replace(" ", ""),
                                                   StringComparison.InvariantCultureIgnoreCase));
            if (fieldInfo == null)
                throw new NoSuchBrowserException(browserName);
            return (Browser) fieldInfo.GetValue(null);
        }

        public override string ToString() => Name;

        private static IEnumerable<FieldInfo> BrowserFields()
        {
            return typeof(Browser).GetFields(BindingFlags.Public | BindingFlags.Static);
        }

    }
}
