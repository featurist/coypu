using System;

namespace Coypu.Queries
{
    internal class HasContentQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        internal HasContentQuery(Driver driver, DriverScope scope, string text, Options options) : base(scope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override bool Run()
        {
            return driver.HasContent(text, DriverScope);
        }
    }
}