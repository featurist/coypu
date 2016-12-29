using System;
using System.IO;
using Xunit;
using Coypu.Queries;

namespace Coypu.AcceptanceTests
{
    public class States : IClassFixture<StatesFixture>
    {
        private BrowserSession browser;
        
        public States(StatesFixture statesFixture)
        {
            browser = statesFixture.BrowserSession;
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

        [Fact]
        public void Page_reaches_first_of_three_possible_states()
        {
            ShowStateAsync("state1", 500);

            var state1 = new State(() => browser.HasContent("State one reached"));
            var state2 = new State(() => browser.HasContent("State two reached"));
            var state3 = new State(() => browser.HasContent("State three reached"));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.Same(state1, foundState);
        }

        [Fact]
        public void Page_reaches_second_of_three_possible_states()
        {
            ShowStateAsync("state2", 500);

            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.Same(state2, foundState);
        }


        [Fact]
        public void Page_reaches_third_of_three_possible_states()
        {
            ShowStateAsync("state3", 500);

            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            State foundState = browser.FindState(state1, state2, state3);

            Assert.Same(state3, foundState);
        }

        [Fact]
        public void Page_reaches_none_of_three_possible_states()
        {
            var state1 = new State(new LambdaQuery<bool>(() => browser.HasContent("State one reached")));
            var state2 = new State(new LambdaQuery<bool>(() => browser.HasContent("State two reached")));
            var state3 = new State(new LambdaQuery<bool>(() => browser.HasContent("State three reached")));

            Assert.Throws<MissingHtmlException>(() => browser.FindState(state1, state2, state3));
        }
    }

    public class StatesFixture : IDisposable
    {
        public BrowserSession BrowserSession;

        public StatesFixture()
        {
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(1000),
            };
            BrowserSession = new BrowserSession(configuration);
        }

        public void Dispose()
        {
            BrowserSession.Dispose();
        }
    }
}