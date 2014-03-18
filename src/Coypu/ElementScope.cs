using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu {
    public abstract class ElementScope : DriverScope, Element
    {
        internal ElementScope(ElementFinder elementFinder, DriverScope outerScope)
            : base(elementFinder, outerScope)
        {
        }

        internal abstract void Try(DriverAction action);
        internal abstract bool Try(Query<bool> query);

        public string Id
        {
            get { return Now().Id; }
        }

        public string Text
        {
            get { return Now().Text; }
        }

        public string Value
        {
            get { return Now().Value; }
        }

        public string Name
        {
            get { return Now().Name; }
        }

        public string OuterHTML
        {
            get { return Now().OuterHTML; }
        }

        public string InnerHTML
        {
            get { return Now().InnerHTML; }
        }

        public string Title
        {
            get { return Now().Title; }
        }

        public string SelectedOption
        {
            get { return Now().SelectedOption; }
        }

        public bool Selected
        {
            get { return Now().Selected; }
        }

        public object Native
        {
            get { return Now().Native; }
        }

        public string this[string attributeName]
        {
            get {

                ElementFound elementFound = Now();
                return elementFound[attributeName];
            }
        }

        public ElementScope Click(Options options = null) 
        {
            Try(new ClickAction(this, driver, Merge(options)));
            return this;
        }

        /// <summary>
        /// Treat this scope as an input field and fill in with the specified value
        /// </summary>
        /// <param name="value">The value to fill in with</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// <returns>The current scope</returns>
        public ElementScope FillInWith(string value, Options options = null) 
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

        public bool Exists()
        {
            return Try(new ElementExistsQuery(this));
        }

        public bool Missing()
        {
            return Try(new ElementMissingQuery(this));
        }

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