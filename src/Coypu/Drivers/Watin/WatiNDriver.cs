using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Constraints;
using WatiN.Core.Exceptions;
using WatiN.Core.Interfaces;

namespace Coypu.Drivers.Watin
{
    public class WatiNDriver : Driver
    {
        readonly string[] sectionTagNames = new[] { "SECTION", "DIV" };
        readonly string[] headerTagNames = new[] { "H1", "H2", "H3", "H4", "H5", "H6" };
        public bool Disposed { get; private set; }
        private WatiN.Core.Browser watinInstance;

        private WatiN.Core.Browser Watin 
        { 
            get 
            { 
                return watinInstance ?? (watinInstance = NewDriver());
            }
        }

        private WatiN.Core.Browser NewDriver()
        {
            switch (Configuration.Browser)
            {
                case (Browser.InternetExplorer):
                    return new IE();
                default:
                    throw new BrowserNotSupportedException(Configuration.Browser, this.GetType());
            }
        }

        public void SetScope(Func<Element> find)
        {
            Scope = (WatiN.Core.Element)find().Native;
        }

        private WatiN.Core.Element Scope { get; set; }

        public void ClearScope()
        {
            Scope = null;
        }

        public string ExecuteScript(string javascript)
        {
            var stripReturn = Regex.Replace(javascript, @"^\s*return ", "");
            return Watin.Eval(stripReturn);
        }

        public Element FindFieldset(string locator)
        {
            var fieldsets = Watin.Elements.Filter(IsFieldset);
            var fieldset =
                FindFirst(fieldsets, Find.ById(locator)) ??
                FindFieldsetByLegend(locator);

            return BuildElement(fieldset, "Failed to find fieldset: " + locator);
        }

        private WatiN.Core.Element FindFieldsetByLegend(string locator)
        {
            var legend = Watin.Elements.Filter(e => e.TagName == "LEGEND")
                                       .Filter(Find.ByText(locator))
                                       .FirstDisplayedOrDefault();

            return legend != null && IsFieldset(legend.Parent) 
                    ? legend.Parent 
                    : null;
        }

        private bool IsFieldset(WatiN.Core.Element e)
        {
            return e.TagName == "FIELDSET";
        }

        public Element FindSection(string locator)
        {
            var section = Watin.Elements.FirstDisplayedOrDefault(e => e.Id == locator && IsSection(e)) ??
                          Watin.Elements
                                 .Where(e => headerTagNames.Contains(e.TagName) && 
                                             TextMatches(e,locator) &&
                                             sectionTagNames.Contains(e.Parent.TagName))
                               .Select(h => h.Parent)
                               .FirstDisplayedOrDefault();

            return BuildElement(section, "Failed to find section: " + locator);
        }

        private bool IsSection(WatiN.Core.Element e)
        {
            return sectionTagNames.Contains(e.TagName);
        }

        public Element FindId(string id)
        {
            var element = Watin.Elements.Filter(Find.ById(id)).FirstDisplayedOrDefault(Scope);
            return BuildElement(element, "Failed to find id: " + id);
        }

        public Element FindIFrame(string locator)
        {
            var frame = Filter(Watin.Frames, f => HasElement(f, "h1", locator) ||
                                                  f.Title == locator ||
                                                  f.Id == locator).FirstOrDefault();

            return BuildElement(frame, "Failed to find frame: " + locator);
        }

        public void Hover(Element element)
        {
            (WatiNElement(element)).MouseEnter();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            throw new NotSupportedException();
        }

        private bool HasElement(IElementContainer elementContainer, string tagName, string text)
        {
            return elementContainer.ElementsWithTag(new List<ElementTag> { new ElementTag(tagName) })
                    .Filter(Find.ByText(text))
                    .Any();
        }

        public Element FindButton(string locator)
        {
            var button = FindFirst(Watin.Buttons, e => TextMatches(e,locator) ||
                                                       e.Id == locator ||
                                                       e.Name == locator ||
                                                       e.Value == locator);
            if (button == null)
            {
                var buttonTags = Watin.Elements.Filter(e => e.TagName == "BUTTON");
                button = FindFirst(buttonTags, e => TextMatches(e,locator) ||
                                                    e.Id == locator ||
                                                    e.Name == locator);
            }

            return BuildElement(button, "Failed to find button with text, id or name: " + locator);
        }

        private bool TextMatches(WatiN.Core.Element element, string expectedText)
        {
            return !string.IsNullOrEmpty(element.OuterText) && element.OuterText.Trim() == expectedText.Trim();
        }

        public IEnumerable<WatiN.Core.Element> Filter<TComponent>(IComponentCollection<TComponent> collection, Constraint constraint) where TComponent : Component
        {
            return collection.Filter(constraint).Cast<WatiN.Core.Element>().WithinScope(Scope);
        }

        public IEnumerable<WatiN.Core.Element> Filter<TComponent>(IComponentCollection<TComponent> collection, Predicate<TComponent> predicate) where TComponent : Component
        {
            return collection.Filter(predicate).Cast<WatiN.Core.Element>().WithinScope(Scope);
        }

