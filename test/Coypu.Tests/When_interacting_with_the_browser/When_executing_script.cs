using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_executing_script : BrowserInteractionTests
    {
        [Fact]
        public void Visit_passes_message_directly_to_the_driver_returning_response_immediately()
        {
            const string script = "document.getElementById('asdf').click();";
            const string expectedReturnValue = "script return value";

            driver.StubExecuteScript(script, expectedReturnValue, browserSession);

            Assert.Equal(expectedReturnValue, browserSession.ExecuteScript(script));
        }
    }
}