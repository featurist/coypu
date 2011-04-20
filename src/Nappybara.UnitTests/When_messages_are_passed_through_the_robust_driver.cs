using System;
using System.Linq;
using Nappybara.Drivers;
using NUnit.Framework;

namespace Nappybara.UnitTests
{
    [TestFixture]
    public class When_messages_are_passed_through_the_robust_driver
    {
        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            deferedRobustness = new DeferedRobustWrapper();
        }

        private FakeDriver driver;
        private DeferedRobustWrapper deferedRobustness;

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

            Node found = ((Func<Node>) deferedRobustness.DeferredFunctions.Single())();
            Assert.That(found, Is.SameAs(node));
        }

        [Test]
        public void FindLink_should_make_robust_call_to_underlying_driver()
        {
            var node = new Node(driver);
            driver.StubLink("Find link robustly", node);

            new RobustDriver(driver, deferedRobustness).FindLink("Find link robustly");

            Node found = ((Func<Node>) deferedRobustness.DeferredFunctions.Single())();
            Assert.That(found, Is.SameAs(node));
        }

        [Test]
        public void Visit_should_make_direct_call_to_underlying_driver()
        {
            new RobustDriver(driver, deferedRobustness).Visit("some url");
        }
    }
}