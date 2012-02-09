using WatiN.Core;
using WatiN.Core.Native.InternetExplorer;
using WatiN.Core.Native.Windows;

namespace Coypu.Drivers.Watin
{
    public class IEWithDialogWaiter : IE
    {
        public override void WaitForComplete(int waitForCompleteTimeOut)
        {
            WaitForComplete(new IEWaitForCompleteWithDialogs((IEBrowser)NativeBrowser, waitForCompleteTimeOut));
        }
    }

    public class IEWaitForCompleteWithDialogs : IEWaitForComplete
    {
        private readonly IEBrowser ieBrowser;

        public IEWaitForCompleteWithDialogs(IEBrowser ieBrowser, int waitForCompleteTimeOut)
            : base(ieBrowser, waitForCompleteTimeOut)
        {
            this.ieBrowser = ieBrowser;
        }

        protected override void WaitForCompleteOrTimeout()
        {
            if (ieBrowser != null && !new Hwnd(ieBrowser.hWnd).IsWindowEnabled)
                return;

            base.WaitForCompleteOrTimeout();
        }
    }
}