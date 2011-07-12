using System;
using System.IO;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class States
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(500);
            ReloadTestPage();
        }

        #endregion

        private Session browser
        {
            get { return Browser.Session; }
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

        [TestFixtureTearDown]
        public void TearDown()
        {
            Browser.EndSession();
        }

        [Test]
        public void Page_reaches_first_of_three_possible_states()
        {
            ShowStateAsync("stateOne", 500);

            var state1 = new State {Condition = () => browser.HasContent("State one reached")};
            var state2 = new State {Condition = () => browser.HasContent("State two reached")};
            var state3 = new State {Condition = () => browser.HasContent("State three reached")};

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state1));
        }

        [Test]
        public void Page_reaches_second_of_three_possible_states()
        {
            ShowStateAsync("stateTwo", 500);

            var state1 = new State {Condition = () => browser.HasContent("State one reached")};
            var state2 = new State {Condition = () => browser.HasContent("State two reached")};
            var state3 = new State {Condition = () => browser.HasContent("State three reached")};

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state2));
        }


        [Test]
        public void Page_reaches_third_of_three_possible_states()
        {
            ShowStateAsync("stateThree", 500);

            var state1 = new State {Condition = () => browser.HasContent("State one reached")};
            var state2 = new State {Condition = () => browser.HasContent("State two reached")};
            var state3 = new State {Condition = () => browser.HasContent("State three reached")};

            State foundState = browser.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state3));
        }
    }
}