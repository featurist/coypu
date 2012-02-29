using System;

namespace Coypu.Queries
{
    internal abstract class ElementPresenceQuery : Query<bool>
    {
        private readonly DriverScope driverScope;
        private readonly bool resultOnFound;
        public abstract object ExpectedResult { get; }
        public bool Result { get; private set; }

        protected ElementPresenceQuery(DriverScope driverScope, bool resultOnFound)
        {
            this.driverScope = driverScope;
            this.resultOnFound = resultOnFound;
        }

        public TimeSpan Timeout
        {
            get { return driverScope.Timeout; }
        }

        public void Run()
        {
            try
            {
                driverScope.Now();
                Result = resultOnFound;
            }
            catch (MissingHtmlException)
            {
                Result = !resultOnFound;
            }
        }
    }
}