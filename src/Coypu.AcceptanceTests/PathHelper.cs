using System.IO;
using NUnit.Framework;

namespace Coypu.AcceptanceTests;

internal static class PathHelper
{
    internal static string GetPageHtmlPath(string page)
    {
        var schema = "file:///";
        if (TestContext.CurrentContext.TestDirectory.Length > 0 &&
            TestContext.CurrentContext.TestDirectory[0] == '/')
        {
            schema = "file://";
        }
        return schema + Path.Combine(TestContext.CurrentContext.TestDirectory, "html", page).Replace("\\", "/");
    }
}