using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Queries
{
    internal class FindAllCssWithPredicateQuery : DriverScopeQuery<IEnumerable<SnapshotElementScope>>
    {
        private readonly string locator;
        private readonly Func<IEnumerable<SnapshotElementScope>,bool> predicate;

        public FindAllCssWithPredicateQuery(string locator, Func<IEnumerable<SnapshotElementScope>,bool> predicate, DriverScope driverScope, Options options) : base(driverScope, options)
        {
            this.locator = locator;
            this.predicate = predicate;
        }

        public override IEnumerable<SnapshotElementScope> Run()
        {
            var allElements = Scope.FindAllCssNoPredicate(locator, Options).ToArray();
            if (!predicate(allElements))
                throw new MissingHtmlException("FindAllCss did not find elements matching your predicate");

            return allElements;
        }

        public override IEnumerable<SnapshotElementScope> ExpectedResult
        {
            get { return null; }
        }
    }
}