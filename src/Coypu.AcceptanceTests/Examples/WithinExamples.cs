using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class WithinExamples : WaitAndRetryExamples
    {
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