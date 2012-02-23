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
            using (var sessionOne = Browser.NewSession())
            {
                using (var sessionTwo = Browser.NewSession())
                {

                    VisitTestPage(sessionOne);
                    VisitTestPage(sessionTwo);

                    sessionOne.FillIn(sessionOne.FindCss("input[type=text]")).With("from session one");
                    sessionOne.FillIn(sessionTwo.FindCss("input[type=text]")).With("from session two");

                    Assert.That(sessionOne.FindCss("input[type=text]").Value, Is.EqualTo("from session one"));
                    Assert.That(sessionTwo.FindCss("input[type=text]").Value, Is.EqualTo("from session two"));
                }
            }
        }

        private void VisitTestPage(Session session)
        {
            session.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }
    }
}