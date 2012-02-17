using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    public static class ElementContainerExtensions
    {
        public static ElementCollection<Fieldset> Fieldsets(this IElementContainer elementContainer)
        {
            return elementContainer.ElementsOfType<Fieldset>();
        }

        public static ElementCollection<Section> Sections(this IElementContainer elementContainer)
        {
            return elementContainer.ElementsOfType<Section>();
        }
    }
}