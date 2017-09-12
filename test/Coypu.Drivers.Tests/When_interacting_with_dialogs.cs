using Coypu.Finders;
using Shouldly;
using System;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_interacting_with_dialogs : DriverSpecs
    {
        [Fact]
        public void Accepts_alerts()
        {
            using (Driver)
            {
                Driver.Click(Link("Trigger an alert"));
                Driver.HasDialog("You have triggered an alert and this is the text.", Root).ShouldBeTrue();
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered an alert and this is the text.", Root).ShouldBeFalse();
            }
        }


        [Fact]
        public void Clears_dialog()
        {
            using (Driver)
            {
                Driver.Click(Link("Trigger a confirm"));
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).ShouldBeTrue();
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).ShouldBeFalse();
            }
        }

        [Fact]
        public void Missing_dialog_throws_coypu_exception()
        {
            using (Driver)
            {
                Assert.Throws<MissingDialogException>(() => Driver.AcceptModalDialog(Root));
                Assert.Throws<MissingDialogException>(() => Driver.CancelModalDialog(Root));
            }
        }

        [Fact]
        public void Returns_true()
        {
            using (Driver)
            {
                Driver.Click(Link("Trigger a confirm"));
                Driver.AcceptModalDialog(Root);
                Link("Trigger a confirm - accepted", Root).ShouldNotBeNull();
            }
        }


        [Fact]
        public void Cancel_Clears_dialog()
        {
            using (Driver)
            {
                Driver.Click(Link("Trigger a confirm"));
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).ShouldBeTrue();
                Driver.CancelModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).ShouldBeFalse();
            }
        }

        [Fact]
        public void Cancel_Returns_false()
        {
            using (Driver)
            {
                Driver.Click(Link("Trigger a confirm"));
                Driver.CancelModalDialog(Root);

                Link("Trigger a confirm - cancelled");
            }
        }

        // IE can't do this
        [Fact]
        public void Finds_scope_first_for_alerts()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
                Assert.Equal("Pop Up Window", Driver.Title(popUp));

                Driver.ExecuteScript("window.setTimeout(function() {document.getElementById('alertTriggerLink').click();},200);", Root);
                Assert.Equal("Pop Up Window", Driver.Title(popUp));

                System.Threading.Thread.Sleep(1000);
                Driver.AcceptModalDialog(Root);
                Driver.HasDialog("You have triggered a alert and this is the text.", Root).ShouldBeFalse();
            }
        }

        // IE can't do this
        [Fact]
        public void Finds_scope_first_for_confirms()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
                Assert.Equal("Pop Up Window", Driver.Title(popUp));

                Driver.ExecuteScript("window.setTimeout(function() {document.getElementById('confirmTriggerLink').click();},500);", Root);
                Assert.Equal("Pop Up Window", Driver.Title(popUp));
                CloseWindow(popUp);

                System.Threading.Thread.Sleep(500);
                Driver.CancelModalDialog(Root);
                Driver.HasDialog("You have triggered a confirm and this is the text.", Root).ShouldBeFalse();
            }
        }

        private static void CloseWindow(DriverScope popUp)
        {
            try
            {
                Driver.ExecuteScript("self.close();", popUp);
            }
            catch (Exception)
            {
                // IE permissions
            }
        }
    }
}