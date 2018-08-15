using System;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu
{
    /// <summary>
    /// The scope of an element already found in the document, therefore not deferred. 
    /// 
    /// If this element becomes stale then using this scope will not try to refind the element but 
    /// will raise a MissingHtmlException immediately.
    /// </summary>
    public class SnapshotElementScope : ElementScope
    {
        private readonly Element element;
        private readonly Options options;

        internal SnapshotElementScope(Element element, DriverScope scope, Options options)
            : base(null, scope)
        {
            this.element = element;
            this.options = options;
        }

        internal override bool Stale
        {
            get { return true; }
            set {}
        }

        public bool StaleSelenium
        {
            get
            {
                var e = (OpenQA.Selenium.IWebElement) element.Native;
                try
                {
                    return e == null || !e.Enabled;     // Calling any method forces a staleness check
                }
                catch (OpenQA.Selenium.StaleElementReferenceException)
                {
                    return true;
                }
            }
        }

        protected internal override Element FindElement()
        {
            return element;
        }

        internal override void Try(DriverAction action)
        {
            action.Act();
        }

        internal override bool Try(Query<bool> query)
        {
            return query.Run();
        }

        internal override T Try<T>(Func<T> getAttribute)
        {
            return getAttribute();
        }

        public override bool Exists(Options options = null)
        {
            return FindXPath(".", options).Exists();
        }

        public override bool Missing(Options options = null)
        {
            return FindXPath(".", options).Missing();
        }
    }
}