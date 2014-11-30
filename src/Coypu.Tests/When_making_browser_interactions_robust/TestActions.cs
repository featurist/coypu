using Coypu.Actions;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class CountTriesAction : BrowserAction
    {
        public CountTriesAction(Options options) : base(null, options)
        {
        }

        public int Tries { get; private set; }

        public override void Act()
        {
            Tries++;
        }
    }
}