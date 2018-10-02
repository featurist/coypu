using System.IO;
using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class ShowModalDialog
    {
        private void VisitTestPage(BrowserSession browserSession)
        {
            browserSession.Visit("file:///" + Path.Combine(TestContext.CurrentContext.TestDirectory, "html\\InteractionTestsPage.htm")
                                                  .Replace("\\", "/"));
        }

        [Test]
        public void Modal_dialog()
        {
            using (var session = new BrowserSession(new SessionConfiguration {Browser = Browser.Firefox}))
            {
                VisitTestPage(session);
                var linkId = session.FindLink("Open modal dialog")
                                    .Id;
                session.ExecuteScript($"window.setTimeout(function() {{document.getElementById('{linkId}').click()}},1);");

                var dialog = session.FindWindow("Pop Up Window");
                dialog.FillIn("text input in popup")
                      .With("I'm interacting with a modal dialog");
                dialog.ClickButton("close");
            }
        }
    }
}