namespace Coypu.Drivers.Watin
{
    public static class ElementMatcher
    {
        public static bool HasId(this WatiN.Core.Element element, string expectedId)
        {
            return element.Id == expectedId || (element.Id != null && element.Id.EndsWith(expectedId));
        }

        public static bool HasName(this WatiN.Core.Element element, string expectedName)
        {
            return element.Name == expectedName;
        }

        public static bool HasText(this WatiN.Core.Element element, string expectedText)
        {
            return !string.IsNullOrEmpty(element.OuterText) && element.OuterText.Trim() == expectedText.Trim();
        }

        public static bool HasValue(this WatiN.Core.Element element, string expectedValue)
        {
            return element.GetAttributeValue("value") == expectedValue;
        }

        public static bool HasAltText(this WatiN.Core.Element element, string expectedAltText)
        {
            return element.GetAttributeValue("alt") == expectedAltText;
        }
    }
}