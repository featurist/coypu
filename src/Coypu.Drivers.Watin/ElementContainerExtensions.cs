using System.Collections.Generic;
using System.Linq;
using Coypu.Drivers.Watin.Properties;
using WatiN.Core;
using WatiN.Core.Native.InternetExplorer;

namespace Coypu.Drivers.Watin
{
    internal static class ElementContainerExtensions
    {
        public static ElementCollection<Fieldset> Fieldsets(this IElementContainer elementContainer)
        {
            return elementContainer.ElementsOfType<Fieldset>();
        }

        public static ElementCollection<Section> Sections(this IElementContainer elementContainer)
        {
            return elementContainer.ElementsOfType<Section>();
        }

        public static IEnumerable<WatiN.Core.Element> XPath(this IElementContainer elementContainer, string xpath)
        {
            DomContainer domContainer = null;

            // TODO: Test that contextNode works
            var container = "document";

            var document = elementContainer as Document;
            if (document != null)
                domContainer = document.DomContainer;

            var element = elementContainer as WatiN.Core.Element;
            if (element != null)
            {
                container = element.NativeElement.GetJavaScriptElementReference();
                domContainer = element.DomContainer;
            }

            if (domContainer == null)
                return Enumerable.Empty<WatiN.Core.Element>();

            if (domContainer.Eval("window.jsxpath") == "undefined")
            {
                var script = "window.jsxpath = { targetFrame: undefined, exportInstaller: false, useNative: true, useInnerText: true };" +
                    Resources.javascript_xpath;
                domContainer.RunScript(script);
            }

            // TODO: This escaping will fail if the string is already escaped
            var code = string.Format("document.___WATINRESULT = document.evaluate('{0}', {1}, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);", xpath.Replace("'", @"\'"), container);
            domContainer.RunScript(code);

            return from nativeElement in new JScriptXPathResultEnumerator((IEDocument)domContainer.NativeDocument, "___WATINRESULT")
                   select ElementFactory.CreateElement(domContainer, nativeElement);
        }
    }
}