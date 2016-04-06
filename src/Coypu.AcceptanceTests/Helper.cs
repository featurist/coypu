using System;
using System.IO;
using System.Reflection;

namespace Coypu.AcceptanceTests
{
    public static class Helper
    {
        private static readonly string WorkingDirectory;

        static Helper()
        {
            var currentDir = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var index = currentDir.LastIndexOf("\\bin\\");
            var projectDir = currentDir.Remove(index);
            var mode = "Debug";

#if (!DEBUG)
            mode = "Release";
#endif
            WorkingDirectory = $"file:///{new DirectoryInfo($@"{projectDir}\bin\{mode}")}";
        }

        public static string GetProjectFile(string relativePath)
        {
            return $@"{WorkingDirectory}\{relativePath}";
        }
    }
}