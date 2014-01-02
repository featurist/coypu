namespace Coypu
{
    /// <summary>
    /// <para>With Match you can control how Coypu behaves when multiple elements match a query.</para>
    /// <para><code>Match.First</code> just takes the first match and ignores any others</para>
    /// <para><code>Match.Single</code> throws <code>Coypu.AmbiguousException</code> if there is more than one match</para>
    /// </summary>
    public enum Match
    {
        /// <summary>
        /// Just picks the first element that matches
        /// </summary>
        First,

        /// <summary>
        /// Raises an error if more than one element matches
        /// </summary>
        Single
    }

    public enum TextPrecision
    {
        Exact,
        Substring,
        PreferExact
    }


}