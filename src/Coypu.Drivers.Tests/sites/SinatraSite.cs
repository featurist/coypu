using System;
using System.Diagnostics;
using System.IO;

namespace Coypu.Drivers.Tests.Sites
{
    public class SinatraSite : IDisposable
    {
        private readonly Process process;

        public SinatraSite(string sitePath)
        {
            var processStartInfo = new ProcessStartInfo("ruby", "\"" + new FileInfo(sitePath).FullName + "\"");
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            process = Process.Start(processStartInfo);
        }

        public void Dispose()
        {
            process.Kill();
            process.Dispose();
        }
    }
}