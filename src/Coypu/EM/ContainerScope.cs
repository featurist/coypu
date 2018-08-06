using System;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu
{
    public class ContainerScope : ElementScope
    {
        protected string defaultLocator = null;
        protected Options options;
        bool init = false;

        public ContainerScope() : base (null, null)
        {

        }

        public void Init(DriverScope outerScope, Options options)
        {
            Init(defaultLocator, outerScope, options);
        }

        public void Init(string xpath, DriverScope outerScope, Options options)
        {
            init = true;
            this.options = Merge(options);
            SetScope(outerScope);
            SetFinder(new XPathFinder(driver, xpath, outerScope, this.options));

            UpdateChildren(this);
        }

        protected void UpdateChildren(DriverScope s)
        {
            foreach (var f in GetType().GetFields())
            {
                if (f.FieldType == typeof(ElementScope))
                {
                    var e = (ElementScope)f.GetValue(this);
                    if (e != null)
                    {
                        e.SetScope(s);
                    }
                }
            }
        }

        SnapshotElementScope _element;
        public void Init(SnapshotElementScope self, Options options = null)
        {
            init = true;
            this.options = Merge(options);
            _element = self;
            UpdateChildren(_element);
        }

        internal override bool Stale { get; set; }

        public override Element Now()
        {
            if (!init) throw new Exception("ContainerScope not initialized.");
            if (_element != null) return _element.Now();
            return timingStrategy.Synchronise(new ElementQuery(this, options));
        }

        internal override void Try(DriverAction action)
        {
            if (!init) throw new Exception("ContainerScope not initialized.");
            if (_element != null) _element.Try(action);
            RetryUntilTimeout(action);
        }

        /// <summary>
        /// <para>Check if this element exists within the <see cref="SessionConfiguration.Timeout"/></para>
        /// </summary>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        public override bool Exists(Options options = null)
        {
            if (_element != null) return _element.Exists(options);
            return Try(new ElementExistsQuery(this, Merge(options)));
        }

        /// <summary>
        /// <para>Check if this element becomes missing within the <see cref="SessionConfiguration.Timeout"/></para>
        /// </summary>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        public override bool Missing(Options options = null)
        {
            if (_element != null) return _element.Missing(options);
            return Try(new ElementMissingQuery(this, Merge(options)));
        }

        internal override bool Try(Query<bool> query)
        {
            if (!init) throw new Exception("ContainerScope not initialized.");
            if (_element != null) return _element.Try(query);
            return Query(query);
        }

        internal override T Try<T>(Func<T> getAttribute)
        {
            if (!init) throw new Exception("ContainerScope not initialized.");
            if (_element != null) return _element.Try(getAttribute);
            return Query(new LambdaQuery<T>(getAttribute, null, this, options));
        }

        static protected BrowserSession scope;

        public static ElementScope Css(string locator, Options options = null)
        {
            return new CssFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope Link(string locator, Options options = null)
        {
            return new LinkFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope Field(string locator, Options options = null)
        {
            return new FieldFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope Select(string locator, Options options = null)
        {
            return new SelectFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope Text(string text, Options options = null)
        {
            string locator = $"//*[text()='{text}' or contains(text(),'{text}')]";
            return new XPathFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope Button(string locator, Options options = null)
        {
            return new ButtonFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static ElementScope XPath(string locator, Options options = null)
        {
            return new XPathFinder(null, locator, null, Merge(options)).AsScope();
        }

        public static Options Merge(Options options)
        {
            if (options == null)
                return new Options { Match = Match.First };
            else
                return options;
        }
    }
}
