using System.IO;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class MultipleSessions
    {
        [Fact]
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

                    Assert.Equal("from session one", sessionOne.FindCss("input[type=text]", new Options { Match = Match.First }).Value);
                    Assert.Equal("from session two", sessionTwo.FindCss("input[type=text]", new Options { Match = Match.First }).Value);
                }
            }
        }

        private void VisitTestPage(BrowserSession browserSession)
        {
            browserSession.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }
    }
}