        public WatiN.Core.Element FindFirst<TComponent>(IComponentCollection<TComponent> collection, Constraint constraint) where TComponent : Component
        {
            return Filter(collection, constraint).FirstDisplayedOrDefault(Scope);
        }

        public WatiN.Core.Element FindFirst<TComponent>(IComponentCollection<TComponent> collection, Predicate<TComponent> predicate) where TComponent : Component
        {
            return Filter(collection, predicate).FirstDisplayedOrDefault(Scope);
        }

        public Element BuildElement(WatiN.Core.Element element, string description)
        {
            if (element == null)
            {
                throw new MissingHtmlException(description);
            }
            return new WatiNElement(element);
        }

        public Element FindLink(string linkText)
        {
            var link = FindFirst(Watin.Links, Find.ByText(linkText));
            return BuildElement(link, "Failed to find link with text: " + linkText);
        }

        public Element FindField(string locator)
        {
            var allFields = FindAllFields();

            var field = FindFieldByLabel(locator, allFields) ??
                        allFields.FirstDisplayedOrDefault(
                            Scope, f => f.Id == locator ||
                                        f.Name == locator ||
                                        HasAttribute(f, "value", locator) ||
                                        HasAttribute(f, "placeholder", locator));

            return BuildElement(field, "Failed to find field with label, id, name or placeholder: " + locator);
        }

        private bool HasAttribute(WatiN.Core.Element element, string attributeName, string attributeValue)
        {
            return element.GetAttributeValue(attributeName) == attributeValue;
        }

        private WatiN.Core.Element FindFieldByLabel(string locator, IEnumerable<WatiN.Core.Element> allFields)
        {
            var label = (Label)Filter(Watin.Labels, Find.ByText(locator)).FirstWithinScopeOrDefault(Scope);
            if (label != null)
            {
                return allFields.FirstDisplayedOrDefault(Scope, f => f.Id == label.For) ??
                       allFields.FirstDisplayedOrDefault(Scope, f => IsElementInContainer(f, label));
            }
            return null;
        }

        private bool IsElementInContainer(WatiN.Core.Element element, IElementContainer container)
        {
            return container.Children().Any(child => child.Equals(element));
        }

        private WatiN.Core.Element GetRadioButtonWithValue(string value)
        {
            return FindFirst(Watin.RadioButtons, r => r.GetAttributeValue("value") == value);
        }

        private IEnumerable<WatiN.Core.Element> FindAllFields()
        {
            var textFields = Watin.TextFields.Cast<WatiN.Core.Element>();
            var selects = Watin.SelectLists.Cast<WatiN.Core.Element>();
            var checkboxes = Watin.CheckBoxes.Cast<WatiN.Core.Element>();
            var radioButtons = Watin.RadioButtons.Cast<WatiN.Core.Element>();
            var fileUploads = Watin.FileUploads.Cast<WatiN.Core.Element>();

            return fileUploads
                    .Union(textFields)
                    .Union(selects)
                    .Union(checkboxes)
                    .Union(radioButtons)
                    .Union(fileUploads);
        }

        public void Click(Element element)
        {
            WatiNElement(element).Click();
        }

        private WatiN.Core.Element WatiNElement(Element element)
        {
            return ((WatiN.Core.Element) element.Native);
        }

        public void Visit(string url)
        {
            Watin.GoTo(url);
        }

        public void Set(Element element, string value)
        {
            var textField = element.Native as TextField;
            if (textField != null)
            {
                textField.Value = value;
                return;
            }
            var fileUpload = element.Native as FileUpload;
            if (fileUpload != null) fileUpload.Set(value);
        }

        public void Select(Element element, string option)
        {
            try
            {
                ((SelectList) element.Native).Select(option);
            }
            catch (WatiNException)
            {
                ((SelectList) element.Native).SelectByValue(option);
            }
        }

        public object Native
        {
            get { return Watin; }
        }

        public bool HasContent(string text)
        {
            return Scope != null
                ? Scope.Text.Contains(text) 
                : Watin.ContainsText(text);
        }
        
        public bool HasContentMatch(Regex pattern)
        {
            return Scope != null
                ? pattern.IsMatch(Scope.Text) 
                : Watin.ContainsText(pattern);
        }

        public void Check(Element field)
        {
            ((CheckBox)field.Native).Checked = true;
        }

        public void Uncheck(Element field)
        {
            ((CheckBox)field.Native).Checked = false;
        }

        public void Choose(Element field)
        {
            ((RadioButton)field.Native).Checked = true;
        }

        public bool HasDialog(string withText)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public void AcceptModalDialog()
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public void CancelModalDialog()
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public bool HasCss(string cssSelector)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public bool HasXPath(string xpath)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public Element FindCss(string cssSelector)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public Element FindXPath(string xpath)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            throw new NotSupportedException("Not yet implemented in WatiNDriver");
        }

        public void Dispose()
        {
            Watin.Dispose();
            Disposed = true;
        }


    }
}