using System;
using System.Linq;

namespace Coypu.Drivers
{
    /// <summary>
    /// Helper for formatting XPath queries
    /// </summary>
    public class XPath
    {
        private readonly bool uppercaseTagNames;

        public XPath(bool uppercaseTagNames = false)
        {
            this.uppercaseTagNames = uppercaseTagNames;
        }

        /// <summary>
        /// <para>Format an XPath query that uses string values for comparison that may contain single or double quotes</para>
        /// <para>Wraps the string in the appropriate quotes or uses concat() to separate them if both are present.</para>
        /// <para>Usage:</para>
        /// <code>  new XPath().Format(".//element[@attribute1 = {0} and @attribute2 = {1}]",inputOne,inputTwo) </code>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Format(string value, params object[] args)
        {
            return String.Format(value, args.Select(a => Literal(a.ToString())).ToArray());
        }

        internal string Literal(string value)
        {
            if (HasNoDoubleQuotes(value))
                return WrapInDoubleQuotes(value);

            if (HasNoSingleQuotes(value))
                return WrapInSingleQuote(value);

            return BuildConcatSeparatingSingleAndDoubleQuotes(value);
        }

        private string BuildConcatSeparatingSingleAndDoubleQuotes(string value)
        {
            var doubleQuotedParts = value.Split('\"')
                                         .Select(e => WrapInDoubleQuotes(e))
                                         .ToArray();

            var reJoinedWithDoubleQuoteParts = String.Join(", '\"', ", doubleQuotedParts);

            return String.Format("concat({0})", TrimEmptyParts(reJoinedWithDoubleQuoteParts));
        }

        private string WrapInSingleQuote(string value)
        {
            return String.Format("'{0}'", value);
        }

        private string WrapInDoubleQuotes(string value)
        {
            return String.Format("\"{0}\"",value);
        }

        private string TrimEmptyParts(string concatArguments)
        {
            return concatArguments.Replace(", \"\"", "")
                                           .Replace("\"\", ", "");
        }

        private bool HasNoSingleQuotes(string value)
        {
            return !value.Contains("'");
        }

        private bool HasNoDoubleQuotes(string value)
        {
            return !value.Contains("\"");
        }

        public static string XPathNodeHasOneOfClasses(params string[] classNames)
        {
            return String.Join(" or ", classNames.Select(XPathNodeHasClass).ToArray());
        }

        public static string XPathNodeHasClass(string className)
        {
            return String.Format(" contains(@class,' {0}') " +
                                 "or contains(@class,'{0} ') " +
                                 "or contains(@class,' {0} ') ",className);
        }

        public string AttributeIsOneOf(string attributeName, string[] values)
        {
            return "(" + String.Join(" or ",values.Select(t => "@" + attributeName + "='" + t + "'").ToArray()) + ")";
        }

        public string TagNamedOneOf(params string[] fieldTagNames)
        {
            return "(" + string.Join(" or ", fieldTagNames.Select(t => "name()='" + CasedTagName(t) + "'").ToArray()) + ")";
        }

        public string CasedTagName(string tagName)
        {
            return uppercaseTagNames ? tagName.ToUpper() : tagName;;
        }

        private static readonly string[] InputButtonTypes = new[] { "button", "submit", "image", "reset" };

        public string ButtonXPath(string locator)
        {
            return Format(
                ".//*[" +    IsInputButton() +
                "     or " + TagNamedOneOf("button") +
                "     or " + XPathNodeHasOneOfClasses("button", "btn") +
                "     or @role = 'button'" +
                "][" + ButtonAttributesMatchLocator(locator.Trim()) + "]",
                locator.Trim());
        }

        private string ButtonAttributesMatchLocator(string locator)
        {
            return Format("@id = {0} or @name = {0} or @value = {0} or @alt = {0} or normalize-space() = {0}", locator);
        }

        private string IsInputButton()
        {
            return "(" + TagNamedOneOf("input") + " and " + AttributeIsOneOf("type", InputButtonTypes) + ")";
        }

        private static readonly string[] FieldTagNames = new[] { "input", "select", "textarea" };
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file", "email", "tel", "url" };
        private static readonly string[] FindByNameTypes = FieldInputTypes.Except(new []{"radio"}).ToArray();
        private static readonly string[] FieldInputTypeWithHidden = FieldInputTypes.Union(new[] { "hidden" }).ToArray();
        private static readonly string[] FindByValueTypes = new[] { "checkbox", "radio" };

        public string[] FieldXPathsByPrecedence(string locator, Scope scope)
        {
            locator = locator.Trim();
            return new[]
                {
                    ForlabeledOrByAttribute(locator, scope),
                    ContainerLabeled(locator, scope),
                };
        }

        private string ContainerLabeled(string locator, Scope scope)
        {
            return (scope.FieldFinderPrecision == FieldFinderPrecision.ExactLabel)
                       ? ContainerLabeledExact(locator)
                       : ContainerLabeledPartial(locator);
        }

        private string ForlabeledOrByAttribute(string locator, Scope scope)
        {
            var isLabeledWith = (scope.FieldFinderPrecision == FieldFinderPrecision.ExactLabel)
                                    ? IsLabelledWith(locator)
                                    : IsPartialLabelledWith(locator);
            return Format(
                ".//*[" + TagNamedOneOf(FieldTagNames) +
                "   and " +
                "   (" + isLabeledWith +
                "      or " + HasIdOrPlaceholder(locator, scope) +
                "      or " + HasName(locator) +
                "      or " + HasValue(locator) +
                "   )" +
                "]",
                locator);
        }

        private string ContainerLabeledPartial(string locator)
        {
            return Format(".//label[contains(normalize-space(),{0})]//*[" + TagNamedOneOf(FieldTagNames) + "]",locator);
        }

        private string ContainerLabeledExact(string locator)
        {
            return Format(".//label[normalize-space() = {0}]//*[" + TagNamedOneOf(FieldTagNames) + "]",locator);
        }

        private string IsLabelledWith(string locator)
        {
            return Format("(@id = //label[normalize-space() = {0}]/@for)", locator);
        }

        private string IsPartialLabelledWith(string locator)
        {
            return Format("(@id = //label[contains(normalize-space(),{0})]/@for)", locator);
        }

        private string HasValue(string locator)
        {
            return Format("(" + AttributeIsOneOf("type", FindByValueTypes) + " and @value = {0})", locator);
        }

        private string HasName(string locator)
        {
            return Format("((" + AttributeIsOneOf("type", FindByNameTypes) + " or not(@type)) and @name = {0})", locator);
        }

        private string HasIdOrPlaceholder(string locator, Scope scope)
        {
            return Format("(" + IsAFieldInputType(scope) + " and " + "(@id = {0} or @placeholder = {0}))", locator);
        }

        private string IsAFieldInputType(Scope scope)
        {
            var fieldInputTypes = scope.ConsiderInvisibleElements
                            ? FieldInputTypeWithHidden
                            : FieldInputTypes;

            return "(" + AttributeIsOneOf("type", fieldInputTypes) + " or not(@type))";
        }
    }
}