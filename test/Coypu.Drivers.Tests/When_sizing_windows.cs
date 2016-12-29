using System.Drawing;
using Coypu.Finders;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_sizing_windows : DriverSpecs
    {
        [Fact]
        public void MaximisesWindow()
        {
            using (Driver)
            {
                AssertMaximisesWindow(Root);
            }
        }

        [Fact]
        public void MaximisesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);

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
            var availWidth = Driver.ExecuteScript("return window.screen.availWidth;", driverScope);
            var initalWidth =  Driver.ExecuteScript("return window.outerWidth;", driverScope);

            Assert.That(initalWidth, Is.LessThan(availWidth));

            Driver.MaximiseWindow(driverScope);

            Assert.That( Driver.ExecuteScript("return window.outerWidth;", driverScope), Is.GreaterThanOrEqualTo(availWidth));
        }



        [Fact]
        public void ResizesWindow()
        {
            using (Driver)
            {
                AssertResizesWindow(Root);
            }
        }

        [Fact]
        public void ResizesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);

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
            var availWidth = Driver.ExecuteScript("return window.screen.availWidth;", driverScope);
            var initalWidth = Driver.ExecuteScript("return window.outerWidth;", driverScope);

            Assert.That(initalWidth, Is.LessThan(availWidth));

            Driver.ResizeTo(new Size(768, 500), driverScope);

            Assert.That(Driver.ExecuteScript("return window.outerWidth;", driverScope), Is.EqualTo(768));
            Assert.That(Driver.ExecuteScript("return window.outerHeight;", driverScope), Is.EqualTo(500));
        }
    }
}