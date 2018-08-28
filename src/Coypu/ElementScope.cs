using System;
using System.Linq;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu {
    public abstract class ElementScope : DriverScope, Element
    {
        protected internal ElementScope() : base()
        {

        }

        internal ElementScope(ElementFinder elementFinder, DriverScope outerScope)
            : base(elementFinder, outerScope)
        {
            
        }
        
        internal abstract void Try(DriverAction action);
        internal abstract bool Try(Query<bool> query);
        internal abstract T Try<T>(Func<T> getAttribute);

        public string Id
        {
            get { return  Try(() => Now().Id); }
        }

        public string Text
        {
            get { return Try(() => Now().Text); }
        }

        public string Value
        {
            get { return Try(() => Now().Value); }
        }

        public string Name
        {
            get { return Try(() => Now().Name); }
        }

        public string OuterHTML
        {
            get { return Try(() => Now().OuterHTML); }
        }

        public string InnerHTML
        {
            get { return Try(() => Now().InnerHTML); }
        }

        public string Title
        {
            get { return Try(() => Now().Title); }
        }

        public string SelectedOption
        {
            get { return Try(() => Now().SelectedOption); }
        }

        public bool Selected
        {
            get { return Try(() => Now().Selected); }
        }

        public object Native
        {
            get { return Try(() => Now().Native); }
        }

        public bool Disabled
        {
            get
            {
                return Try(() => Now().Disabled);
            }
        }

        public string this[string attributeName]
        {
            get { return Try(() => Now()[attributeName]); }
        }

        public ElementScope Click(Options options = null) 
        {
            Try(new ClickAction(this, driver, Merge(options)));
            return this;
        }

        public void ClickWait(Options options = null)
        {
            if (options == null)
                options = new Options() { Timeout = TimeSpan.FromSeconds(60) };
            var opts = Merge(options);

            var html = FindAllXPath("//html").First();
            Click(opts);
            
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            while (stopWatch.Elapsed < opts.Timeout && !html.StaleSelenium)
            {
                System.Threading.Thread.Sleep(opts.RetryInterval);
            }
        }

        /// <summary>
        /// Treat this scope as an input field and fill in with the specified value
        /// </summary>
        /// <param name="value">The value to fill in with</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// <returns>The current scope</returns>
        public virtual ElementScope FillInWith(string value, Options options = null) 
        {
            Try(new FillIn(driver, this, value, Merge(options)));
            return this;
        }

        /// <summary>
        /// Treat this scope as a select element and choose the specified option
        /// </summary>
        /// <param name="value">The text or value of the option</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait</para>
        /// <returns>The current scope</returns>
        public ElementScope SelectOption(string value, Options options = null)
        {
            if (value == null)
                return this;
            options = Merge(options);

            if (options.SplitSelectValue)
                return SelectOption(value.Split(';'), options);
            else
                Try(new Select(driver, this, value, DisambiguationStrategy, options));
            return this;
        }

        /// <summary>
        /// Treat this scope as a select element and choose the specified options
        /// </summary>
        /// <param name="value">List of texts or values of the option</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait</para>
        /// <returns>The current scope</returns>
        public ElementScope SelectOption(string[] values, Options options = null)
        {
            if (values == null)
                return this;

            foreach (var value in values)
                Try(new Select(driver, this, value, DisambiguationStrategy, Merge(options)));

            return this;
        }

        public ElementScope Hover(Options options = null)
        {
            Try(new Hover(this, driver, Merge(options)));
            return this;
        }

        public ElementScope SendKeys(string keys, Options options = null)
        {
            Try(new SendKeys(keys, this, driver, Merge(options)));
            return this;
        }

        public ElementScope Check(Options options = null)
        {
            Try(new CheckAction(driver, this, Merge(options)));
            return this;
        }

        public ElementScope Uncheck(Options options = null) {
            Try(new Uncheck(driver, this, Merge(options)));
            return this;
        }

        public abstract bool Exists(Options options = null);

        public abstract bool Missing(Options options = null);

        public bool HasValue(string text, Options options = null)
        {
            return Try(new HasValueQuery(this, text, Merge(options)));
        }

        public bool HasNoValue(string text, Options options = null)
        {
            return Try(new HasNoValueQuery(this, text, Merge(options)));
        }
    }
}