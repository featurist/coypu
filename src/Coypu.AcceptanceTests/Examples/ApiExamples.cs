using System;
using System.IO;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using Coypu.NUnit.Matchers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests.Examples
{
    /// <summary>
    ///     Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    [TestFixture]
    public class ApiExamples : WaitAndRetryExamples
    {
        private object GetOuterHeight()
        {
            return Browser.ExecuteScript("return window.outerHeight;");
        }

        private object GetOuterWidth()
        {
            return Browser.ExecuteScript("return window.outerWidth;");
        }

        public class CustomFirefoxProfileSeleniumWebDriver : SeleniumWebDriver
        {
            public CustomFirefoxProfileSeleniumWebDriver(Browser browser) : base(CustomOptions(), browser) { }

            private static RemoteWebDriver CustomOptions()
            {
                return new FirefoxDriver(new FirefoxOptions());
            }
        }

        [Test]
        public void AcceptModalDialog_example()
        {
            Browser.ClickLink("Trigger an alert");
            Assert.IsTrue(Browser.HasDialog("You have triggered an alert and this is the text."));

            Browser.AcceptModalDialog();
            Assert.IsTrue(Browser.HasNoDialog("You have triggered an alert and this is the text."));
        }

        [Test]
        public void Attributes_on_stale_scope_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field.Value, Is.EqualTo("This value is what we are looking for"));

            ReloadTestPage();
            Assert.That(field.Value, Is.EqualTo("This value is what we are looking for"));
            Assert.That(field.Id, Is.EqualTo("find-this-field"));
            Assert.That(field["id"], Is.EqualTo("find-this-field"));
        }

        [Test]
        public void Can_find_checkbox_and_check_it()
        {
            var checkbox = Browser.FindCss("#uncheckedBox");
            checkbox.Check();
            Assert.IsTrue(Browser.FindField("uncheckedBox")
                                 .Selected);
        }

        [Test]
        public void Can_find_checkbox_and_uncheck_it()
        {
            var checkbox = Browser.FindCss("#checkedBox");
            checkbox.Uncheck();
            Assert.IsFalse(Browser.Query(() => Browser.FindField("checkedBox")
                                                      .Selected,
                                         false));
        }

        [Test]
        public void CancelModalDialog_example()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }

        [Test]
        public void Check_example()
        {
            Browser.Check("uncheckedBox");
            Assert.IsTrue(Browser.FindField("uncheckedBox")
                                 .Selected);
        }

        [Test]
        public void Choose_example()
        {
            Browser.Choose("chooseRadio1");
            Assert.IsTrue(Browser.FindField("chooseRadio1")
                                 .Selected);

            Browser.Choose("chooseRadio2");
            Assert.IsTrue(Browser.FindField("chooseRadio2")
                                 .Selected);
            Assert.IsFalse(Browser.FindField("chooseRadio1")
                                  .Selected);
        }

        [Test]
        public void Click_example()
        {
            var element = Browser.FindButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me"));

            element.Click();
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButton_example()
        {
            Browser.ClickButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickLink_example()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ClickLinkWithTitle_example()
        {
            Browser.ClickLink("Link with title");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ConsideringInvisibleElements()
        {
            Browser.FindButton("firstInvisibleInputId", new Options {ConsiderInvisibleElements = true})
                   .Now();
        }

        [Test]
        public void ConsideringOnlyVisibleElements()
        {
            Assert.Throws<MissingHtmlException>(() => Browser.FindButton("firstInvisibleInputId")
                                                             .Now());
        }

        [Test]
        public void CustomProfile()
        {
            var configuration = new SessionConfiguration {Driver = typeof(CustomFirefoxProfileSeleniumWebDriver)};
            using (var custom = new BrowserSession(configuration))
            {
                custom.Visit("https://www.relishapp.com/");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        [Test]
        public void DisabledButton_example()
        {
            Assert.That(Browser.FindButton("Disabled button")
                               .Disabled,
                        Is.True,
                        "Expected button to be disabled");
            Assert.That(Browser.FindButton("Click me")
                               .Disabled,
                        Is.False,
                        "Expected button to be enabled");
        }

        [Test]
        public void ExecuteScript_example()
        {
            ReloadTestPage();
            Assert.That(Browser.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;"),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void ExecuteScriptWithArgs_example()
        {
            ReloadTestPage();
            Assert.That(Browser.ExecuteScript("return arguments[0].innerHTML;", Browser.FindId("firstButtonId")),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void FillIn_file_example()
        {
            const string someLocalFile = @"local.file";
            try
            {
                var directoryInfo = new DirectoryInfo(Path.GetTempPath());
                var fullPath = Path.Combine(directoryInfo.FullName, someLocalFile);
                using (File.Create(fullPath)) { }

                Browser.FillIn("forLabeledFileFieldId")
                       .With(fullPath);
                var findAgain = Browser.FindField("forLabeledFileFieldId");
                Assert.That(findAgain.Value, Does.EndWith(someLocalFile));
            }
            finally
            {
                File.Delete(someLocalFile);
            }
        }

        [Test]
        public void FillInWith_element_example()
        {
            Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                   .FillInWith("New text input value");
            Assert.That(Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void FillInWith_example()
        {
            Browser.FillIn("scope2ContainerLabeledTextInputFieldId")
                   .With("New text input value");
            Assert.That(Browser.FindField("scope2ContainerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void Hover_example()
        {
            Assert.That(Browser.FindId("hoverOnMeTest")
                               .Text,
                        Is.EqualTo("Hover on me"));
            Browser.FindId("hoverOnMeTest")
                   .Hover();
            Assert.That(Browser.FindId("hoverOnMeTest")
                               .Text,
                        Is.EqualTo("Hover on me - hovered"));
        }

        [Test]
        public void MaximiseWindow()
        {
            var availWidth = Browser.ExecuteScript("return window.screen.availWidth;");
            var initalWidth = GetOuterWidth();
            Assert.That(initalWidth, Is.LessThan(availWidth));

            Browser.MaximiseWindow();
            Assert.That(GetOuterWidth(), Is.GreaterThanOrEqualTo(availWidth));
        }

        [Test]
        public void ModalDialog_while_multiple_windows_are_open()
        {
            Browser.ClickLink("Open pop up window");
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }

        [Test]
        public void Multiple_interactions_within_iframe_example()
        {
            var iframe = Browser.FindFrame("I am iframe one");
            iframe.FillIn("text input in iframe")
                  .With("filled in");
            Assert.That(iframe.FindField("text input in iframe")
                              .Value,
                        Is.EqualTo("filled in"));
        }

        [Test]
        public void Native_example()
        {
            var button = (IWebElement) Browser.FindButton("clickMeTest")
                                              .Native;
            button.Click();
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void RefreshingWindow()
        {
            var tickBeforeRefresh = (long) Browser.ExecuteScript("return window.SpecData.CurrentTick;");
            Browser.Refresh();
            var tickAfterRefresh = (long) Browser.ExecuteScript("return window.SpecData.CurrentTick;");
            Assert.That(tickAfterRefresh - tickBeforeRefresh, Is.GreaterThan(0));
        }

        [Test]
        public void ResizeWindow()
        {
            var initalWidth = GetOuterWidth();
            var initialHeight = GetOuterHeight();
            Assert.That(initalWidth, Is.Not.EqualTo(500));
            Assert.That(initialHeight, Is.Not.EqualTo(600));

            Browser.ResizeTo(500, 600);
            Assert.That(GetOuterWidth(), Is.EqualTo(500));
            Assert.That(GetOuterHeight(), Is.EqualTo(600));
        }

        [Test]
        public void SelectFrom_element_example()
        {
            var field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option one"));

            field.SelectOption("select two option two");
            field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void SelectFrom_example()
        {
            var textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

            Browser.Select("select2value2")
                   .From("containerLabeledSelectFieldId");
            textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void SendKeys_example()
        {
            Browser.FindField("containerLabeledTextInputFieldId")
                   .SendKeys(" - send these keys");
            Assert.That(Browser.FindField("containerLabeledTextInputFieldId")
                               .Value,
                        Is.EqualTo("text input field two val - send these keys"));
        }

        [Test]
        public void ShowsAllCssInOrder_example()
        {
            Assert.That(Browser,
                        Shows.AllCssInOrder("#inspectingContent ul li",
                                            "Some",
                                            "text",
                                            "in",
                                            "a",
                                            "list",
                                            "one",
                                            "two",
                                            "Me! Pick me!"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.AllCssInOrder("#inspectingContent ul li",
                                                                                    "Some",
                                                                                    "text",
                                                                                    "in",
                                                                                    "a",
                                                                                    "list",
                                                                                    "two",
                                                                                    "one",
                                                                                    "Me! Pick me!")));
        }


        [Test]
        public void ShowsContentContaining_example()
        {
            Assert.That(Browser, Shows.ContentContaining("Some", "text", "in", "a", "list"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.ContentContaining("this is not in the page",
                                                                                        "in",
                                                                                        "a",
                                                                                        "list")));
        }


        [Test]
        public void ShowsCssContaining_example()
        {
            Assert.That(Browser, Shows.CssContaining("#inspectingContent ul li", "Some", "text", "in", "a", "list"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.CssContaining("#inspectingContent ul li",
                                                                                    "missing",
                                                                                    "from",
                                                                                    "a",
                                                                                    "list")));
        }

        [Test]
        public void Title_example()
        {
            Assert.That(Browser.Title, Is.EqualTo("Coypu interaction tests page"));
        }

        [Test]
        public void TryUntil_example()
        {
            var tryThisButton = Browser.FindButton("try this");
            Assert.That(tryThisButton.Exists());
            Browser.TryUntil(() => tryThisButton.Click(),
                             () => Browser.HasContent("try until 5"),
                             TimeSpan.FromMilliseconds(50),
                             new Options {Timeout = TimeSpan.FromMilliseconds(10000)});
        }

        [Test]
        public void Uncheck_example()
        {
            Browser.Uncheck("checkedBox");
            Assert.IsFalse(Browser.Query(() => Browser.FindField("checkedBox")
                                                      .Selected,
                                         false));
        }

        [Test]
        public void WindowScoping_example()
        {
            var mainWindow = Browser;
            Assert.That(mainWindow.FindButton("scoped button", Options.First)
                                  .Id,
                        Is.EqualTo("scope1ButtonId"));
            mainWindow.ExecuteScript("setTimeout(function() {document.getElementById(\"openPopupLink\").click();}), 3000");
            var popUp = mainWindow.FindWindow("Pop Up Window");
            Assert.That(popUp.FindButton("scoped button")
                             .Id,
                        Is.EqualTo("popUpButtonId"));
            // And back
            Assert.That(mainWindow.FindButton("scoped button", Options.First)
                                  .Id,
                        Is.EqualTo("scope1ButtonId"));
        }
    }
}