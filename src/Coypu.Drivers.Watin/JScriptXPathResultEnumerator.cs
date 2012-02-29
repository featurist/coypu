using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using WatiN.Core.Native;
using WatiN.Core.Native.InternetExplorer;
using mshtml;

namespace Coypu.Drivers.Watin
{
    internal class JScriptXPathResultEnumerator : IEnumerable<INativeElement>
    {
        private readonly IEDocument ieDocument;
        private readonly string fieldName;

        public JScriptXPathResultEnumerator(IEDocument ieDocument, string fieldName)
        {
            this.ieDocument = ieDocument;
            this.fieldName = fieldName;
        }

        public IEnumerator<INativeElement> GetEnumerator()
        {
            var result = new Expando(ieDocument.HtmlDocument).GetValue(fieldName);
            if (result == null) yield break;

            var resultAsExpando = new Expando(result);
            var length = resultAsExpando.GetValue<int>("snapshotLength");
            for (var i = 0; i < length; i++)
            {
                var method = resultAsExpando.AsExpando.GetMethod("snapshotItem", BindingFlags.Public);
                var element = method.Invoke(resultAsExpando.Object, new object[] {i}) as IHTMLElement;
                if (element != null) yield return new IEElement(element);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}