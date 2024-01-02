using System;
using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
{
    internal class Dialogs
    {
        internal void ActOnDialog(string text, IPage page, Action trigger, string dialogType, Action<IDialog> dialogAction)
        {
            var dialogFound = false;
            var match = false;
            EventHandler<IDialog> listener = (_, dialog) =>
            {
                dialogFound = true;
                if (dialog.Type == dialogType &&
                    (text == null || dialog.Message == text))
                {
                    match = true;
                    dialogAction(dialog);
                }
                else
                {
                    dialog.DismissAsync();
                }
            };
            page.Dialog += listener;
            try
            {
                trigger.Invoke();
            }
            finally
            {
                page.Dialog -= listener; // Only applies for while the trigger action is running
            }
            if (!dialogFound)
            {
                throw new MissingDialogException("No dialog was present to accept");
            }
            if(!match)
            {
                throw new MissingDialogException("A dialog was present but didn't match the expected text or type.");
            }
        }
    }
}
