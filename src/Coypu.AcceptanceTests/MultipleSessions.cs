using System.IO;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class MultipleSessions
    {
        [Test]
        public void Two_browser_sessions_can_be_controlled_independently()
        {
            using (var sessionOne = new BrowserSession())
            {
                using (var sessionTwo = new BrowserSession())
                {

                    VisitTestPage(sessionOne);
                    VisitTestPage(sessionTwo);

                    sessionOne.FindCss("input[type=text]", new Options { Match = Match.First }).FillInWith("from session one");
                    sessionTwo.FindCss("input[type=text]", new Options { Match = Match.First }).FillInWith("from session two");

                    Assert.That(sessionOne.FindCss("input[type=text]", new Options { Match = Match.First }).Value, Is.EqualTo("from session one"));
                    Assert.That(sessionTwo.FindCss("input[type=text]", new Options { Match = Match.First }).Value, Is.EqualTo("from session two"));
                }
            }
        }

        private void VisitTestPage(BrowserSession browserSession)
        {
            browserSession.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }
    }
}