using System;

namespace Coypu
{
    /// <summary>
    /// An HTML element
    /// </summary>
    public interface Element
    {
        /// <summary>
        /// The value of the 'id' attribute
        /// </summary>
        string Id { get; }
        /// <summary>
        /// The inner text of the element
        /// </summary>
        string Text { get; }
        /// <summary>
        /// The value of the 'value' attribute
        /// </summary>
        string Value { get; }
        /// <summary>
        /// The value of the 'name' attribute
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The selected option - applies to select elements only
        /// </summary>
        string SelectedOption { get; }
        /// <summary>
        /// Whether the element is selected
        /// </summary>
        bool Selected { get; }
        /// <summary>
        /// The native element returned by your chosen driver
        /// </summary>
        object Native { get; }
        /// <summary>
        /// The attributes of the HTML element
        /// </summary>
        string this[string attributeName] { get; }
    }

    public class ScopedElement : Element
    {
        private readonly ElementFinder elementFinder;
        private readonly Session session;

        public ScopedElement(ElementFinder elementFinder, Session session)
        {
            this.elementFinder = elementFinder;
            this.session = session;
            throw new System.NotImplementedException();
        }

        public Element Now()
        {
            return elementFinder.Now();
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

        public ScopedElement WithIndividualTimeout(TimeSpan individualTimeout)
        {
            elementFinder.Timeout = individualTimeout;
        }
    }
}