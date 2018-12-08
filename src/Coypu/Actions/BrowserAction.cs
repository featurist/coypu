using System;
using Coypu.Queries;

namespace Coypu.Actions
{
    public abstract class BrowserAction : Query<object>
    {
        public Options Options { get; }
        public DriverScope Scope { get; }

        protected BrowserAction(DriverScope scope, Options options)
        {
            Options = options;
            Scope = scope;
        }

        public abstract void Act();

        public object Run()
        {
            Act();
            return null;
        }

        public object ExpectedResult => null;
    }
}