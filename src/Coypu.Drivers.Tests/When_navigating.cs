using System;
using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_location : DriverSpecs
    {

        [Test]
        public void Go_back_and_forward_in_history()
        {
            using (Driver)
            {
                Driver.Visit("http://localhost:4567", Root);
                Driver.Visit("http://localhost:4567/auto_login", Root);

                Driver.GoBack(Root);
                Assert.That(Driver.Location(Root), Is.EqualTo(new Uri("http://localhost:4567/")));

                Driver.GoForward(Root);
                Assert.That(Driver.Location(Root), Is.EqualTo(new Uri("http://localhost:4567/auto_login")));

            }
        }

        [Test]
        public void Go_back_and_forward_in_correct_window_scope()
        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Open pop up window", Root));
                var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root),
                                            Driver, null, null, null);

                Driver.Visit("http://localhost:4567/auto_login", Root);
                Driver.Visit("http://localhost:4567", popUp);

                Driver.GoBack(popUp);
                Assert.That(Driver.Location(popUp).AbsoluteUri,
                            Is.StringEnding("src/Coypu.Drivers.Tests/html/popup.htm"));
                Assert.That(Driver.Location(Root).AbsoluteUri, Is.EqualTo("http://localhost:4567/auto_login"));

                Driver.GoForward(popUp);
                Assert.That(Driver.Location(popUp).AbsoluteUri, Is.EqualTo("http://localhost:4567/"));

                Driver.GoBack(Root);
                Assert.That(Driver.Location(Root).AbsoluteUri, Is.StringEnding("/html/InteractionTestsPage.htm"));
                Assert.That(Driver.Location(popUp).AbsoluteUri, Is.EqualTo("http://localhost:4567/"));
            }
        }
    }
}