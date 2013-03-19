namespace Coypu
{
    /// <summary>
    /// 
    /// </summary>
    public interface ElementScope : Scope, Element
    {
        bool Exists(Options options = null);
        bool Missing(Options options = null);
        ElementScope Click(Options options = null);
        ElementScope FillInWith(string value, bool forceAllEvents = false, Options options = null);
        ElementScope Hover(Options options = null);
        ElementScope SendKeys(string keys, Options options = null);
        ElementScope Check(Options options = null);
        ElementScope Uncheck(Options options = null);
    }
}