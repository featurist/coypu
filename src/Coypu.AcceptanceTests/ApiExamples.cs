using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Coypu.Drivers.Selenium;
using Coypu.Matchers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests
{
    /// <summary>
    /// Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    [TestFixture]
    public class Examples : WaitAndRetryExamples
    {
        [Test]
        public void AcceptModalDialog_example()
        {
            browser.ClickLink("Trigger an alert");
            Assert.IsTrue(browser.HasDialog("You have triggered an alert and this is the text."));

            browser.AcceptModalDialog();
            Assert.IsTrue(browser.HasNoDialog("You have triggered an alert and this is the text."));
        }

        [Test]
        public void CancelModalDialog_example()
        {
            browser.ClickLink("Trigger a confirm");
            browser.CancelModalDialog();
            browser.FindLink("Trigger a confirm - cancelled").Find();
        }

        [Test]
        public void ModalDialog_while_multiple_windows_are_open()
        {
            browser.ClickLink("Open pop up window");
            Thread.Sleep(1000);
            browser.ClickLink("Trigger a confirm");
            browser.CancelModalDialog();
            browser.FindLink("Trigger a confirm - cancelled").Find();
        }

        [Test]
        public void Check_example()
        {
            browser.Check("uncheckedBox");
            Assert.IsTrue(browser.FindField("uncheckedBox").Selected);
        }

        [Test]
        public void Uncheck_example()
        {
            browser.Uncheck("checkedBox");
            Assert.IsFalse(browser.Query(() => browser.FindField("checkedBox").Selected, false));
        }

        [Test]
        public void Can_find_checkbox_and_check_it()
        {
            var checkbox = browser.FindCss("#uncheckedBox");
            checkbox.Check();
            Assert.IsTrue(browser.FindField("uncheckedBox").Selected);
        }

        [Test]
        public void Can_find_checkbox_and_uncheck_it()
        {
            var checkbox = browser.FindCss("#checkedBox");
            checkbox.Uncheck();
            Assert.IsFalse(browser.Query(() => browser.FindField("checkedBox").Selected, false));
        }

        [Test]
        public void Choose_example()
        {
            browser.Choose("chooseRadio1");

            Assert.IsTrue(browser.FindField("chooseRadio1").Selected);

            browser.Choose("chooseRadio2");

            Assert.IsTrue(browser.FindField("chooseRadio2").Selected);
            Assert.IsFalse(browser.FindField("chooseRadio1").Selected);
        }

        [Test]
        public void Click_example()
        {
            var element = browser.FindButton("clickMeTest");
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me"));

            element.Click();
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButton_example()
        {
            browser.ClickButton("clickMeTest");
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickLink_example()
        {
            browser.ClickLink("Trigger a confirm");
            browser.CancelModalDialog();
        }

        [Test]
        public void ExecuteScript_example()
        {
            ReloadTestPage();
            Assert.That(browser.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;"),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void FillInWith_example()
        {
            browser.FillIn("scope2ContainerLabeledTextInputFieldId").With("New text input value");
            Assert.That(browser.FindField("scope2ContainerLabeledTextInputFieldId").Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void SendKeys_example()
        {
            browser.FindField("containerLabeledTextInputFieldId").SendKeys(" - send these keys");
            Assert.That(browser.FindField("containerLabeledTextInputFieldId").Value,
                        Is.EqualTo("text input field two val - send these keys"));
        }

        [Test]
        public void FillInWith_element_example()
        {
            browser.FillIn("scope2ContainerLabeledTextInputFieldId").With("New text input value");
            Assert.That(browser.FindField("scope2ContainerLabeledTextInputFieldId").Value,
                        Is.EqualTo("New text input value"));
        }

        [Test]
        public void FindAllCss_example()
        {
            ReloadTestPage();

            const string shouldFind = "#inspectingContent ul#cssTest li";
            var all = browser.FindAllCss(shouldFind).ToList();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1).Text, Is.EqualTo("two"));
            Assert.That(all.ElementAt(2).Text, Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindAllXPath_example()
        {
            ReloadTestPage();

            const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
            var all = browser.FindAllXPath(shouldFind).ToArray();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1).Text, Is.EqualTo("two"));
            Assert.That(all.ElementAt(2).Text, Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindButton_example()
        {
            Assert.That(browser.FindButton("Click me").Id, Is.EqualTo("clickMeTest"));
        }

        [Test]
        public void FindCss_example()
        {
            var first = browser.FindCss("#inspectingContent ul#cssTest li", new Options{Match = Match.First});
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindCss_with_text_example()
        {
            var two = browser.FindCss("#inspectingContent ul#cssTest li", text: "two");
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindCss_with_text_matching()
        {
            var two = browser.FindCss("#inspectingContent ul#cssTest li", text: new Regex("wo"));
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindXPath_example()
        {
            var first = browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li", new Options{Match = Match.First});
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindField_examples()
        {
            Assert.That(browser.FindField("text input field linked by for", new Options{Exact = true}).Id, Is.EqualTo("forLabeledTextInputFieldId"));
            Assert.That(browser.FindField("checkbox field in a label container").Id,
                        Is.EqualTo("containerLabeledCheckboxFieldId"));
            Assert.That(browser.FindField("containerLabeledSelectFieldId").Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
            Assert.That(browser.FindField("containerLabeledPasswordFieldName").Id,
                        Is.EqualTo("containerLabeledPasswordFieldId"));
        }

        [Test]
        public void FindFieldset_example()
        {
            Assert.That(browser.FindFieldset("Scope 1").Id, Is.EqualTo("fieldsetScope1"));
        }

        [Test]
        public void FindId_example()
        {
            Assert.That(browser.FindId("containerLabeledSelectFieldId").Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
        }

        [Test]
        public void FindLink_example()
        {
            Assert.That(browser.FindLink("Trigger an alert").Id, Is.EqualTo("alertTriggerLink"));
        }

        [Test]
        public void FindSection_example()
        {
            Assert.That(browser.FindSection("Inspecting Content").Id, Is.EqualTo("inspectingContent"));
            Assert.That(browser.FindSection("Div Section Two h2 with link").Id, Is.EqualTo("divSectionTwoWithLink"));
        }

        [Test]
        public void SelectFrom_example()
        {
            var textField = browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

            browser.Select("select2value2").From("containerLabeledSelectFieldId");

            textField = browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void Has_example()
        {
            Assert.IsTrue(browser.Has(browser.FindSection("Inspecting Content")));
            Assert.IsFalse(browser.Has(browser.FindCss("#no-such-element")));
        }

        [Test]
        public void HasNo_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"no-such-element\">asdf</div>'");
            Assert.IsTrue(browser.HasNo(browser.FindCss("#no-such-element")));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNo(browser.FindSection("Inspecting Content")));
        }

        [Test]
        public void HasContent_example()
        {
            Assert.That(browser, Shows.Content("This is what we are looking for"));
            Assert.That(browser.HasContent("This is not in the page"), Is.False);

            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.Content("This is not in the page")));
        }

        [Test]
        public void HasNoContent_example()
        {
            browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.That(browser, Shows.No.Content("This is not in the page"));

            ReloadTestPage();
            Assert.That(browser.HasNoContent("This is what we are looking for"), Is.False);

            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.No.Content("This is what we are looking for")));
        }

        [Test]
        public void HasContentMatch_example()
        {
            Assert.IsTrue(browser.HasContentMatch(new Regex("This is what (we are|I am) looking for")));
            Assert.IsFalse(browser.HasContentMatch(new Regex("This is ?n[o|']t in the page")));
        }

        [Test]
        public void HasNoContentMatch_example()
        {
            browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.IsTrue(browser.HasNoContentMatch(new Regex("This is ?n[o|']t in the page")));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoContentMatch(new Regex("This is what (we are|I am) looking for")));
        }

        [Test]
        public void HasValue_example()
        {
            var field = browser.FindField("find-this-field");
            Assert.That(field, Shows.Value("This value is what we are looking for"));
            Assert.IsFalse(field.HasValue("This is not the value"));
        }

        [Test]
        public void HasNoValue_example()
        {
            var field = browser.FindField("find-this-field");
            Assert.That(field, Shows.No.Value("This is not the value"));
            Assert.IsFalse(field.HasNoValue("This value is what we are looking for"));
        }

        [Test]
        public void ShowsAllCssInOrder_example()
        {
            Assert.That(browser, Shows.AllCssInOrder("#inspectingContent ul li", new[] { "Some", "text", "in", "a", "list","one","two","Me! Pick me!"}));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.AllCssInOrder("#inspectingContent ul li", new[] { "Some", "text", "in", "a", "list","two", "one","Me! Pick me!"})));
        }


        [Test]
        public void ShowsCssContaining_example()
        {
            Assert.That(browser, Shows.CssContaining("#inspectingContent ul li", "Some", "text","in","a","list"));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.CssContaining("#inspectingContent ul li", "missing","from","a","list")));
        }


        [Test]
        public void ShowsContentContaining_example()
        {
            Assert.That(browser, Shows.ContentContaining("Some", "text", "in", "a", "list"));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.ContentContaining("this is not in the page", "in", "a", "list")));
        }

        [Test]
        public void HasCss_example()
        {
            Assert.That(browser, Shows.Css("#inspectingContent ul#cssTest"));
            Assert.IsFalse(browser.HasCss("#inspectingContent ul#nope"));

            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.Css("#inspectingContent ul#nope")));
        }

        [Test]
        public void HasCss_with_text_example()
        {   
            Assert.That(browser, Shows.Css("#inspectingContent ul#cssTest li", text: "two"));
            Assert.False(browser.HasCss("#inspectingContent ul#cssTest li", text: "fifty"));

            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.Css("#inspectingContent ul#cssTest li", text: "Not this text")));
        }

        [Test]
        public void HasCss_with_text_matching_example()
        {
            Assert.That(browser, Shows.Css("#inspectingContent ul#cssTest li", text: new Regex("wo.*$")));
            Assert.False(browser.HasCss("#inspectingContent ul#cssTest li", text: new Regex("fifty")));

            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.Css("#inspectingContent ul#cssTest li", text: new Regex("Not this text"))));
        }

        [Test]
        public void HasNoCss_example()
        {
            browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");
            
            Assert.That(browser, Shows.No.Css("#inspectingContent ul#nope"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoCss("#inspectingContent ul#cssTest"));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.No.Css("#inspectingContent ul#cssTest")));
        }

        [Test]
        public void HasNoCss_with_text_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");

            Assert.That(browser, Shows.No.Css("li", text: "This is not in the page"));
        }

        [Test]
        public void HasNoCss_with_text_negative_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<ul><li>Some</li></ul>'");

            Assert.IsFalse(browser.HasNoCss("li", text: "Some"));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.No.Css("li", text: "Some")));
        }

        [Test]
        public void HasNoCss_with_text_matching_example()
        {
            browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");
            Assert.That(browser, Shows.No.Css("li", text: new Regex("not in the page$")));
        }

        [Test]
        public void HasNoCss_with_text_matching_negative_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<ul><li>Some</li></ul>'");

            Assert.IsFalse(browser.HasNoCss("li", text: new Regex("^Some$")));
            Assert.Throws<AssertionException>(() => Assert.That(browser, Shows.No.Css("li", new Regex("^Some$"))));
        }

        [Test]
        public void HasXPath_example()
        {
            Assert.IsTrue(browser.HasXPath("//*[@id='inspectingContent']//ul[@id='cssTest']"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasXPath("//*[@id='inspectingContent']//ul[@id='nope']"));
        }

        [Test]
        public void HasNoXpath_example()
        {
            browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");
            Assert.IsTrue(browser.HasNoXPath("//*[@id='inspectingContent']//ul[@id='nope']"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoXPath("//*[@id='inspectingContent']//ul[@id='cssTest']"));
        }

        [Test]
        public void Hover_example()
        {
            Assert.That(browser.FindId("hoverOnMeTest").Text, Is.EqualTo("Hover on me"));
            browser.FindId("hoverOnMeTest").Hover();
            Assert.That(browser.FindId("hoverOnMeTest").Text, Is.EqualTo("Hover on me - hovered"));
        }

        [Test]
        public void Native_example()
        {
            var button = (IWebElement) browser.FindButton("clickMeTest").Native;
            button.Click();
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void Title_example()
        {
            Assert.That(browser.Title, Is.EqualTo("Coypu interaction tests page"));
        }

        [Test]
        public void Within_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";

            var expectingScope1 = browser.FindId("scope1").FindField(locatorThatAppearsInMultipleScopes);
            var expectingScope2 = browser.FindId("scope2").FindField(locatorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("scope1TextInputFieldId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("scope2TextInputFieldId"));
        }

        [Test]
        public void WithinFieldset_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";

            var expectingScope1 = browser.FindFieldset("Scope 1")
                                         .FindField(locatorThatAppearsInMultipleScopes);

            var expectingScope2 = browser.FindFieldset("Scope 2")
                                         .FindField(locatorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("scope1TextInputFieldId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("scope2TextInputFieldId"));
        }

        [Test]
        public void WithinSection_example()
        {
            const string selectorThatAppearsInMultipleScopes = "h2";

            var expectingScope1 = browser.FindSection("Section One h1").FindCss(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = browser.FindSection("Div Section Two h1").FindCss(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Text, Is.EqualTo("Section One h2"));
            Assert.That(expectingScope2.Text, Is.EqualTo("Div Section Two h2"));
        }

        [Test]
        public void TryUntil_example()
        {
            var tryThisButton = browser.FindButton("try this");
            Assert.That(tryThisButton.Exists());
            browser.TryUntil(() => tryThisButton.Click(),
                             () => browser.HasContent("try until 5"),
                             TimeSpan.FromMilliseconds(50),
                             new Options {Timeout = TimeSpan.FromMilliseconds(5000)});
        }

        [Test]
        public void WithinIFrame_example()
        {
            const string selectorThatAppearsInMultipleScopes = "scoped button";

            var expectingScope1 = browser.FindFrame("iframe1").FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = browser.FindFrame("iframe2").FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("iframe1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("iframe2ButtonId"));
        }

        [Test]
        public void WithinIFrame_FoundByCss_example()
        {
            const string selectorThatAppearsInMultipleScopes = "scoped button";

            var expectingScope1 = browser.FindCss("iframe#iframe1").FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = browser.FindCss("iframe#iframe2").FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("iframe1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("iframe2ButtonId"));
        }

        [Test]
        public void WithinFrame_example()
        {
            browser.Visit(TestPageLocation("frameset.htm"));

            const string selectorThatAppearsInMultipleScopes = "scoped button";

            var expectingScope1 = browser.FindFrame("frame1").FindButton(selectorThatAppearsInMultipleScopes);
            var expectingScope2 = browser.FindFrame("frame2").FindButton(selectorThatAppearsInMultipleScopes);

            Assert.That(expectingScope1.Id, Is.EqualTo("frame1ButtonId"));
            Assert.That(expectingScope2.Id, Is.EqualTo("frame2ButtonId"));
        }

        [Test]
        public void Multiple_interactions_within_iframe_example()
        {
            var iframe = browser.FindFrame("I am iframe one");
            iframe.FillIn("text input in iframe").With("filled in");
            Assert.That(iframe.FindField("text input in iframe").Value, Is.EqualTo("filled in"));
        }

        [Test]
        public void FillIn_file_example()
        {
            const string someLocalFile = @"local.file";
            try
            {
                var directoryInfo = new DirectoryInfo(".");
                var fullPath = Path.Combine(directoryInfo.FullName, someLocalFile);
                using (File.Create(fullPath))
                {
                }

                browser.FillIn("forLabeledFileFieldId").With(fullPath);

                var findAgain = browser.FindField("forLabeledFileFieldId");
                Assert.That(findAgain.Value, Is.StringEnding("\\" + someLocalFile));
            }
            finally
            {
                File.Delete(someLocalFile);
            }
        }

        [Test]
        public void ConsideringInvisibleElements()
        {
            browser.FindButton("firstInvisibleInputId", new Options {ConsiderInvisibleElements = true}).Find();
        }

        [Test]
        public void ConsideringOnlyVisibleElements()
        {
            Assert.Throws<MissingHtmlException>(() => browser.FindButton("firstInvisibleInputId").Find());
        }

        [Test]
        public void WindowScoping_example()
        {
            var mainWindow = browser;

            mainWindow.ClickLink("Open pop up window");

            var popUp = mainWindow.FindWindow("Pop Up Window");

            Assert.That(mainWindow.FindButton("scoped button", new Options { Match = Match.First }).Id, Is.EqualTo("scope1ButtonId"));
            Assert.That(popUp.FindButton("scoped button").Id, Is.EqualTo("popUpButtonId"));

            // And back
            Assert.That(mainWindow.FindButton("scoped button", new Options { Match = Match.First }).Id, Is.EqualTo("scope1ButtonId"));
        }

        [Test]
        public void MaximiseWindow()
        {
            var availWidth = float.Parse(browser.ExecuteScript("return window.screen.availWidth;"));
            var initalWidth = GetOuterWidth();

            Assert.That(initalWidth, Is.LessThan(availWidth));

            browser.MaximiseWindow();

            Assert.That(GetOuterWidth(), Is.GreaterThanOrEqualTo(availWidth));
        }

        [Test]
        public void ResizeWindow()
        {
            var initalWidth = GetOuterWidth();
            var initialHeight = GetOuterHeight();

            Assert.That(initalWidth, Is.Not.EqualTo(500));
            Assert.That(initialHeight, Is.Not.EqualTo(600));

            browser.ResizeTo(500, 600);

            Assert.That(GetOuterWidth(), Is.EqualTo(500));
            Assert.That(GetOuterHeight(), Is.EqualTo(600));
        }

        private float GetOuterHeight()
        {
            return float.Parse(browser.ExecuteScript("return window.outerHeight;"));
        }

        private float GetOuterWidth()
        {
            return float.Parse(browser.ExecuteScript("return window.outerWidth;"));
        }

        [Test]
        public void RefreshingWindow()
        {
             var tickBeforeRefresh = Int64.Parse(browser.ExecuteScript("return window.SpecData.CurrentTick;"));
 
             browser.Refresh();
 
             var tickAfterRefresh = Int64.Parse(browser.ExecuteScript("return window.SpecData.CurrentTick;"));
 
             Assert.That((tickAfterRefresh - tickBeforeRefresh), Is.GreaterThan(0));
        }

        [Test]
        public void CustomProfile()
        {
            var configuration = new SessionConfiguration {Driver = typeof (CustomFirefoxProfileSeleniumWebDriver)};

            using (var custom = new BrowserSession(configuration))
            {
                custom.Visit("https://www.relishapp.com/");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo("0"));
            }
        }

        public class CustomFirefoxProfileSeleniumWebDriver : SeleniumWebDriver
        {
            public CustomFirefoxProfileSeleniumWebDriver(Drivers.Browser browser)
                : base(CustomProfile(), browser)
            {
            }

            private static RemoteWebDriver CustomProfile()
            {
                var yourCustomProfile = new FirefoxProfile();
                return new FirefoxDriver(yourCustomProfile);
            }
        }
    }
}