using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu {
    public abstract class DeferredElementScope : DriverScope, ElementScope
    {
        internal DeferredElementScope(ElementFinder elementFinder, DriverScope outer)
            : base(elementFinder, outer)
        {
        }

        internal abstract void Try(DriverAction action);
        internal abstract bool Try(Query<bool> query);

        public string Id
        {
            get { return Find().Id; }
        }

        public string Text
        {
            get { return Find().Text; }
        }

        public string Value
        {
            get { return Find().Value; }
        }

        public string Name
        {
            get { return Find().Name; }
        }

        public string OuterHTML
        {
            get { return Find().OuterHTML; }
        }

        public string InnerHTML
        {
            get { return Find().InnerHTML; }
        }

        public string Title
        {
            get { return Find().Title; }
        }

        public string SelectedOption
        {
            get { return Find().SelectedOption; }
        }

        public bool Selected
        {
            get { return Find().Selected; }
        }

        public object Native
        {
            get { return Find().Native; }
        }

        public string this[string attributeName]
        {
            get {

                ElementFound elementFound = Find();
                return elementFound[attributeName];
            }
        }

        public ElementScope Click(Options options = null) 
        {
            Try(new ClickAction(this, driver, SetOptions(options)));
            return this;
        }

        public ElementScope FillInWith(string value, Options options = null) 
        {
            Try(new FillIn(driver, this, value, SetOptions(options)));
            return this;
        }

        public ElementScope Hover(Options options = null)
        {
            Try(new Hover(this, driver, SetOptions(options)));
            return this;
        }

        public ElementScope SendKeys(string keys, Options options = null)
        {
            Try(new SendKeys(keys, this, driver, SetOptions(options)));
            return this;
        }

        public ElementScope Check(Options options = null)
        {
            Try(new Check(driver, this, SetOptions(options)));
            return this;
        }

        public ElementScope Uncheck(Options options = null) {
            Try(new Uncheck(driver, this, SetOptions(options)));
            return this;
        }

        public bool Exists(Options options = null)
        {
            return Try(new ElementExistsQuery(this, SetOptions(options)));
        }

        public bool Missing(Options options = null)
        {
            return Try(new ElementMissingQuery(this, SetOptions(options)));
        }

        public bool HasValue(string text, Options options = null)
        {
            return Try(new HasValueQuery(this, text, SetOptions(options)));
        }

        public bool HasNoValue(string text, Options options = null)
        {
            return Try(new HasNoValueQuery(this, text, SetOptions(options)));
        }
    }
}