using System;
using System.IO;
using System.Reflection;

namespace Coypu.AcceptanceTests
{
    public static class Helper
    {
        public static string GetProjectFile(string relativePath)
        {
            var currentDir = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var index = currentDir.LastIndexOf("\\bin\\");
            var projectDir = currentDir.Remove(index);
            var mode = "Debug";

#if (!DEBUG)
            mode = "Release";
#endif
            return $"file:///{new DirectoryInfo($@"{projectDir}\bin\{mode}\{relativePath}")}";
        }
    }
}