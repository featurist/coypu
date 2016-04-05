using System;
using System.IO;
using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class ShowModalDialog
    {
        [Test]
        public void Modal_dialog()
        {
            using (var session = new BrowserSession(new SessionConfiguration{Browser = Browser.InternetExplorer}))
            {
                VisitTestPage(session);

                var linkId = session.FindLink("Open modal dialog").Id;
                session.ExecuteScript(
                    string.Format("window.setTimeout(function() {{document.getElementById('{0}').click()}},1);", linkId));

                var dialog = session.FindWindow("Pop Up Window");
                dialog.FillIn("text input in popup").With("I'm interacting with a modal dialog");
                dialog.ClickButton("close");
            }
        }

        private void VisitTestPage(BrowserSession browserSession)
        {
            browserSession.Visit(Helper.GetProjectFile(@"html\InteractionTestsPage.htm"));
        }
    }
}