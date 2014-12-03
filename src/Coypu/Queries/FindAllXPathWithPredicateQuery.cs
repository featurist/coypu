using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Queries
{
    internal class FindAllXPathWithPredicateQuery : DriverScopeQuery<IEnumerable<SnapshotElementScope>>
    {
        private readonly string locator;
        private  Func<IEnumerable<SnapshotElementScope>, bool> predicate;

        public FindAllXPathWithPredicateQuery(string locator, Func<IEnumerable<SnapshotElementScope>, bool> predicate, DriverScope driverScope, Options options)
            : base(driverScope, options)
        {
            if (predicate == null)
                predicate = e => true;

            this.predicate = predicate;
            this.locator = locator;
        }

        public override IEnumerable<SnapshotElementScope> Run()
        {
            var allElements = Scope.FindAllXPathNoPredicate(locator, Options).ToArray();
            if (!predicate(allElements))
                throw new MissingHtmlException("FindAllXPath did not find elements matching your predicate");

            return allElements;
        }

        public override object ExpectedResult
        {
            get { return null; }
        }
    }
}