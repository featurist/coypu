using System;
using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu
{
    public class IFrameElementScope : RobustElementScope
    {
        internal IFrameElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper) : base(elementFinder, outerScope, robustWrapper)
        {
        }

        public Uri Location
        {
            get
            {
                FindXPath("/*").Now();
                return DriverScope.Location;
            }
        }
    }
}