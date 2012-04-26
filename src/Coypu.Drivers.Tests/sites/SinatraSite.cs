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
            processStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            process = Process.Start(processStartInfo);
        }

        public void Dispose()
        {
            if (!process.HasExited)
            {
                process.Kill();
                process.Dispose();
            }
        }
    }
}