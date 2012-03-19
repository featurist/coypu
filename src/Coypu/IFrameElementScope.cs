using System;
using Coypu.Finders;

namespace Coypu
{
    public class IFrameElementScope : RobustElementScope
    {
        internal IFrameElementScope(ElementFinder elementFinder, DriverScope outerScope, Options options) 
            : base(elementFinder, outerScope,options)
        {
        }

        public override Uri Location
        {
            get
            {
                FindXPath("/*").Now();
                return driver.Location;
            }
        }
    }
}