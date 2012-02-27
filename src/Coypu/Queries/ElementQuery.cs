using System;
using Coypu.Finders;

namespace Coypu.Queries
{
    internal class ElementQuery : Query<Element>
    {
        private readonly ElementFinder elementFinder;

        public ElementQuery(ElementFinder elementFinder, TimeSpan timeout)
        {
            Timeout = timeout;
            this.elementFinder = elementFinder;
        }

        public Element Result { get; set; }
        public TimeSpan Timeout { get; set; }

        public object ExpectedResult
        {
            get { return null; }
        }

        public void Run()
        {
            Result = elementFinder.Find();
        }
    }
}