using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coypu;
using Coypu.Finders;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu
{
    public class FieldAutocomplete : ElementScope
    {
        protected string elementsXPath;
        protected string listXPath;
        ElementScope _element;
        protected Options options;

        public FieldAutocomplete(ElementScope field, string listXPath, string elementsXPath, Options options = null)
        {
            _element = field;
            this.listXPath = listXPath;
            this.elementsXPath = elementsXPath;
            this.options = Merge(options);

            SetScope(field.OuterScope);
            SetFinder(field.elementFinder);
        }

        public override ElementScope FillInWith(string value, Options options = null)
        {
            _element.FillInWith(value, Merge(options));

            var list = OuterScope.FindXPath(listXPath, new Options() { Timeout = TimeSpan.FromSeconds(30) });
            if (!list.Exists(Merge(options)))
                throw new Exception("Autocomplete list has not been found.");
            else
                Console.WriteLine(list.Text);
            list.FindAllXPath(elementsXPath).Where(x => x.Text == value).First().Click(Merge(options));

            return this;
        }

        internal override bool Stale { get; set; }

        public override Element Now()
        {
            if (_element != null) return _element.Now();
            return timingStrategy.Synchronise(new ElementQuery(this, options));
        }

        internal override void Try(DriverAction action)
        {
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
            return Try(new ElementExistsQuery(_element, Merge(options)));
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
            return Try(new ElementMissingQuery(_element, Merge(options)));
        }

        internal override bool Try(Query<bool> query)
        {
            return Query(query);
        }

        internal override T Try<T>(Func<T> getAttribute)
        {
            return Query(new LambdaQuery<T>(getAttribute, null, this, options));
        }
    }
}