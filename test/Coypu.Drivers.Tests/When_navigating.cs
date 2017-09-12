using System;
using Coypu.Finders;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_inspecting_location : DriverSpecs
    {
        [Fact]
        public void Go_back_and_forward_in_history()
        {
            using (Driver)
            {
                Driver.Visit(TestSiteUrl("/"), Root);
                Driver.Visit(TestSiteUrl("/auto_login"), Root);

                Driver.GoBack(Root);
                Assert.Equal(new Uri(TestSiteUrl("/")), Driver.Location(Root));

                Driver.GoForward(Root);
                Assert.Equal(new Uri(TestSiteUrl("/auto_login")), Driver.Location(Root));

            }
        }

        [Fact]
        public void Go_back_and_forward_in_correct_window_scope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);

                Driver.Visit(TestSiteUrl("/auto_login"), Root);
                Driver.Visit(TestSiteUrl("/"), popUp);

                Driver.GoBack(popUp);
                Assert.EndsWith("src/Coypu.Drivers.Tests/html/popup.htm", Driver.Location(popUp).AbsoluteUri);
                Assert.Equal(TestSiteUrl("/auto_login"), Driver.Location(Root).AbsoluteUri);

                Driver.GoForward(popUp);
                Assert.Equal(TestSiteUrl("/"), Driver.Location(popUp).AbsoluteUri);

                Driver.GoBack(Root);
                Assert.EndsWith("/html/InteractionTestsPage.htm", Driver.Location(Root).AbsoluteUri);
                Assert.Equal(TestSiteUrl("/"), Driver.Location(popUp).AbsoluteUri);
            }
        }
    }
}