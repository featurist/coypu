namespace Coypu
{
    /// <summary>
    /// An HTML element
    /// </summary>
    public interface Element {

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

        /// <summary>
        /// The outer HTML of the element
        /// </summary>
        string OuterHTML { get; }

        /// <summary>
        /// The inner HTML of the element
        /// </summary>
        string InnerHTML { get; }

        /// <summary>
        /// The title of the element
        /// </summary>
        string Title { get; }
    }
}