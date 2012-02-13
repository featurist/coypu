using Coypu.Robustness;

namespace Coypu
{
    public class FillInWith : DriverAction
    {
        private readonly string locator;
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly DriverScope scope;
        private Element element;
        private string value;

        internal FillInWith(string locator, Driver driver, RobustWrapper robustWrapper, DriverScope scope)
        {
            this.locator = locator;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
        }

        internal FillInWith(Element element, Driver driver, RobustWrapper robustWrapper, DriverScope scope)
        {
            this.element = element;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.scope = scope;
        }

        public Element Element
        {
            get { return element; }
        }

        public string Locator
        {
            get { return locator; }
        }

        public Driver Driver
        {
            get { return driver; }
        }

        public DriverScope Scope
        {
            get { return scope; }
        }

        /// <summary>
        /// Supply a value for the text field
        /// </summary>
        /// <param name="value">The value to fill in</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public void With(string value)
        {
            this.value = value;
            robustWrapper.RobustlyDo(this);
        }

        internal Element Field
        {
            get { return Element ?? Driver.FindField(locator, scope); }
        }

        private void BringIntoFocus()
        {
            Driver.Click(Field);
        }

        internal void Set(string value)
        {
            Driver.Set(Field, value);
        }

        internal void Focus()
        {
            if (Field["type"] != "file")
                BringIntoFocus();
        }

        public void Act()
        {
            Focus();
            Set(value);
        }
    }

    internal class FillInWithDriverAction : DriverAction
    {
        private readonly FillInWith fillInWith;
        private readonly string value;


        internal FillInWithDriverAction(FillInWith fillInWith, string value)
        {
            this.fillInWith = fillInWith;
            this.value = value;
        }

        public void Act()
        {
            fillInWith.Focus();
            fillInWith.Set(value);
        }

    }

    internal class ChooseDriverAction : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string locator;

        internal ChooseDriverAction(Driver driver, DriverScope scope, string locator)
        {
            this.driver = driver;
            this.scope = scope;
            this.locator = locator;
        }

        public void Act()
        {
            driver.Choose(driver.FindField(locator,scope));
        }
    }
}