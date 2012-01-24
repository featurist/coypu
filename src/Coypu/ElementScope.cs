namespace Coypu
{
    public class ElementScope : DriverScope, Element
    {
        private readonly ElementFinder elementFinder;
        private readonly Scope outer;

        public ElementScope(ElementFinder elementFinder, DriverScope outer) : base(outer)
        {
            this.elementFinder = elementFinder;
            this.outer = outer;
        }

        public string Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Text
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Value
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public string SelectedOption
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Selected
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Native
        {
            get { throw new System.NotImplementedException(); }
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}