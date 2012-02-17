using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    public static class ElementContainerExtensions
    {
        public static ElementCollection<Fieldset> Fieldsets(this IElementContainer elementContainer)
        {
            return elementContainer.ElementsOfType<Fieldset>();
        }
    }
}