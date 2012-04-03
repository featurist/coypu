using System;
using System.IO;
using NUnit.Framework;
using Coypu.Queries;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class States
    {
        private SessionConfiguration SessionConfiguration;
        private BrowserSession browser;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            browser = new BrowserSession(SessionConfiguration);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            browser.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            
            ReloadTestPage();
        }

        private void ShowStateAsync(string id, int delayMilliseconds)
        {
            browser.ExecuteScript(
                string.Format("setTimeout(function() {{document.getElementById('{0}').style.display = 'block'}},{1})",
                              id, delayMilliseconds));
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\states.htm").FullName.Replace("\\", "/"));
        }

        [Test]
        public void Page_reaches_first_of_three_possible_states()
        {
            ShowStateAsync("state1", 500);

            var state1 = new State(() => browser.HasContent("State one reached"));
            var state2 = new State(() => browser.HasContent("State two reached"));
            var state3 = new State(() => browser.HasContent("State three reached"));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state1));
        }

        [Test]
        public void Page_reaches_second_of_three_possible_states()
        {
            ShowStateAsync("state2", 500);

            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state2));
        }


        [Test]
        public void Page_reaches_third_of_three_possible_states()
        {
            ShowStateAsync("state3", 500);

            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state3));
        }

        [Test]
        public void Page_reaches_none_of_three_possible_states()
        {
            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            Assert.Throws<MissingHtmlException>(() => browser.FindState(state1, state2, state3));
        }
    }
}