using System.Linq;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    public static class ElementCollectionExtensions
    {
        public static WatiN.Core.Element FirstMatching(this ElementCollection elementCollection, params Constraint[] constraints)
        {
            return (from constraint in constraints
                    let element = elementCollection.First(constraint)
                    where element != null
                    select element).FirstOrDefault();
        }
    }
}