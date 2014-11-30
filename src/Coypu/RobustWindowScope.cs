using Coypu.Finders;
using Coypu.Queries;
using Coypu.Timing;

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

        public override Element Now()
        {
            return timingStrategy.Synchronise(new ElementQuery(this, options));
        }
    }
}