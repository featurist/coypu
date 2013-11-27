using System.Drawing;
using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_sizing_windows : DriverSpecs
    {
        [Test]
        public void MaximisesWindow()
        {
            using (Driver)
            {
                AssertMaximisesWindow(Root);
            }
        }

        [Test]
        public void MaximisesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root),
                                            Driver, null, null, null);

                try
                {
                    AssertMaximisesWindow(popUp);
                }
                finally
                {
                    Driver.ExecuteScript("return self.close();", popUp);
                }
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



        [Test]
        public void ResizesWindow()
        {
            using (Driver)
            {
                AssertResizesWindow(Root);
            }
        }

        [Test]
        public void ResizesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root),
                                            Driver, null, null, null);

                try
                {
                    AssertResizesWindow(popUp);
                }
                finally
                {
                    Driver.ExecuteScript("return self.close();", popUp);
                }
            }
        }

        private static void AssertResizesWindow(DriverScope driverScope)
        {
            var availWidth = float.Parse(Driver.ExecuteScript("return window.screen.availWidth;", driverScope));
            var initalWidth = float.Parse(Driver.ExecuteScript("return window.outerWidth;", driverScope));

            Assert.That(initalWidth, Is.LessThan(availWidth));

            Driver.ResizeTo(new Size(768, 500), driverScope);

            Assert.That(float.Parse(Driver.ExecuteScript("return window.outerWidth;", driverScope)), Is.EqualTo(768));
            Assert.That(float.Parse(Driver.ExecuteScript("return window.outerHeight;", driverScope)), Is.EqualTo(500));
        }
    }
}