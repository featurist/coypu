using Coypu.Finders;

namespace Coypu.Actions
{
    internal class Select : DriverAction
    {
        private readonly DisambiguationStrategy _disambiguationStrategy;
        private Options _fromOptions;
        private readonly string _locator;
        private readonly Options _options;
        private readonly string _optionToSelect;
        private ElementScope _selectElement;

        internal Select(IDriver driver,
                        DriverScope scope,
                        string locator,
                        string optionToSelect,
                        DisambiguationStrategy disambiguationStrategy,
                        Options options) : base(driver, scope, options)
        {
            _locator = locator;
            _optionToSelect = optionToSelect;
            _options = options;
            _disambiguationStrategy = disambiguationStrategy;
        }

        internal Select(IDriver driver,
                        DriverScope scope,
                        string locator,
                        string optionToSelect,
                        DisambiguationStrategy disambiguationStrategy,
                        Options options,
                        Options fromOptions) : this(driver, scope, locator, optionToSelect, disambiguationStrategy, options)
        {
            _fromOptions = fromOptions;
        }

        internal Select(IDriver driver,
                        ElementScope selectElement,
                        string optionToSelect,
                        DisambiguationStrategy disambiguationStrategy,
                        Options options) : base(driver, selectElement, options)
        {
            _selectElement = selectElement;
            _optionToSelect = optionToSelect;
            _options = options;
            _disambiguationStrategy = disambiguationStrategy;
        }

        public override void Act()
        {
            _selectElement = _selectElement ?? FindSelectElement();
            SelectOption(_selectElement);
        }

        private SnapshotElementScope FindSelectElement()
        {
            if (_fromOptions == null) _fromOptions = _options;
            var selectElementFound = _disambiguationStrategy.ResolveQuery(new SelectFinder(Driver, _locator, Scope, _fromOptions));
            return new SnapshotElementScope(selectElementFound, Scope, _fromOptions);
        }

        private void SelectOption(ElementScope selectElementScope)
        {
            var option = _disambiguationStrategy.ResolveQuery(new OptionFinder(Driver, _optionToSelect, selectElementScope, _options));
            Driver.Click(option);
        }
    }
}