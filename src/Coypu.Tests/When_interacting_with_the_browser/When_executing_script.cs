using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_executing_script : BrowserInteractionTests
    {
        [Test]
        public void Visit_passes_message_directly_to_the_driver_returning_response_immediately()
        {
            const string script = "document.getElementById('asdf').click();";
            const string expectedReturnValue = "script return value";

            driver.StubExecuteScript(script, expectedReturnValue, browserSession.DriverScope);

            Assert.That(browserSession.ExecuteScript(script), Is.EqualTo(expectedReturnValue));
        }
    }
}