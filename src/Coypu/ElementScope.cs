namespace Coypu
{
    /// <summary>
    /// 
    /// </summary>
    public interface ElementScope : Scope, Element
    {
        bool Exists();
        bool Missing();
        ElementScope Click(Options options = null);
        ElementScope FillInWith(string value, Options options = null);
        ElementScope Hover(Options options = null);
        ElementScope SendKeys(string keys, Options options = null);
        ElementScope Check(Options options = null);
        ElementScope Uncheck(Options options = null);
        bool HasValue(string text, Options options = null);
        bool HasNoValue(string text, Options options = null);
    }
}