using Coypu.Finders;

namespace Coypu.Tests.TestDoubles
{
    internal class AlwaysExceptionsErrorFinder : ElementFinder
    {
        public AlwaysExceptionsErrorFinder()
            : base(null, null, null)
        {
        }

        internal override ElementFound Find()
        {
            throw new TestException("I always fail");
        }
    }

    public class AlwaysFindsElementFinder : ElementFinder
    {
        private readonly ElementFound element;

        public AlwaysFindsElementFinder(ElementFound element) : base (null,null,null)
        {
            this.element = element;
        }

        internal override ElementFound Find()
        {
            return element;
        }
    }

    internal class AlwaysMissingElementFinder : ElementFinder
    {
        public AlwaysMissingElementFinder() : base(null,null,null)
        {
            
        }

        internal override ElementFound Find()
        {
            throw new MissingHtmlException("From AlwaysMissingElementFinder");
        }
    }
}