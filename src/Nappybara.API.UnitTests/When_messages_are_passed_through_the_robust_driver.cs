using System;
using System.Linq;
using NUnit.Framework;

namespace Nappybara.API.UnitTests
{
    [TestFixture]
    public class When_messages_are_passed_through_the_robust_driver
    {
        private FakeDriver driver;
        private DeferedRobustness deferedRobustness;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            deferedRobustness = new DeferedRobustness();
        }

        [Test]
        public void ClickButton_should_make_robust_call_to_underlying_driver()
        {

            var node = new Node(driver);
            new RobustDriver(driver, deferedRobustness).Click(node);

            Assert.That(driver.ClickedNodes, Has.No.Member(node));
            deferedRobustness.DeferredActions.Single()();
            Assert.That(driver.ClickedNodes, Has.Member(node));
        }

		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node(driver);
			driver.StubButton("Find button robustly", node);

			new RobustDriver(driver, deferedRobustness).FindButton("Find button robustly");

			var found = ((Func<Node>)deferedRobustness.DeferredFunctions.Single())();
			Assert.That(found, Is.SameAs(node));
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node(driver);
			driver.StubLink("Find link robustly", node);

			new RobustDriver(driver, deferedRobustness).FindLink("Find link robustly");

			var found = ((Func<Node>)deferedRobustness.DeferredFunctions.Single())();
			Assert.That(found, Is.SameAs(node));
		}

		[Test]
		public void Visit_should_make_direct_call_to_underlying_driver()
		{
			new RobustDriver(driver, deferedRobustness).Visit("some url");
		}
    }
}