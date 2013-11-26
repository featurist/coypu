namespace Coypu
{
    /// <summary>
    /// Whether to consider a partial or exact match of the text of labels when finding a field
    /// </summary>
    public enum FieldFinderPrecision
    {
        /// <summary>
        /// Match any label that partially matches the locator
        /// </summary>
        PartialLabel,

        /// <summary>
        /// Only match any label that exactly matches the locator
        /// </summary>
        ExactLabel
    }
}