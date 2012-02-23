using WatiN.Core;
using WatiN.Core.Native;

namespace Coypu.Drivers.Watin
{
    [ElementTag("section")]
    public class Section : ElementContainer<Section>
    {
        public Section(DomContainer domContainer, INativeElement nativeElement)
            : base(domContainer, nativeElement)
        {
        }

        public Section(DomContainer domContainer, WatiN.Core.ElementFinder finder)
            : base(domContainer, finder)
        {
        }
    }
}