using System.IO;
using NUnit.Framework;

namespace Nappybara.DriverImplementationTests
{
    public abstract class RealDriverImplementationTestSuite
    {
        [SetUp]
        public void SetUp()
        {
            Driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
        }

        [TestFixtureTearDown]
        public abstract void DisposeBrowser();

        private const string INTERACTION_TESTS_PAGE = @"..\..\html\InteractionTestsPage.htm";
        protected abstract Driver Driver { get; }

        [Test]
        public void FindButton_should_find_a_particular_button_by_its_text()
        {
            Assert.That(Driver.FindButton("I am the first button").Id, Is.EqualTo("firstButtonId"));
            Assert.That(Driver.FindButton("I am the second button").Id, Is.EqualTo("secondButtonId"));
        }

        [Test]
        public void FindButton_should_find_a_particular_button_by_its_id_if_not_by_text()
        {
            Assert.That(Driver.FindButton("firstButtonId").Text, Is.EqualTo("I am the first button"));
            Assert.That(Driver.FindButton("thirdButtonId").Text, Is.EqualTo("I am the third button"));
        }
    }
}