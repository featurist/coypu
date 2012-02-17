namespace Coypu.Queries
{
    internal abstract class DialogQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public abstract bool ExpectedResult { get; }
        public bool Result { get; private set; }

        protected DialogQuery(Driver driver, string text)
        {
            this.driver = driver;
            this.text = text;
        }

        public void Run()
        {
            Result = driver.HasDialog(text) == ExpectedResult;
        }
    }

    internal class HasDialogQuery : DialogQuery
    {
        internal HasDialogQuery(Driver driver, string text)
            : base(driver, text)
        {
        }

        public override bool ExpectedResult
        {
            get { return true; }
        }
    }

    internal class HasNoDialogQuery : DialogQuery
    {
        internal HasNoDialogQuery(Driver driver, string text)
            : base(driver, text)
        {
        }

        public override bool ExpectedResult
        {
            get { return false; }
        }
    }
}