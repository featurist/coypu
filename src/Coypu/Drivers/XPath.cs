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
                ".//*[" +
                "     (" + TagNamedOneOf("input") + " and " + AttributeIsOneOf("type", InputButtonTypes) + ") " +
                "     or " + TagNamedOneOf("button") + " or @role = 'button'" +
                "     or " + XPathNodeHasOneOfClasses("button", "btn") +
                "]" +
                "[@id = {0} or @name = {0} or @value = {0} or @alt = {0} or normalize-space() = {0}]",
                locator.Trim());
        }

        private static readonly string[] FieldTagNames = new[] { "input", "select", "textarea" };
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file", "email", "tel" };
        private static readonly string[] FieldInputTypeWithHidden = FieldInputTypes.Union(new[] { "hidden" }).ToArray();
        private static readonly string[] FindByValueTypes = new[] { "checkbox", "radio" };

        public string[] FieldXPaths(string locator, Scope scope)
        {
            var fieldInputTypes = scope.ConsiderInvisibleElements
                                      ? FieldInputTypeWithHidden
                                      : FieldInputTypes;

            var forLabeledOrByIdNamePlaceholderOrValue = Format(
                ".//*[" +
                "   (" + TagNamedOneOf(FieldTagNames) + " and " +
                "       (" +
                "           (@id = //label[normalize-space() = {0}]/@for)" +
                "           or " +
                "           (" +
                "               (" + AttributeIsOneOf("type", fieldInputTypes) + " or " +
                TagNamedOneOf(FieldTagNames.Except(new[] { "input" }).ToArray()) + ")" +
                "               and " +
                "               (@id = {0} or @name = {0} or @placeholder = {0})" +
                "           )" +
                "           or " +
                "           (" + AttributeIsOneOf("type", FindByValueTypes) + " and @value = {0})" +
                "       )" +
                "   )" +
                "]",
                locator.Trim());

            var containerLabeled = Format(
                ".//label[normalize-space() = {0}]//*[" + TagNamedOneOf(FieldTagNames) + "]",
                locator.Trim());

            var forLabeledPartial = Format(
                ".//*[" + TagNamedOneOf(FieldTagNames) +
                " and @id = //label[contains(normalize-space(),{0})]/@for]",
                locator.Trim());

            var containerLabeledPartial = Format(
                ".//label[contains(normalize-space(),{0})]//*[" + TagNamedOneOf(FieldTagNames) + "]",
                locator.Trim());

            return new[] { forLabeledOrByIdNamePlaceholderOrValue, containerLabeled, forLabeledPartial, containerLabeledPartial };
        }
    }
}