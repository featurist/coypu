using System;
using System.Diagnostics;
using System.IO;

namespace Coypu.AcceptanceTests.sites
{
    internal class SinatraSite : IDisposable
    {
        private readonly Process process;

        internal SinatraSite(string name)
        {
            var sitePath = string.Format(@"sites\{0}.rb", name);
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