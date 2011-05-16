using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_executing_script
    {
        private FakeDriver driver;
        private Session session;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            session = new Session(driver, null);
        }

        [Test]
        public void Visit_passes_message_directly_to_the_driver_returning_response_immediately()
        {
            const string script = "document.getElementById('asdf').click();";
            const string expectedReturnValue = "script return value";

            driver.StubExecuteScript(script, expectedReturnValue);

            Assert.That(session.ExecuteScript(script), Is.EqualTo(expectedReturnValue));
        }
    }
}