# Coypu is
* A robust wrapper for browser automation tools (e.g. Selenium WebDriver) to ease automating js/ajax-heavy websites from .Net
* A more intuitive DSL for interacting with the browser, inspired by the ruby framework Capybara - http://github.com/jnicklas/capybara

# Coypu is not
* A port of Capybara to .Net
* A headless browser

It is targetted at people doing browser automation in .Net with Selenium WebDriver (or other drivers) to help make your tests readable, robust and fast to write.

## Using Coypu

### Configuration

#### Driver

Coypu drivers must implement the `Coypu.Driver` interface and read the `Configuration.Browser` setting to test the correct browser. So far `Coypu.Drivers.Selenium.SeleniumWebDriver` is the only driver.

Choose your driver/browser combination like so:

	Configuration.RegisterDriver = typeof (SeleniumWebDriver);
	Configuration.Browser = Drivers.Browser.Firefox;`

#### Timeout

Most of the methods in the Coypu DSL are automatically retried on any driver error until a configurable timeout is reached. It doesn't try to monitor XmlHttpRequests or hook into any ready events, just catches exceptions -- mainly the `Coypu.Drivers.MissingHtmlException` that a driver should throw when it cannot find something, but also any internal driver errors that the driver might throw up. 

This is a rather blunt approach but the only truly robust method of using Selenium WebDriver that we have found. It also frees the driver from any concerns except for interacting with the browser in its current state.

All methods use this wait and retry strategy EXCEPT: `Visit()`, `FindAllCss()` and `FindAllXPath()` which call the driver once immediately. In the case of the FindAll methods this gives you a snapshot of the current state. TODO: add a WaitForAtLeast(n) wrapper for FindAll methods.

Setup timeout/retry like so:

	Configuration.Timeout = TimeSpan.FromSeconds(5);
	Configuration.RetryInterval = TimeSpan.FromSeconds(0.5);

### Visible elements

Coypu drivers filter out any elements that are not visible on the page -- this includes hidden inputs. 

Non-visible elements can get in the way of finding the elements that we are really looking for and cause often errors when trying to interact with them. 

What we are really trying to do here is interact with the browser in the way that a human would. Once we start hacking around with elements not accessible to the user then we invalidate our tests in any case.

### DSL

Here are some examples to get you started using Coypu

#### Visiting

	using Coypu;
	
	Browser.Session.Visit("http://www.autotrader.co.uk/used-cars") // TODO: Configure the host globally then Visit("/used-cars")

#### Completing forms

Form fields are found by label text, partial label text, id, name, placeholder or radio button value

	var browser = Browser.Session;
	
	// Drop downs
	browser.Select("toyota").From("make");
	
	// Text inputs
	browser.FillIn("keywords").With("hybrid");
	
	// Radio button lists
	browser.Choose("Trade");
	browser.Choose("Private");
	
	// Checkboxes
	browser.Check("Additional ads")
	browser.Uncheck("Additional ads")	

#### Clicking

Buttons are found by value/text, id or name

	browser.ClickButton("Search");
	browser.ClickButton("search-used-vehicles");
	
Links are found by the text of the link

	browser.ClickButton("Reset search");

#### Finding single nodes

TODO

#### Finding all matching nodes

TODO

#### Inspecting nodes

TODO

#### Inspecting page content

TODO

#### Dialogs

TODO
