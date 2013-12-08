using Coypu.Actions;
using Coypu.Queries;

namespace Coypu
{
    /// <summary>
    /// The scope of an element already found in the document, therefore not deferred. 
    /// 
    /// If this element becomes stale then using this scope will not try to refind the element but 
    /// will raise a MissingHtmlException immediately.
    /// </summary>
    public class SnapshotElementScope : DeferredElementScope
    {
        private readonly ElementFound elementFound;
        private readonly Options options;

        internal SnapshotElementScope(ElementFound elementFound, DriverScope scope, Options options)
            : base(null, scope)
        {
            this.elementFound = elementFound;
            this.options = options;
        }

        public override ElementFound Now()
        {
            if (elementFound.Stale(options))
                throw new MissingHtmlException(string.Format("Snapshot element scope has become stale. {0}", elementFound));

            return elementFound;
        }

        internal override void Try(DriverAction action)
        {
            action.Act();
        }

        internal override bool Try(Query<bool> query)
        {
            return query.Run();
        }
    }
}