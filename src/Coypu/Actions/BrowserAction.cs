using System;
using Coypu.Queries;

namespace Coypu.Actions
{
    public abstract class BrowserAction : Query<object>
    {
        public Options Options { get; private set; }

        protected BrowserAction(Options options)
        {
            Options = options;
        }

        public abstract void Act();

        public object Run()
        {
            Act();
            return null;
        }

        public object ExpectedResult
        {
            get { return null; }
        }
    }
}