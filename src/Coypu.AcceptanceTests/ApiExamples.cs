using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using Coypu.NUnit.Matchers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests
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
            public CustomFirefoxProfileSeleniumWebDriver(Browser browser)
                : base(CustomOptions(), browser) { }

            private static RemoteWebDriver CustomOptions()
            {
                return new FirefoxDriver(new FirefoxOptions());
            }
        }

        [TestCase("Windows 7", "firefox", "25")]
        [TestCase("Windows 10", "MicrosoftEdge", "14")]
        public void CustomBrowserSessionWithCustomRemoteDriver(string platform,
                                                               string browserName,
                                                               string version)
        {
            var desiredCapabilites = new DesiredCapabilities(browserName, version, Platform.CurrentPlatform);
            desiredCapabilites.SetCapability("platform", platform);
            desiredCapabilites.SetCapability("username", "appiumci");
            desiredCapabilites.SetCapability("accessKey", "af4fbd21-6aee-4a01-857f-c7ffba2f0a50");
            desiredCapabilites.SetCapability("name", TestContext.CurrentContext.Test.Name);

            IDriver driver = new CustomRemoteDriver(Drivers.Browser.Parse(browserName), desiredCapabilites);

            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        [TestCase("chrome")]
        [TestCase("internet explorer")]
        [TestCase("MicrosoftEdge")]
        public void CustomBrowserSession(string browserName)
        {
            var driver = new SeleniumWebDriver(Drivers.Browser.Parse(browserName));
            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        public class CustomRemoteDriver : SeleniumWebDriver
        {
            public CustomRemoteDriver(Browser browser,
                                      ICapabilities capabilities)
                : base(CustomWebDriver(capabilities), browser) { }

            private static RemoteWebDriver CustomWebDriver(ICapabilities capabilities)
            {
                var remoteAppHost = new Uri("http://ondemand.saucelabs.com:80/wd/hub");
                return new RemoteWebDriver(remoteAppHost, capabilities);
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
        public void FindAllCss_example()
        {
            ReloadTestPage();

            const string shouldFind = "#inspectingContent ul#cssTest li";
            var all = Browser.FindAllCss(shouldFind)
                             .ToList();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1)
                           .Text,
                        Is.EqualTo("two"));
            Assert.That(all.ElementAt(2)
                           .Text,
                        Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindAllXPath_example()
        {
            ReloadTestPage();

            const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
            var all = Browser.FindAllXPath(shouldFind)
                             .ToArray();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1)
                           .Text,
                        Is.EqualTo("two"));
            Assert.That(all.ElementAt(2)
                           .Text,
                        Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindButton_example()
        {
            Assert.That(Browser.FindButton("Click me")
                               .Id,
                        Is.EqualTo("clickMeTest"));
        }

        [Test]
        public void FindCss_example()
        {
            var first = Browser.FindCss("#inspectingContent ul#cssTest li", Options.First);
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindCss_with_text_example()
        {
            var two = Browser.FindCss("#inspectingContent ul#cssTest li", "two");
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindCss_with_text_matching()
        {
            var two = Browser.FindCss("#inspectingContent ul#cssTest li", new Regex("wo"));
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindField_examples()
        {
            Assert.That(Browser.FindField("text input field linked by for", Options.Exact)
                               .Id,
                        Is.EqualTo("forLabeledTextInputFieldId"));
            Assert.That(Browser.FindField("checkbox field in a label container")
                               .Id,
                        Is.EqualTo("containerLabeledCheckboxFieldId"));
            Assert.That(Browser.FindField("containerLabeledSelectFieldId")
                               .Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
            Assert.That(Browser.FindField("containerLabeledPasswordFieldName")
                               .Id,
                        Is.EqualTo("containerLabeledPasswordFieldId"));
        }

        [Test]
        public void FindFieldset_example()
        {
            Assert.That(Browser.FindFieldset("Scope 1")
                               .Id,
                        Is.EqualTo("fieldsetScope1"));
        }

        [Test]
        public void FindId_example()
        {
            Assert.That(Browser.FindId("containerLabeledSelectFieldId")
                               .Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
        }

        [Test]
        public void FindIdEndingWith_example()
        {
            Assert.That(Browser.FindIdEndingWith("aspWebFormsContainerLabeledFileFieldId")
                               .Id,
                        Is.EqualTo("_ctrl01_ctrl02_aspWebFormsContainerLabeledFileFieldId"));
        }

        [Test]
        public void FindLink_example()
        {
            Assert.That(Browser.FindLink("Trigger an alert")
                               .Id,
                        Is.EqualTo("alertTriggerLink"));
        }

        [Test]
        public void Finds_link_by_href()
        {
            Assert.That(Browser.FindLink("#link1href")
                               .Id,
                        Is.EqualTo("firstLinkId"));
            Assert.That(Browser.FindLink("#link2href")
                               .Id,
                        Is.EqualTo("secondLinkId"));
        }

        [Test]
        public void FindSection_example()
        {
            Assert.That(Browser.FindSection("Inspecting Content")
                               .Id,
                        Is.EqualTo("inspectingContent"));
            Assert.That(Browser.FindSection("Div Section Two h2 with link")
                               .Id,
                        Is.EqualTo("divSectionTwoWithLink"));
        }

        [Test]
        public void FindXPath_example()
        {
            var first = Browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li", Options.First);
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindXPath_with_text_example()
        {
            var two = Browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li", "two");
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void HasContent_example()
        {
            Assert.That(Browser, Shows.Content("This is what we are looking for"));
            Assert.That(Browser.HasContent("This is not in the page"), Is.False);

            Assert.Throws<AssertionException>(() => Assert.That(Browser, Shows.Content("This is not in the page")));
        }

        [Test]
        public void HasContentMatch_example()
        {
            Assert.IsTrue(Browser.HasContentMatch(new Regex("This is what (we are|I am) looking for")));
            Assert.IsFalse(Browser.HasContentMatch(new Regex("This is ?n[o|']t in the page")));
        }

        [Test]
        public void HasContentMatching_example()
        {
            Assert.That(Browser, Shows.Content(new Regex(@"This.is.what.we.are.looking.for")));
            Assert.That(Browser.HasContentMatch(new Regex(@"This.is.not.in.the.page")), Is.False);

            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.Content(new Regex(@"This.is.not.in.the.page"))));
        }

        [Test]
        public void HasNoContent_example()
        {
            Browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.That(Browser, Shows.No.Content("This is not in the page"));

            ReloadTestPage();
            Assert.That(Browser.HasNoContent("This is what we are looking for"), Is.False);

            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.No.Content("This is what we are looking for")));
        }

        [Test]
        public void HasNoContentMatch_example()
        {
            Browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.IsTrue(Browser.HasNoContentMatch(new Regex("This is ?n[o|']t in the page")));

            ReloadTestPage();
            Assert.IsFalse(Browser.HasNoContentMatch(new Regex("This is what (we are|I am) looking for")));
        }

        [Test]
        public void HasNoValue_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field, Shows.No.Value("This is not the value"));
            Assert.IsFalse(field.HasNoValue("This value is what we are looking for"));
        }

        [Test]
        public void HasValue_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field, Shows.Value("This value is what we are looking for"));
            Assert.IsFalse(field.HasValue("This is not the value"));
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

        [Test]
        public void Within_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";
            var expectingScope1 = Browser.FindId("scope1")
                                         .FindField(locatorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindId("scope2")
                                         .FindField(locatorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("scope1TextInputFieldId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("scope2TextInputFieldId"));
        }

        [Test]
        public void WithinFieldset_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";
            var expectingScope1 = Browser.FindFieldset("Scope 1")
                                         .FindField(locatorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindFieldset("Scope 2")
                                         .FindField(locatorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("scope1TextInputFieldId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("scope2TextInputFieldId"));
        }

        [Test]
        public void WithinFrame_example()
        {
            Browser.Visit(TestPageLocation("frameset.htm"));
            const string selectorThatAppearsInMultipleScopes = "scoped button";
            var expectingScope1 = Browser.FindFrame("frame1")
                                         .FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindFrame("frame2")
                                         .FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("frame1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("frame2ButtonId"));
        }

        [Test]
        public void WithinIFrame_example()
        {
            const string selectorThatAppearsInMultipleScopes = "scoped button";
            var expectingScope1 = Browser.FindFrame("iframe1")
                                         .FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindCss("#iframe2")
                                         .FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("iframe1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("iframe2ButtonId"));
        }

        [Test]
        public void WithinIFrame_FoundByCss_example()
        {
            const string selectorThatAppearsInMultipleScopes = "scoped button";
            var expectingScope1 = Browser.FindCss("iframe#iframe1")
                                         .FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindCss("iframe#iframe2")
                                         .FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("iframe1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("iframe2ButtonId"));
        }

        [Test]
        public void WithinSection_example()
        {
            const string selectorThatAppearsInMultipleScopes = "h2";
            var expectingScope1 = Browser.FindSection("Section One h1")
                                         .FindCss(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = Browser.FindSection("Div Section Two h1")
                                         .FindCss(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Text, Is.EqualTo("Section One h2"));
            Assert.That(expectingScope2.Text, Is.EqualTo("Div Section Two h2"));
        }
    }
}