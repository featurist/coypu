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

        public const string and = " and ";
        public const string or = " or ";

        public string Group(string expression)
        {
            return "(" + expression + ")";
        }

        public string And(string expression)
        {
            return and + Group(expression);
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
                                         .Select(WrapInDoubleQuotes)
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
            return String.Format("\"{0}\"", value);
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

        public string HasOneOfClasses(params string[] classNames)
        {
            return Group(String.Join(" or ", classNames.Select(XPathNodeHasClass).ToArray()));
        }

        public string XPathNodeHasClass(string className)
        {
            return String.Format("contains(@class,' {0}') " +
                                 "or contains(@class,'{0} ') " +
                                 "or contains(@class,' {0} ')", className);
        }


        public string AttributeIsOneOfOrMissing(string attributeName, string[] values)
        {
            return Group(AttributeIsOneOf(attributeName, values) + or + "not(@" + attributeName + ")");
        }

        public string AttributeIsOneOf(string attributeName, string[] values)
        {
            return Group(String.Join(" or ", values.Select(t => Format("@" + attributeName + " = {0}",t)).ToArray()));
        }

        public string Attr(string name, string value, Options options)
        {
            return Is("@" + name, value, options);
        }

        public string TagNamedOneOf(params string[] fieldTagNames)
        {
            return Group(string.Join(" or ", fieldTagNames.Select(t => Format("name() = {0}", CasedTagName(t))).ToArray()));
        }

        public string CasedTagName(string tagName)
        {
            return uppercaseTagNames ? tagName.ToUpper() : tagName; ;
        }

        public string AttributesMatchLocator(string locator, Options options, params string[] attributes)
        {
            return Group(string.Join(" or ", attributes.Select(a => Is(a, locator, options)).ToArray()));
        }

        public string IsContainerLabeled(string locator, Options options)
        {
            return Format("ancestor::label[" + IsTextShallow(locator, options) + "]", locator);
        }

        public string IsForLabeled(string locator, Options options)
        {
            return Format(" (@id = //label[" + IsText(locator, options) + "]/@for) ", locator);
        }

        public string Is(string selector, string locator, Options options)
        {
            return options.TextPrecisionExact
                ? Format(selector + " = {0} ", locator)
                : Format("contains(" + selector + ",{0})", locator);
        }

        public string IsText(string locator, Options options)
        {
            return Is("normalize-space()", locator,options);
        }

        public string IsTextShallow(string locator, Options options)
        {
            return Is("normalize-space(text())", locator, options);
        }

        protected string Descendent(string tagName = "*")
        {
            return ".//" + tagName;
        }

        protected string Child(string tagName = "*")
        {
            return "./" + tagName;
        }

        protected static string Where(string predicate)
        {
            return "[" + predicate + "]";
        }
    }
}