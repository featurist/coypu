using System.Linq;

namespace Coypu.Drivers
{
    /// <summary>
    /// Construct XPath queries to find different types of HTML element
    /// </summary>
    public class Html : XPath
    {
        private static readonly string[] InputButtonTypes = new[] { "button", "submit", "image", "reset" };
        private static readonly string[] FieldTagNames = new[] { "input", "select", "textarea" };
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file", "email", "tel", "url" };
        private static readonly string[] FieldInputTypeWithHidden = FieldInputTypes.Union(new[] { "hidden" }).ToArray();
        private static readonly string[] FindByNameTypes = FieldInputTypes.Except(new[] { "radio" }).ToArray();
        private static readonly string[] FindByValueTypes = new[] { "checkbox", "radio" };
        private readonly string[] sectionTags = {"section", "div", "li"};
        private readonly string[] headerTags = {"h1", "h2", "h3", "h4", "h5", "h6"};

        public Html(bool uppercaseTagNames) : base(uppercaseTagNames)
        {
        }

        public string Id(string locator, Options options)
        {
            return
                Descendent() +
                Where(Attr("id", locator, true));
        }

        public string Link(string locator, Options options)
        {
            return 
                Descendent("a") + 
                Where(IsText(locator, options.Exact));
        }

        public string Field(string locator, Options options)
        {
            return
                Descendent() +
                Where(
                    TagNamedOneOf(FieldTagNames) +
                    And(
                        IsForLabeled(locator, options.Exact) +
                        or + IsContainerLabeled(locator, options.Exact) +
                        or + HasIdOrPlaceholder(locator, options) +
                        or + HasName(locator) +
                        or + HasValue(locator)));
        }

        public string FrameXPath(string locator)
        {
            return
                Descendent() +
                Where(
                    TagNamedOneOf("iframe", "frame")) +
                    or + FrameAttributesMatch(locator);

        }

        public string Button(string locator, Options options)
        {
            return
                Descendent() +
                Where(
                    Group(
                        IsInputButton() +
                        or + TagNamedOneOf("button") +
                        or + HasOneOfClasses("button", "btn") +
                        or + Attr("role", "button", exact: true)) +
                    And(
                        AttributesMatchLocator(locator, true, "@id", "@name") +
                        or + AttributesMatchLocator(locator.Trim(), options.Exact, "@value", "@alt", "normalize-space()")));
        }

        public string Fieldset(string locator, Options options)
        {
            return
                Descendent("fieldset") +
                Where(
                    Child("legend") +
                    Where(IsText(locator, options.Exact)) +
                    or + Attr("id", locator, exact: true));

        }

        public string Section(string locator, Options options)
        {
            return
                Descendent() +
                Where(
                    TagNamedOneOf(sectionTags) +
                    And(
                        Child() +
                        Where(TagNamedOneOf(headerTags) + and + IsText(locator, options.Exact)) +
                        or + Attr("id", locator, exact: true)));
        }

        public string Option(string locator, Options options)
        {
            return
                Child("option") +
                Where(IsText(locator, options.Exact) + or + Is("@value", locator, options.Exact));
        }

        private string HasValue(string locator)
        {
            return Group(AttributeIsOneOf("type", FindByValueTypes) + and + Attr("value", locator, exact: true));
        }

        private string HasName(string locator)
        {
            return Group(AttributeIsOneOfOrMissing("type", FindByNameTypes) + and + Attr("name", locator, exact: true));
        }

        private string FrameAttributesMatch(string locator)
        {
            return AttributesMatchLocator(locator.Trim(), true, "@id", "@name", "@title");
        }

        private string IsInputButton()
        {
            return Group(TagNamedOneOf("input") + and + AttributeIsOneOf("type", InputButtonTypes));
        }

        private string IsAFieldInputType(Options options)
        {
            var fieldInputTypes = options.ConsiderInvisibleElements
                            ? FieldInputTypeWithHidden
                            : FieldInputTypes;

            return AttributeIsOneOfOrMissing("type", fieldInputTypes);
        }

        private string HasIdOrPlaceholder(string locator, Options options)
        {
            return Group(IsAFieldInputType(options)
                         + And(Attr("id", locator, exact: true) + or + Is("@placeholder", locator, options.Exact)));

        }
    }
}