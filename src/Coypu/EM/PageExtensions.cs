using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu
{
    public static class PageExtensions
    {
        public static T Visit<T>(this T page, BrowserSession session) where T : Page
        {
            return Page.Visit<T>(page, session);
        }
    }
}