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
    }
}