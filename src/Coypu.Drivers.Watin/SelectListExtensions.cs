using WatiN.Core;
using WatiN.Core.Comparers;
using WatiN.Core.Constraints;
using WatiN.Core.Exceptions;

namespace Coypu.Drivers.Watin
{
    internal static class SelectListExtensions
    {
        public static void SelectByTextOrValue(this SelectList selectList, string option)
        {
            var comparer = new StringEqualsAndCaseInsensitiveComparer(option);
            var constraint = Find.ByText(comparer) | Find.ByValue(comparer);

            if (selectList.Multiple)
                selectList.SelectByTextOrValueMultiple(constraint);
            else
                selectList.SelectByTextOrValueSingle(constraint);
        }

        private static void SelectByTextOrValueMultiple(this SelectList selectList, Constraint constraint)
        {
            // This is copied from SelectList.SelectMultiple
            var options = selectList.Options.Filter(constraint);
            if (options.Count == 0)
                throw new SelectListItemNotFoundException(constraint.ToString(), selectList);

            foreach (var option in options)
            {
                if (option.Selected) continue;
                option.SetAttributeValue("selected", "true");
            }

            selectList.FireEvent("onchange");
        }

        private static void SelectByTextOrValueSingle(this SelectList selectList, Constraint constraint)
        {
            selectList.Option(constraint).Select();
        }
    }
}