using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_sizing_windows : DriverSpecs
    {

        [Test]
        public void MaximisesWindow()
        {
            AssertMaximisesWindow(Root);
        }

        [Test]
        public void MaximisesCorrectWindowScope()
        {
            Driver.Click(Driver.FindLink("Open pop up window", Root));
            var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);

            try
            {
                AssertMaximisesWindow(popUp);
            }
            finally
            {
                Driver.ExecuteScript("return self.close();", popUp);
            }
        }

        private static void AssertMaximisesWindow(DriverScope driverScope)
        {
            var availWidth = float.Parse(Driver.ExecuteScript("return window.screen.availWidth;", driverScope));
            var initalWidth = float.Parse(Driver.ExecuteScript("return window.outerWidth;", driverScope));

            Assert.That(initalWidth, Is.LessThan(availWidth));

            Driver.MaximiseWindow(driverScope);

            Assert.That(float.Parse(Driver.ExecuteScript("return window.outerWidth;", driverScope)), Is.GreaterThanOrEqualTo(availWidth));
        }
    }
}