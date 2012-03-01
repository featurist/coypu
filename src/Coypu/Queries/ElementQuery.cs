using System;

namespace Coypu.Queries
{
    internal class ElementQuery : Query<ElementFound>
    {
        private readonly DriverScope driverScope;


        public ElementQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
            Timeout = driverScope.Timeout;
        }

        public ElementFound Result { get; set; }
        public TimeSpan Timeout { get; set; }

        public object ExpectedResult
        {
            get { return null; }
        }

        public void Run()
        {
            Result = driverScope.Now();
        }
    }
}