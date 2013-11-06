using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu.Queries
{
    internal class HasValueQuery : Query<bool>
    {
        private readonly ElementScope elementScope;
        private readonly string text;

        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }
        public virtual bool ExpectedResult { get { return true; } }

        internal HasValueQuery(ElementScope scope, string text, Options options)
        {
            this.elementScope = scope;
            this.text = text;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public bool Run()
        {
            return elementScope.Now().Value == text;
        }
    }
}