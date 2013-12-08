using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu {
    public abstract class DeferredElementScope : DriverScope, ElementScope
    {
        internal DeferredElementScope(ElementFinder elementFinder, DriverScope outerScope)
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
            Try(new ClickAction(this, driver, MergeWithSession(options)));
            return this;
        }

        public ElementScope FillInWith(string value, Options options = null) 
        {
            Try(new FillIn(driver, this, value, MergeWithSession(options)));
            return this;
        }

        public ElementScope Hover(Options options = null)
        {
            Try(new Hover(this, driver, MergeWithSession(options)));
            return this;
        }

        public ElementScope SendKeys(string keys, Options options = null)
        {
            Try(new SendKeys(keys, this, driver, MergeWithSession(options)));
            return this;
        }

        public ElementScope Check(Options options = null)
        {
            Try(new Check(driver, this, MergeWithSession(options)));
            return this;
        }

        public ElementScope Uncheck(Options options = null) {
            Try(new Uncheck(driver, this, MergeWithSession(options)));
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
            return Try(new HasValueQuery(this, text, MergeWithFinder(options)));
        }

        public bool HasNoValue(string text, Options options = null)
        {
            return Try(new HasNoValueQuery(this, text, MergeWithFinder(options)));
        }
    }
}