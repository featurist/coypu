using System;

namespace Coypu.Queries
{
    internal class ElementMissingQuery : Query<bool>
    {
        private readonly DriverScope driverScope;
        public bool Result { get; private set; }

        public object ExpectedResult
        {
            get { return true; }
        }

        protected internal ElementMissingQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
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
                Result = false;
            }
            catch (MissingHtmlException)
            {
                Result = true;
            }
        }
    }
}