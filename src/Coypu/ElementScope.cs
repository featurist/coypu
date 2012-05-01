using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu
{
    public interface ElementScope : Scope, Element
    {
        bool Exists(Options options = null);
        bool Missing(Options options = null);
        ElementScope Click(Options options = null);
        Scope Hover(Options options = null);
    }

    public class DeferredElementScope : DriverScope, ElementScope
    {
        internal DeferredElementScope(ElementFinder elementFinder, DriverScope outer)
            : base(elementFinder, outer)
        {
        }

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
            get { return Now()[attributeName]; }
        }

        public ElementScope Click(Options options = null)
        {
            RetryUntilTimeout(new Click(this, driver, SetOptions(options)));
            return this;
        }

        public Scope Hover(Options options = null)
        {
            RetryUntilTimeout(new Hover(this, driver, SetOptions(options)));
            return this;
        }

        public bool Exists(Options options = null)
        {
            return robustWrapper.Robustly(new ElementExistsQuery(this, SetOptions(options)));
        }

        public bool Missing(Options options = null)
        {
            return robustWrapper.Robustly(new ElementMissingQuery(this, SetOptions(options)));
        }
    }
}