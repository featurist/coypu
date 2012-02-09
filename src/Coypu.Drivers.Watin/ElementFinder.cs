using System;

using WatiN.Core;
using WatiN.Core.Interfaces;

namespace Coypu.Drivers.Watin
{
    public class ElementFinder
    {
        private readonly WatiNDriver driver;

        public ElementFinder(WatiNDriver driver)
        {
            this.driver = driver;
        }

        public WatiN.Core.Element FindFirst<TComponent>(IComponentCollection<TComponent> collection, Predicate<TComponent> predicate)
            where TComponent : Component
        {
            return driver.Filter(collection, predicate).FirstDisplayedOrDefault(driver.Scope);
        }
    }
}