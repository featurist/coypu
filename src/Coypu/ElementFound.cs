namespace Coypu
{
    /// <summary>
    /// An element found by a Coypu Driver
    /// </summary>
    public interface ElementFound : Element
    {
        /// <summary>
        /// The native element is no longer attached to the DOM
        /// </summary>
        bool Stale { get; }
    }
}