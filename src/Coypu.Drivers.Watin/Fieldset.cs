using WatiN.Core;
using WatiN.Core.Native;

namespace Coypu.Drivers.Watin
{
    [ElementTag("fieldset")]
    public class Fieldset : ElementContainer<Fieldset>
    {
        public Fieldset(DomContainer domContainer, INativeElement nativeElement)
            : base(domContainer, nativeElement)
        {
        }

        public Fieldset(DomContainer domContainer, WatiN.Core.ElementFinder finder)
            : base(domContainer, finder)
        {
        }
    }
}