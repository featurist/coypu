using System;

using WatiN.Core.Native.InternetExplorer;
using WatiN.Core.Native.Windows;

namespace Coypu.Drivers.Watin
{
    public class IEWaitForCompleteWithDialogs : IEWaitForComplete
    {
        private readonly IEBrowser ieBrowser;
        private readonly Func<bool> hasDialog;

        public IEWaitForCompleteWithDialogs(IEBrowser ieBrowser, int waitForCompleteTimeOut, Func<bool> hasDialog)
            : base(ieBrowser, waitForCompleteTimeOut)
        {
            this.ieBrowser = ieBrowser;
            this.hasDialog = hasDialog;
        }

        protected override void WaitForCompleteOrTimeout()
        {
            if (ieBrowser != null && !new Hwnd(ieBrowser.hWnd).IsWindowEnabled)
            {
                WaitUntil(() => hasDialog(), () => "Expected dialog did not appear");
                return;
            }

            base.WaitForCompleteOrTimeout();
        }
    }
}