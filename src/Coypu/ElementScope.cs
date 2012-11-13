using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu
{
    /// <summary>
    /// 
    /// </summary>
    public interface ElementScope : Scope, Element
    {
        bool Exists(Options options = null);
        bool Missing(Options options = null);
        ElementScope Click(Options options = null);
        ElementScope Hover(Options options = null);
        ElementScope SendKeys(string keys, Options options = null);
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

        public virtual ElementScope Click(Options options = null)
        {
            ClickAction(options).Act();
            return this;
        }

        internal Click ClickAction(Options options)
        {
            return new Click(this, driver, SetOptions(options));
        }

        public virtual ElementScope Hover(Options options = null)
        {
            HoverAction(options).Act();
            return this;
        }

        public virtual ElementScope SendKeys(string keys, Options options = null)
        {
            SendKeysAction(keys, options).Act();
            return this;
        }

        internal SendKeys SendKeysAction(string keys, Options options)
        {
            return new SendKeys(keys, this, driver, SetOptions(options));
        }

        internal Hover HoverAction(Options options)
        {
            return new Hover(this, driver, SetOptions(options));
        }

        public virtual bool Exists(Options options = null)
        {
            return ExistsQuery(options).Run();
        }

        internal ElementExistsQuery ExistsQuery(Options options)
        {
            return new ElementExistsQuery(this, SetOptions(options));
        }

        public virtual bool Missing(Options options = null)
        {
            return MissingQuery(options).Run();
        }

        internal ElementMissingQuery MissingQuery(Options options)
        {
            return new ElementMissingQuery(this, SetOptions(options));
        }
    }
}