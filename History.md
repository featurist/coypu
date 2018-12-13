# Coypu 3.0.1 - 13.12.2018
* [#199](https://github.com/featurist/coypu/issues/199) removing nuspec related files to allign with new approach of creating nuget packages from csproj files

Can be also downloaded as nuget packages from https://www.nuget.org/packages/Coypu/
## Coypu.NUnit 3.0.1 - 13.12.2018
* nuspec still used to generate nuget package

Can be also downloaded as nuget package from https://www.nuget.org/packages/Coypu.NUnit/

# Coypu 3.0.0-rc - 11.12.2018
Can be also downloaded as nuget package from: https://www.nuget.org/packages/Coypu/
### Breaking changes
* support for .NET Framework 4.5+
* support for .NET Standard 2.0+
* [#144](https://github.com/featurist/coypu/issues/144) support for .NET Core 2.0+
* updated Selenium WebDriver to 3.141.0
* removed all projects and dependencies related to WatiN
* removed methods from DriverScope and IScope which has been marked as obsolete since 2014:
	- HasCss()
	- HasNoCss()
	- HasXPath()
	- HasNoXPath()
### Fixes
* [#92](https://github.com/featurist/coypu/issues/92) Added the ability to pass in separate options to the `Select("value", selectOptions).From("selectId", fromOptions)` methods
* [#189](https://github.com/featurist/coypu/issues/189) fixed by update of selenium webdriver to v3.141.0
* [#146](https://github.com/featurist/coypu/issues/146) and [#164](https://github.com/featurist/coypu/issues/164) fix for WaitBeforeClick for general Click(), previously option WaitBeforeClick worked only for ClickButton() and ClickLink() methods
* various fixes for failing unit, driver and acceptance tests
### Changes
* added new Cookies class with methods which are available from `_browserSession.Driver.Cookies`:
	- AddCookie(cookie)
	- DeleteAll()
	- DeleteCookie(cookie)
	- DeleteCookieNamed(cookieName)
	- GetCookieNamed(cookieName)
	- WaitUntilCookieExists(cookie, options)
* methods marked as obsolete:
	- GetBrowserCookies() -> use instead `_browserSession.Driver.Cookies.GetAll()`
	- ClearBrowserCookies() -> use instead `_browserSession.Driver.Cookies.DeleteAll()`
* migrate solution projects to new VS2017 format
* updated browser drivers to newest versions (geckodriver, chromedriver, iedriver)

## Coypu.NUnit 3.0.0-rc - 11.12.2018
### Breaking changes
* support for .NET Framework 4.5+
* support for .NET Standard 2.0+
* [#144](https://github.com/featurist/coypu/issues/144) support for .NET Core 2.0+
* updated Coypu to v3.0.0-rc

# Version 2.11.0

### Updated

* Update min Selenium dependency to 3.0.1

Release date: 2016-12-06


# Version 2.10.0

### Updated

* Update min Selenium dependency to 2.48.0

Release date: 2015-10-23


# Version 2.9.1

### Added

* Fix FindXPath with text to use xpath builder rather than slow post filter

Release date: 2015-05-29


# Version 2.9.0

### Added

* FindXPath takes a text parameter like FindCss

Release date: 2015-05-29


# Version 2.8.0

### Added

*Added possibility to pass arguments to executeScript #128 (@drauch)

Release date: 2015-01-26


# Version 2.7.0

### Added (incl Coypu.WatiN 2.2.0)

* ElementScope gets Disabled property - Closes #123

Release date: 2014-12-12


# Version 2.6.1	

### Fixed

* Fixes #121 - stale element ref exception calling FindAllCss/XPath on stale outer scope

Release date: 2014-12-03


# Version 2.6.0

### Fixed

* Handle stale elements/windows by exceptions rather than explicit stale checks - fixes #112 AcceptModalDialog() fails in IE11
* Override timeout in Exists() and Missing()
* NoAlertPresentException when Disposing Chrome Browser
* ElementScope's properties (HTML attribute access) race condition - fixes #117

Release date: 2014-11-30

# Version 2.5.1

### Fixed

* Fix AppHost to accept basic auth userinfo

# Version 2.5.0

### Added

* Added CLSCompliant(true) to all production assemblies. (@drauch)

### Fixed

* Firefox webdriver 2.43.0 has started closing alerts when asked for currentWindowHandle
			
### Changed

* Thanks to fix for Firefox Webdriver, if you switch window via the native selenium driver
coypu will no longer be able to switch back to the correct window if you use a pre-existing scope
so use the native SwitchTo with caution. 

# Version 2.4.1

### Fixed

* Only switch to window scope when necessary
* Support more html5 input types, fixes #104

Release date: 2014-07-25


# Version 2.4.0

### Added

*  Parameterized BrowserSession to take in a Driver (@Br3ttl3y)

Release date: 2014-07-25


# Version 2.3.0

### Added

* Upgrade to WebDriver 2.41
* Android dri	ver removed from WebDriver 2.41

Release date: 2014-04-01


# Version 2.2.0

### Added

* Scope.FindIdEndingWith() for Asp.Net Web Forms
* ElementScope.SelectOption() for finding a select then picking an option later

Release date: 2014-03-18


# Version 2.1.4

### Fixed

* Fixed issue where Options.ToString() was causing a stack overflow fixes #85 (@adexios)

Release date: 2014-02-12


# Version 2.1.3

### Fixed

* Fix null reference in queries with snapshot scopes (FindALL) fixes #84

Release date: 2014-02-04


# Version 2.1.2

### Fixed

* Find select and options seperately to allow differing TextPrecisions when PreferExact

Release date: 2014-01-31


# Version 2.1.1

### Fixed

* Exact text not matching selects found by container labels

Release date: 2014-01-31


# Version 2.1.0

### Changed

* NUnit matchers split out into seperate package Coypu.NUnit and Coypu.NUnit262 for those that really need NUnit 2.6.2

Release date: 2014-01-30


# Version 2.0.1

### Added

* New Exact and Match options (http://www.featurist.co.uk/blog/2014/01/22/coypu-2-0/)

### Changed

* Exact = false by default - all finders match partial text by default
* Match.Single by default - more than one matching element throws AmbiguousException
* Depend on same major version of dependencies

### Fixed

* On individual calls, only specified options override session configuration
 
### Removed

* HasCss, HasXPath etc - Just use .Exists()/.Missing() on any scope or Assert.That(scope, Shows.Css(".summat"))

Release date: 2014-01-02

# Version 1.0.0

### Updated

* Selenium WebDriver 2.39.0

Release date: 2013-12-20


# Version 0.23.1

### Fix

* Stack overflow in Shows.CssContaining :o

Release date: 2013-11-21


# Version 0.23.0

### Added

* History: BrowserWindow.GoBack() and BrowserWindow.GoForward()

Release date: 2013-11-18


# Version 0.22.2

### Fixed

* Actual values in matcher feedback could be wrong
* Fix for text of an iframe

Release date: 2013-11-08


# Version 0.22.1

### Fixed

* Radio buttons should NOT be found by name

Release date: 2013-11-06


### Added

* Support for url input type (@ajh79)
* Shows.Value; Shows.No.Value; HasValue; HasNoValue on ElementScope (@kberridge)
* BrowserWindow.Refresh() (@stefanforsberg)

### Updated

* Latest NUnit

Release date: 2013-11-06


# Version 0.21.1

### Fixed

* Correct dependency on Selenium WebDriver 2.37.0

Release date: 2013-10-29

# Version 0.21.0

Release date: 2013-10-28

### Added

* InnerHTML and OuterHTML properties
* NotSupportedException finding invisible elements by text in WebDriver

### Updated

* Selenium WebDriver 2.37.0

### Fixed

* Null reference exception when waiting for invisible elements to appear

# Version 0.20.1

Release date: 2013-10-18

### Fixed

* fixes #70 Inputs with no type attribute can now be found by name/id/placeholder

# Version 0.20.1

Release date: 2013-10-18

### Added

* Shows.AllCssInOrder and ContainsCss matchers for assertions against multiple css matches

# Version 0.19.3

Release date: 2013-09-26

### Updated

* Performance improvements for WebDriver

# Version 0.19.2

Release date: 2013-09-25

### Fixed

* Find state (no options) was not respecting session configuration


# Version 0.19.1

Release date: 2013-09-10

### Updated

* Selenium WebDriver 2.35.0


# Version 0.19.0

Release date: 2013-07-31

### Added

* Windows found by title contains if no exact match

### Updated

* Selenium WebDriver 2.33.0
* Refactoring to single xpaths for performance & standardisation across drivers

### Removed

* Fields and buttons are no longer found by ID ends with, only an exact match of ID


# Version 0.18.0

Release date: 2013-06-18

### Added

* SaveScreenshot(fileName)


# Version 0.17.0

Release date: 2013-06-17

### Added

* MaximiseWindow 

#### Fixed

* Visit now operates on the correct window in all browsers
* WatiNDriver gets a bunch of fixes for window scope (ExecuteScript, Dialogs, Visit)
* WatiNDriver now supports Title and Location
* WatinDriver now passes the "tricky" window tests i.e. window references survive popups being closed and reopened just like the selenium driver does.


# Version 0.16.3

Release date: 2013-06-04

### Removed

* No longer a dependency on Newtonsoft.Json


# Version 0.16.2

Release date: 2013-05-07

### Updated

* Selenium.WebDriver 2.32.1
* WebDrver selects now use Selenium.Support
* iframes found by css/xpath/id can be used as scopes (WebDriver only)


# Version 0.16.1

Release date: 2013-04-22

### Fixed

* FindCss with exact text now escapes regex chars correctly


# Version 0.16.0

Release date: 2013-04-19

### Added

* FindCss can now take required text for it to match an element
* HasCss and HasNoCss use get text filter via FindCss, and matchers


# Version 0.15.2

Release date: 2013-04-18

### Fixed

* One more place left to clean up forceAllEvents


# Version 0.15.1

Release date: 2013-04-18

### Fixed

* Left forceAllEvents hanging in the API by mistake

# Version 0.15.0

Release date: 2013-04-18

### Added

* FindAllCss and FindAllXPath now take a predicate which will be retried

### Removed

* Remove default fill in via Javascript. Native key events always fire now.


# Version 0.14.0

Release date: 2013-03-20

### Added

* Added FillInWith/Check/Uncheck to ElementScope (@refractalize)

### Removed

* BrowserSession.FillIn(ElementScope) (@refractalize)


# Version 0.13.0

Release date: 2013-03-07

### Added

* WebDriver now mostly supports PhantomJS so added to Coypu Browsers. Tests for iframes and modal dialogs fail in SeleniumWebdriver+PhantomJS

# Version 0.12.8

Release date: 2012-03-07

### Updated

* Configuration.AppHost may contain http:// or https://

# Version 0.12.7

Release date: 2013-03-06

### Fixed

* WebDriver dependency is now correct: 2.31.1

# Version 0.12.6

Release date: 2013-03-04	

### Updated

* WebDriver 2.31.1

# Version 0.12.5

Release date: 2013-02-22

# Added Shows.Content and Shows.No.Content NUnit matchers

# Version 0.12.4

Release date: 2013-02-15

### Updated

* WebDriver 2.29.1

# Version 0.12.3

Release date: 2012-12-12

### Updated

* WebDriver 2.28

# Version 0.12.2

Release date: 2012-12-04

### Fixed

* Fixes #37 WATIN respect forceAllEvents (@AidenMontgomery)
* Fixes #36 Allow interaction with input fields of type tel (@AidenMontgomery)

# Version 0.12.1

Release date: 2012-11-28

### Updated

* Any element with class 'btn' is considered a button to match bootstrap convention

# Version 0.12.0

Release date: 2012-11-13

### Fixed

* FillIn now works when passed and ElementScope

### Added

* SendKeys added to ElementFound

# Version 0.11.1

Release date: 2012-11-13

### Updated

* FillIn only clicks on element first when forceAllEvents is set

# Version 0.11.0

Release date: 2012-11-07

### Updated

* FindAll API now returns a list of snapshot element scopes

# Version 0.10.3

Release date: 2012-11-07

### Updated

* WebDriver 2.26.0

### Fixed

* Dialogs scoped correctly to windows
  - Fixes non user-triggered dialogs accessed directly after interacting with a different window

# Version 0.10.2

### Updated

* WebDriver 2.25.1
* When a window is missing a MissingWindowException is thrown, rather than MissingHtmlException

# Version 0.10.0

Release date: 2012-06-20

### Updated

* WebDriver 2.23
* Elements are only found by partial ID if there is a leading underscore 
  - This was only intended for asp.net webforms testing and was causing too many collisions
* Removed all Obsolete methods left over from < 0.8.0
  
# Version 0.8.10

Release date: 2012-05-01

### Added

* Expose window title

# Version 0.8.9

Release date: 2012-04-24

### Fixed

* Dependency on WebDriver 2.21 broken

# Version 0.8.8

Release date: 2012-04-23

### Added

* Browser.Parse() helper for dynamic configuration of browser

# Version 0.8.7

Release date: 2012-04-23

### Added

* FindFrame finds both iframes and old school frameset frames, FindIframe is now obsolete.

### Fixed

* Location respects window scope in all cases

# Version 0.8.6

Release date: 2012-04-18

### Updated

* WebDrier 2.21 - Fixes hover

# Version 0.8.5

Release date: 2012-04-12

### Fixed

* Coypu.xml not being packaged

# Version 0.8.4

Release date: 2012-04-12

### Fixed

* Finds fields with type=email 

# Version 0.8.3

Release date: 2012-04-11

### Fixed

* Only yse JS to fillin in js-supporting drivers, i.e. not HtmlUnit

# Version 0.8.2

Release date: 2012-04-05

### Fixed

* FillIn now uses JS by default for speed in WebDriver.

# Version 0.8.1

Release date: 2012-04-05

### Updated

* FillIn now uses JS by default for speed in WebDriver.

### Fixed

* Fill in making unneccassary driver calls.

# Version 0.8.0

Release date: 2012-04-03

** NOTE: Breaking changes **

Check the README for the latest API

### Removed
* Static configuration and static Browser.Session - use new BrowserSession(Configuration) and manage lifetime yourself.

### Updated
* Find methods return ElementScope with deferred execution. This replaces the browser.Within() functionality.
* Timing Options can be passed to every method to override configuration. This replaces browser.WithIndividualTimeout()

### Added
* Nested scopes
* Windows as scopes

# Version 0.7.3

Release date: 2012-03-20

### Fixed
* Quit not Close + Dispose fixes HtmlUnit & Chrome not closing properly

# Version 0.7.2

Release date: 2012-02-27

### Updated
* Minimum Selenium.WebDriver is now 2.19.0

### Added to Coypu.WatiN 0.6.0
* Watin will now consider invisible elements (citizenmatt)
* Watin can now find by css (citizenmatt)
* Watin handles FindField and partial ID correctly (citizenmatt)
* Watin - Only buttons and fields now match by partial id (citizenmatt)
* Watin frames can now be used as scopes (citizenmatt)
* Watin - Find frames support (citizenmatt)
* Watin - Options in a select list are now selected by text or value (citizenmatt)
* WatiNDriver now supports cookies (citizenmatt)
* WatiNDriver now supports hover (citizenmatt)
* Added support for Accept/CancelModalDialog to WatiNDriver (citizenmatt)
* Added HasDialog support to WatiNDriver (citizenmatt)

### Changed
* Selenium 2.11.0 or later

# Version 0.7.0

Release date: 2011-02-01

### Added
* Simple support for multiple sessions

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


