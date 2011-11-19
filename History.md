# Version 0.6.0

Release date: 2011-11-19

### Added
* ConsideringInvisibleElements scope added to sessions - for when you reeeeeally need it

# Version 0.5.4

Release date: 2011-11-10

### Fixed
* Session.Location not respecting iframe scope

# Version 0.5.3

Release date: 2011-11-06

### Changed
* Selenium 2.11.0 or later

# Version 0.5.2

Release date: 2011-10-18

### Fixed
* Proj dependencies broken

# Version 0.5.1

Release date: 2011-10-18

### Fixed
* Dependency on Newtonsoft.Json via Selenium.WebDriver broken
* You may need to Uninstall-Package for Coypu, Selenium.WebDriver and Newtonsoft.Json then Install-Package Coypu

# Version 0.5.0

Release date: 2011-10-11

### Added
* HtmlUnit help

### Changed
* Upgrade to Selenium.WebDriver 2.7.0

### BREAKING CHANGE
* Timeout default down from 10 to 1 sec

# Version 0.4.0

Release date: 2011-09-02

### Added
* Expose browser's current location

### Changed
* Split out WatiN driver into seperate Coypu.Watin package, only WebDriver is included in Coypu by default.
* Reference official Selenium on nuget and make a dependency of Coypu.nuget

### Fixed
* Error on second use of IFrame scope within the same block

# Version 0.3.0

Release date: 2011-08-02

### Fixed
* FindSection not working across all versions of Firefox

### Changed
* Will bump the minor version for any API/behaviour changes from now on as I should have been doing - bump to 0.3.0 to break with 0.2.x 

# Version 0.2.10

Release date: 2011-07-25

### Fixed
* FindSection patched to remove compound css selector after this broke in WebDriver 2.1

### Added
* Overload for Within that returns a result from within the given scope

# Version 0.2.9

Release date: 2011-07-21

### Changed
* Update to Selenium WebDriver 2.1.0

# Version 0.2.8

Release date: 2011-07-18

### Added
* SaveWebResource saves a resource from the web directly using the current browser session's cookies
* Find buttons by role='button' attr 

### Fixed
* Escaping both types of quotes in xpath literals

# Version 0.2.7

Release date: 2011-07-12

### Added
* FindState - finds the first to be reached from a list of possible states the page may be in

### Fixed 
* Don't click in file fields, was bringing up the browse dialog in some browsers

# Version 0.2.6

Release date: 2011-07-04

### Changed
* Selenium WedDriver RC3 is now on nuget, so it's a dependency again.

### Added
* Enabled AndroidDriver for Selenium
* SeleniumWebDriver now exposes a constructor overload that takes in an instance of RemoteWebDriver. This allows you to create your own derived drivers that can make use of the RemoteWebDriver (e.g. using HtmlUnitDriver) (citizenmatt)

# Version 0.2.5

Release date: 2011-07-03

### Fixed
* Fix issue #13 - HasNo methods are knackered

# Version 0.2.4

Release date: 2011-06-30

### Added
* Adding doc comments to Session API

### Changed
* Bunch of stuff that should have been internal

# Version 0.2.3

Release date: 2011-06-27

### Changed
* Upgrade of Selenium WebDriver to 2.0rc3

# Version 0.2.2

Release date: 2011-06-27

### Added
* Rounded out the session API with FindSection, FindFieldset and FindId and make the Session.Driver internal. 
* Has(Func<Element>) and HasNot(Func<Element>) for custom queries.
* Robustly, Query & TryUntil are now callable directly on session.

# Version 0.2.1

Release date: 2011-06-25

### Added
Expose the RobustWrapper on Session exposing Robustly(), Query() and TryUntil() directly to help with custom waits & retries

# Version 0.2.0

Release date: 2011-06-20

### Dependencies
* Upgrade to Selenium 2.0 RC2 (package Selenium directly again, until it becomes available as a nuget dependency)

### Added
* Hover(Func<Element>)
* Click(Func<Element>)
* Scoping within iframes
* Support id ends with, for buttons and fields
* HasContentMatch for regex, HasContent for text

### Changed
* Convert to VS2010, reference selenium+watin nuget packages, refine end-to-end examples
* SeleniumWebDriver optimisation
* End session checks for ActiveSession

# Version 0.1.3

Release date: 2011-05-27

2011-05-27

### Added
* WithinFieldset and WithinSection
* Configurable wait between find & click
* Much more support for WatiN driver (see driver_test_results.txt)
* MIT Licence 
* Nuget

### Changed
* Section headers may contain other markup, e.g. links 
* Reuse scope within individual driver methods
* Close any alerts on disposing SeleniumWebDriver 

# Version 0.1.2

Release date: 2011-05-12

### Added

* Configure the AppHost, Port and SSL globally
* Simple ExecuteScript in SeleniumWebDriver
* Set file upload paths with FillIn.With
* Scoping of HasContent
* ClickLink and ClickButton now have TryUntil overloads

### Changed

* Visit now takes a virtual path
* Renamed WaitAndRetryRobustWrapper to RetryUntilTimeoutRobustWrapper 


