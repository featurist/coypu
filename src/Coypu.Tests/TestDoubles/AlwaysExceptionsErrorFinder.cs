namespace Coypu.Tests.TestDoubles
{
    public class AlwaysExceptionsErrorFinder : ElementFinder
    {
        public AlwaysExceptionsErrorFinder()
            : base(null, null, null)
        {
        }

        internal override Element Find()
        {
            throw new ExplicitlyThrownTestException("I always fail");
        }
    }

    public class AlwaysFindsElementFinder : ElementFinder
    {
        private readonly Element element;

        public AlwaysFindsElementFinder(): this(new StubElement())
        {
        }

        public AlwaysFindsElementFinder(Element element) : base (null,null,null)
        {
            this.element = element;
        }

        internal override Element Find()
        {
            return element;
        }
    }

    public class AlwaysMissingElementFinder : ElementFinder
    {
        public AlwaysMissingElementFinder() : base(null,null,null)
        {
            
        }

        internal override Element Find()
        {
            throw new MissingHtmlException("From AlwaysMissingElementFinder");
        }
    }
}