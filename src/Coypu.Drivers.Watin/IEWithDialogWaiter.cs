using System;

using WatiN.Core;
using WatiN.Core.Interfaces;
using WatiN.Core.Native.InternetExplorer;
using WatiN.Core.Native.Windows;

namespace Coypu.Drivers.Watin
{
    public class IEWithDialogWaiter : IE
    {
        private HasDialogHandler hasDialogHandler;

        public override void WaitForComplete(int waitForCompleteTimeOut)
        {
            if (hasDialogHandler == null)
            {
                hasDialogHandler = new HasDialogHandler();
                DialogWatcher.Add(hasDialogHandler);
            }

            WaitForComplete(new IEWaitForCompleteWithDialogs((IEBrowser)NativeBrowser, waitForCompleteTimeOut, hasDialogHandler.HasDialog));
        }

        private class HasDialogHandler : IDialogHandler
        {
            private Window lastSeenWindow;

            public bool HandleDialog(Window window)
            {
                return false;
            }

            public bool CanHandleDialog(Window window, IntPtr mainWindowHwnd)
            {
                lastSeenWindow = window;
                return false;
            }

            public bool HasDialog()
            {
                return lastSeenWindow != null && lastSeenWindow.Exists();
            }
        }
    }
}