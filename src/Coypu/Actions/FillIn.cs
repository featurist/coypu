namespace Coypu.Actions
{
    internal class FillIn : DriverAction
    {
        private readonly string value;
        private readonly ElementScope element;

        internal FillIn(Driver driver, ElementScope element, string value, Options options) : base(driver,options)
        {
            this.element = element;
            this.value = value;
        }

        private void BringIntoFocus()
        {
            Driver.Click(element);
        }

        internal void Set()
        {
            Driver.Set(element, value);
        }

        internal void Focus()
        {
            if (element["type"] != "file")
                BringIntoFocus();
        }

        public override void Act()
        {
            Focus();
            Set();
        }
    }
}