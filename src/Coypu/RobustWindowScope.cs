using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class RobustWindowScope : BrowserWindow
    {
        private readonly Options options;

        internal RobustWindowScope(Driver driver, SessionConfiguration SessionConfiguration, RobustWrapper robustWrapper, Waiter waiter, UrlBuilder urlBuilder, Options options, WindowFinder windowFinder) 
            : base(SessionConfiguration,windowFinder,driver, robustWrapper, waiter, urlBuilder)
        {
            this.options = options;
        }

        public override ElementFound Now()
        {
            return robustWrapper.Robustly(new ElementQuery(this, options));
        }
    }
}