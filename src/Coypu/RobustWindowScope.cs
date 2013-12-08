using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class RobustWindowScope : BrowserWindow
    {
        private readonly Options options;

        internal RobustWindowScope(WindowFinder windowFinder, BrowserSession scope, Options options)
            : base(windowFinder, scope)
        {
            this.options = options;
        }

        public override ElementFound Now()
        {
            return robustWrapper.Robustly(new ElementQuery(this, options));
        }
    }
}