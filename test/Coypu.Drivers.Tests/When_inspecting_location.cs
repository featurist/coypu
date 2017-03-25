using System;
using System.Threading;
using Coypu.Finders;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_navigating : DriverSpecs
    {
        [Fact]
        public void Gets_the_current_browser_location()
        {
            Driver.Visit(TestSiteUrl("/"), Root);
            Assert.Equal(new Uri(TestSiteUrl("/")), Driver.Location(Root));

            Driver.Visit(TestSiteUrl("/auto_login"), Root);
            Assert.Equal(new Uri(TestSiteUrl("/auto_login")), Driver.Location(Root));
        }


        [Fact]
        public void Gets_location_for_correct_window_scope()
        {
            Driver.Click(Link("Open pop up window"));
            var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);

            Assert.EndsWith("src/Coypu.Drivers.Tests/html/popup.htm", Driver.Location(popUp).AbsoluteUri);
        }


        [Fact]
        public void Not_just_when_set_by_visit()
        {
            Driver.Visit(TestSiteUrl("/auto_login"), Root);
            Driver.ExecuteScript("document.location.href = '" + TestSiteUrl("/resource/bdd") + "'", Root);

            // Seems like WebDriver is not waiting on JS, has exec been made asnyc?
            Thread.Sleep(500);

            Assert.Equal(new Uri(TestSiteUrl("/resource/bdd")), Driver.Location(Root));
        }
    }
}