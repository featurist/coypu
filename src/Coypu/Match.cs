namespace Coypu
{
    /// <summary>
    /// <para>With Match you can control how Coypu behaves when multiple elements all match a query. There are currently two different strategies:</para>
    /// <para>Match.First: The default strategy. If multiple matches are found, some of which are exact, and some of which are not, then the first exactly matching element is returned.</para>
    /// <para>Match.Single: If the Exact option is true, raises an error if more than one element matches, just like one. If Exact is false, it will first try to find an exact match. An error is raised if more than one element is found. If no element is found, a new search is performed which allows partial matches. If that search returns multiple matches, an error is raised.</para>
    /// </summary>
    public enum Match
    {
        /// <summary>
        /// If multiple matches are found, some of which are exact, and some of which are not, then the first exactly matching element is returned.
        /// </summary>
        First,

        /// <summary>
        /// If exact is true, raises an error if more than one element matches, just like one. If exact is false, it will first try to find an exact match. An error is raised if more than one element is found. If no element is found, a new search is performed which allows partial matches. If that search returns multiple matches, an error is raised.
        /// </summary>
        Single
    }
}