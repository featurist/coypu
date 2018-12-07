using System.Linq;

// ReSharper disable InheritdocConsiderUsage
#pragma warning disable 1591

namespace Coypu.Drivers
{
    /// <summary>
    ///     Construct XPath queries to find different types of HTML element
    /// </summary>
    public class Html : XPath
    {
        private static readonly string[] InputButtonTypes = {"button", "submit", "image", "reset"};
        private static readonly string[] FieldTagNames = {"input", "select", "textarea"};
        private static readonly string[] FieldInputTypes =
            {"text", "password", "radio", "checkbox", "file", "email", "tel", "url", "number", "datetime", "datetime-local", "date", "month", "week", "time", "color", "search"};
        private static readonly string[] FieldInputTypeWithHidden = FieldInputTypes.Union(new[] {"hidden"})
                                                                                   .ToArray();
        private static readonly string[] FindByNameTypes = FieldInputTypes.Except(new[] {"radio"})
                                                                          .ToArray();
        private static readonly string[] FindByValueTypes = {"checkbox", "radio"};
        private readonly string[] _headerTags = {"h1", "h2", "h3", "h4", "h5", "h6"};
        private readonly string[] _sectionTags = {"section", "div", "li"};

        public Html(bool uppercaseTagNames = false) : base(uppercaseTagNames) { }

        public string Id(string locator,
                         Options options)
        {
            return Descendent() + Where(HasId(locator));
        }

        public string Link(string locator,
                           Options options)
        {
            return Descendent("a") + Where(IsText(locator, options) + Or + HasTitle(locator, options) + Or + HasHref(locator, options));
        }

        public string HasTitle(string title,
                               Options options)
        {
            return Attr("title", title, options);
        }

        public string HasHref(string href,
                              Options options)
        {
            return Attr("href", href, options);
        }

        public string LinkOrButton(string locator,
                                   Options options)
        {
            return Link(locator, options) + Or + Button(locator, options);
        }

        public string Field(string locator,
                            Options options)
        {
            return Descendent() + Where(TagNamedOneOf(FieldTagNames) + And(IsForLabeled(locator, options) +
                                                                           Or + IsContainerLabeled(locator, options) +
                                                                           Or + HasIdOrPlaceholder(locator, options) +
                                                                           Or + HasName(locator) +
                                                                           Or + HasValue(locator)));
        }

        public string Select(string locator,
                             Options options)
        {
            return Descendent("select") + Where(IsForLabeled(locator, options) +
                                                Or + IsContainerLabeled(locator, options) +
                                                Or + HasId(locator) +
                                                Or + HasName(locator));
        }

        public string FrameXPath(string locator)
        {
            return Descendent() + Where(TagNamedOneOf("iframe", "frame")) + Or + FrameAttributesMatch(locator);
        }

        public string Button(string locator,
                             Options options)
        {
            return Descendent() + Where(Group(IsInputButton() +
                                              Or + TagNamedOneOf("button") +
                                              Or + HasOneOfClasses("button", "btn") +
                                              Or + Attr("role", "button", Options.Exact)) +
                                        And(AttributesMatchLocator(locator, Options.Exact, "@id", "@name") +
                                            Or + AttributesMatchLocator(locator.Trim(), options, "@value", "@alt", "normalize-space()")));
        }

        public string Fieldset(string locator,
                               Options options)
        {
            return Descendent("fieldset") + Where(Child("legend") + Where(IsText(locator, options)) + Or + HasId(locator));
        }

        public string Section(string locator,
                              Options options)
        {
            return Descendent() + Where(TagNamedOneOf(_sectionTags) + And(Child() +
                                                                          Where(TagNamedOneOf(_headerTags) + and + IsText(locator, options)) + Or + HasId(locator)));
        }

        public string Option(string locator,
                             Options options)
        {
            return Child("option") + Where(IsText(locator, options) + Or + Is("@value", locator, options));
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

        private string HasIdOrPlaceholder(string locator,
                                          Options options)
        {
            return Group(IsAFieldInputType(options)
                         + And(HasId(locator) + Or + Is("@placeholder", locator, options)));
        }

        private string HasId(string locator)
        {
            return Attr("id", locator, Options.Exact);
        }
    }
}