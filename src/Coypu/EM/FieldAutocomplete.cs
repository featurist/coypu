using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coypu;
using Coypu.Finders;

namespace Coypu
{
    public class FieldAutocomplete : SynchronisedElementScope
    {
        protected string fieldXPath;
        protected string listXPath;
        protected string elementsXPath;

        public FieldAutocomplete(string fieldXPath, string listXPath, string elementsXPath, Options options = null)
            : base(new FieldFinder(null, fieldXPath, null, options), null, options)
        {
            this.fieldXPath = fieldXPath;
            this.listXPath = listXPath;
            this.elementsXPath = elementsXPath;
        }

        public FieldAutocomplete Init(DriverScope outerScope, Options options = null)
        {
            this.options = Merge(options);
            SetScope(outerScope);
            SetFinder(new FieldFinder(driver, fieldXPath, outerScope, this.options));

            return this;
        }

        public override ElementScope FillInWith(string value, Options options = null)
        {
            base.FillInWith(value, Merge(options));

            var list = OuterScope.FindXPath(listXPath, new Options() { Timeout = TimeSpan.FromSeconds(30) });
            if (!list.Exists(Merge(options)))
                throw new Exception("Autocomplete list has not been found.");
            else
                Console.WriteLine(list.Text);
            list.FindAllXPath(elementsXPath).Where(x => x.Text == value).First().Click(Merge(options));

            return this;
        }
    }
}