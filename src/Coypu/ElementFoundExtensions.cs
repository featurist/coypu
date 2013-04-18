using System.Collections.Generic;
using System.Linq;

namespace Coypu
{
    internal static class ElementFoundExtensions
    {
        public static IEnumerable<SnapshotElementScope> AsSnapshotElementScopes(this IEnumerable<ElementFound> elements, DriverScope driverScope)
        {
            return elements.Select(elementFound => new SnapshotElementScope(elementFound, driverScope));
        }
    }
}