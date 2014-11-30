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
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file", "email", "tel", "url", "number", "datetime", "datetime-local", "date", "month", "week", "time", "color", "search" };
        private static readonly string[] FieldInputTypeWithHidden = FieldInputTypes.Union(new[] { "hidden" }).ToArray();
        private static readonly string[] FindByNameTypes = FieldInputTypes.Except(new[] { "radio" }).ToArray();
        private static readonly string[] FindByValueTypes = new[] { "checkbox", "radio" };
        private readonly string[] sectionTags = {"section", "div", "li"};
        private readonly string[] headerTags = {"h1", "h2", "h3", "h4", "h5", "h6"};

        public Html(bool uppercaseTagNames = false) : base(uppercaseTagNames)
        {
        }

        public string Id(string locator, Options options)
        {
            return
                Descendent() +
                Where(HasId(locator));
        }

        public string Link(string locator, Options options)
        {
            return 
                Descendent("a") + 
                Where(IsText(locator, options));
        }

        public string LinkOrButton(string locator, Options options)
        {
            return Link(locator, options) + or + Button(locator, options);
        }

        public string Field(string locator, Options options)
        {
            return
                Descendent() +
                Where(
                    TagNamedOneOf(FieldTagNames) +
                    And(
                        IsForLabeled(locator, options) +
                        or + IsContainerLabeled(locator, options) +
                        or + HasIdOrPlaceholder(locator, options) +
                        or + HasName(locator) +
                        or + HasValue(locator)));
        }

        public string Select(string locator, Options options)
        {
            return
                Descendent("select") +
                Where(
                    IsForLabeled(locator, options) +
                    or + IsContainerLabeled(locator, options) +
                    or + HasId(locator) +
                    or + HasName(locator));
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
                        or + Attr("role", "button", Options.Exact)) +
                    And(
                        AttributesMatchLocator(locator, Options.Exact, "@id", "@name") +
                        or + AttributesMatchLocator(locator.Trim(), options, "@value", "@alt", "normalize-space()")));
        }

        public string Fieldset(string locator, Options options)
        {
            return
                Descendent("fieldset") +
                Where(
                    Child("legend") +
                    Where(IsText(locator, options)) +
                    or + HasId(locator));

        }

        public string Section(string locator, Options options)
        {
            return
                Descendent() +
                Where(
                    TagNamedOneOf(sectionTags) +
                    And(
                        Child() +
                        Where(TagNamedOneOf(headerTags) + and + IsText(locator, options)) +
                        or + HasId(locator)));
        }

        public string Option(string locator, Options options)
        {
            return
                Child("option") +
                Where(IsText(locator, options) + or + Is("@value", locator, options));
        }

        private string HasValue(string locator)
        {
            return Group(AttributeIsOneOf("type", FindByValueTypes) + and + Attr("value", locator, Options.Exact));
        }

        private string HasName(string locator)
        {
            return Group(AttributeIsOneOfOrMissing("type", FindByNameTypes) + and + Attr("name", locator, Options.Exact));
        }

        private string FrameAttributesMatch(string locator)
        {
            return AttributesMatchLocator(locator.Trim(), Options.Exact, "@id", "@name", "@title");
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
                         + And(HasId(locator) + or + Is("@placeholder", locator, options)));

        }

        private string HasId(string locator)
        {
            return Attr("id", locator, Options.Exact);
        }
    }
}