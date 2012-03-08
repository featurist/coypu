using System;
using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu
{
    public class IFrameElementScope : RobustElementScope
    {
        internal IFrameElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper, Options options) 
            : base(elementFinder, outerScope, robustWrapper,options)
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