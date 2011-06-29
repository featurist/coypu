namespace Coypu
{
    /// <summary>
    /// An HTML element
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// The value of the 'id' attribute
        /// </summary>
        public abstract string Id { get; }
        /// <summary>
        /// The inner text of the element
        /// </summary>
        public abstract string Text { get; }
        /// <summary>
        /// The value of the 'value' attribute
        /// </summary>
        public abstract string Value { get; }
        /// <summary>
        /// The value of the 'name' attribute
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// The selected option - applies to select elements only
        /// </summary>
        public abstract string SelectedOption { get; }
        /// <summary>
        /// Whether the element is selected
        /// </summary>
        public abstract bool Selected { get; }
        /// <summary>
        /// The native element returned by your chosen driver
        /// </summary>
        public object Native { get; protected set; }
        /// <summary>
        /// The attributes of the HTML element
        /// </summary>
        public abstract string this[string attributeName] { get; }
    }
}