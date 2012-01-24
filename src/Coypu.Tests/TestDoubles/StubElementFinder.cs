using System;

namespace Coypu.Tests.TestDoubles
{
    public class StubElementFinder : ElementFinder
    {
        private readonly Element element;

        public StubElementFinder(Element element)
        {
            this.element = element;
        }

        public Element Now()
        {
            return element;
        }

        public TimeSpan Timeout { get; set; }
    }
}