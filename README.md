# Coypu

Coypu supports browser automation in .Net to help make tests readable, robust, fast to write and less tightly coupled to the UI. If your tests are littered with sleeps, retries, complex XPath expressions and IDs dug out of the source with FireBug then Coypu might help.

Coypu is on Nuget:

PM> Install-Package Coypu

Discuss Coypu and get help on the [Google Group](http://groups.google.com/group/coypu)

## Coypu is
* A robust wrapper for browser automation tools on .Net, such as Selenium WebDriver that eases automating ajax-heavy websites
* A more intuitive DSL for interacting with the browser in the way a human being would, inspired by the ruby framework Capybara - http://github.com/jnicklas/capybara

## Coypu is not
* A port of Capybara to .Net
* An acceptance testing framework

## Demo

Check out a [demo of Coypu](http://skillsmatter.com/podcast/open-source-dot-net/london-dot-net-user-group-may) from a talk given at Skills Matter in May 2011.

## Using Coypu

### Configuration

#### Website under test

Configure the website you are testing as follows

	Configuration.AppHost = "autotrader.co.uk";
	Configuration.Port = "5555";
	Configuration.SSL = true|false;
	
If you don't specify any of these, Coypu will default to http, localhost and port 80.

#### Driver

Coypu drivers implement the `Coypu.Driver` interface and read the `Configuration.Browser` setting to pick the correct browser.

`Coypu.Drivers.Selenium.SeleniumWebDriver` uses Selenium 2.0rc3 and supports Firefox (fastest), IE (slowest) and Chrome as the browser.

##### Firefox

Versions up to and including Firefox 5 are now supported.

##### Internet Explorer
Only IE9 supports CSS & XPath and certain HTML features.

##### Chrome
You will need the chromedriver.exe on your PATH or in the bin of your test project. While it comes packaged in the Coypu download zip, it is not in the Nuget package. See: http://code.google.com/p/selenium/wiki/ChromeDriver.

The Coypu WatiN-IE driver is further behind (though it is faster than WebDriver-IE) and could use some more features implementing. 

Check the driver_test_results.txt file for the latest report on driver/browser support.

A goal for the future would be a driver for a headless browser such as zombie.js - http://zombie.labnotes.org/

Choose your driver/browser combination like so:

	Configuration.Driver = typeof (SeleniumWebDriver);
	Configuration.Browser = Drivers.Browser.Firefox;
 
These settings are the default configuration.

#### Timeout

Most of the methods in the Coypu DSL are automatically retried on any driver error until a configurable timeout is reached. It doesn't try to monitor XmlHttpRequests or hook into any ready events, just catches exceptions -- mainly the `Coypu.Drivers.MissingHtmlException` that a driver should throw when it cannot find something, but also any internal driver errors that the driver might throw up. 

This is a rather blunt approach but the only truly robust method of using Selenium WebDriver against heavily asynchronous websites that we have found.

All methods use this wait and retry strategy *except*: `Visit()`, `FindAllCss()` and `FindAllXPath()` which call the driver once immediately.

Setup timeout/retry like so:

	Configuration.Timeout = TimeSpan.FromSeconds(10);
	Configuration.RetryInterval = TimeSpan.FromSeconds(0.1);
	
These settings are the default configuration.

### Visible elements

Coypu drivers filter out any elements that are not visible on the page -- this includes hidden inputs. 

Non-visible elements can get in the way of finding the elements that we are really looking for and cause often errors when trying to interact with them. 

What we are really trying to do here is interact with the browser in the way that a human would. It's probably best to avoid hacking around with elements not accessible to the user where possible to avoid invalidating our tests in any case.

### Disclaimer

Coypu is still rather new. While we have started using it internally, there is plenty it doesn't cover yet. It is pretty well tested however.

If there's something you need that's not part of the DSL then please you may need to dive into the native driver which you can always do by casting the native driver to whatever underlying driver you know you are using:

	var selenium = ((OpenQA.Selenium.Remote.RemoteWebDriver) Browser.Session.Native);
	
But if you need to do this, please consider forking Coypu, adding what you need and sending a pull request. Thanks!

### DSL

Here are some examples to get you started using Coypu

	using Coypu;

#### Browser session

Access the current browser session via the static `Browser.Session`. This will open a browser if required.

	var browser = Browser.Session;
	
If you need to close the browser and start a new session next time `Browser.Session` is accessed then it's:

	Browser.EndSession();

or just dispose the current session like so:

	browser.Dispose();

or so:

	using (var browser = Browser.Session) 
	{
		...		
	}
	
#### Navigating
	
	browser.Visit("/used-cars")
	
If you need to step away and visit a site outside of the `Configuration.AppHost` then you can use a fully qualified Uri:

	browser.Visit("https://gmail.com")
	browser.Visit("file:///C:/users/adiel/localstuff.htm")

#### Completing forms

Form fields are found by label text, partial label text, id, name, placeholder or radio button value
	
	// Drop downs
	browser.Select("toyota").From("make");
	
	// Text inputs
	browser.FillIn("keywords").With("hybrid");
	
	// File inputs
	browser.FillIn("Avatar").With(@"c:\users\adiel\photos\avatar.jpg");
	
	// Radio button lists
	browser.Choose("Trade");
	browser.Choose("Private");
	
	// Checkboxes
	browser.Check("Additional ads")
	browser.Uncheck("Additional ads")	

To help with asp.net testing, if there is no matching element based on the rules above then an element that ends with the locator will be matched.

	browser.Check("radioTrade"); // matches id="_00123_some_aspnet_webforms_crap_radioTrade";
	
#### Clicking

Buttons are found by value/text, id or name. 

	browser.ClickButton("Search");
	browser.ClickButton("search-used-vehicles");
	
To help with asp.net testing, if none of these match an id that ends with the locator will be matched.
	
	browser.ClickButton("btnSearch"); // matches id="_00123_some_aspnet_webforms_crap_btnSearch";
	
Links are found by the text of the link

	browser.ClickLink("Reset search");

Click any other element by passing a function that tells Coypu how to find it:
	
	browser.Click(() => browser.FindCss("span#i-should-be-a-link"));

The above is the recommended, robust way to click, as Coypu can retry the find and click together if there is an exception after the element is found as the driver goes to click it.
	
Alternatively you can pass an element you have already found directly to `Click()`:

	var allToClick = browser.FindAllCss("span.clickable")
	foreach(var element in allToClick)
	{
		browser.Click(element);
	}
	
#### Finding single elements

Find methods return the first matching `Coypu.Element`. The locator arguments are case sensitive.

	var element = browser.FindField("Username");
	var element = browser.FindButton("GO");
	var element = browser.FindLink("Home");
	var element = browser.FindCss("table#menu");
	var element = browser.FindXPath("Username");

#### Finding multiple elements	
	
FindAll methods return all matching elements:

	foreach(var link in browser.FindAllCss("a")) 
	{
		var attributeValue = a["href"];
		...
	}

#### Hover

Hover over an element by passing a function that tells Coypu how to find it:

	browser.Hover(() => browser.FindCss("span#hoverOnMe"));
	
#### Scope

When you want perform operations only within a particular part of the page define a scope by using Within:

	browser.Within(() => browser.FindCss("form.searchForm"), () =>
	{
		browser.FillIn("postcode").With("N1 1AA");

		browser.Select("citroen").From("make");
		browser.Select("c4_grand_picasso").From("model");

		browser.FillIn("Add keyword:").With("vtr");

		browser.ClickButton("Search");
	}
	
The first parameter is a `Func<Element>` which allows you to find a `Coypu.Element` in any way you see fit. 

The actual finding of this element is deferred by the driver until each and every time it tries to find any element inside the Within block.

If you are used to the Capybara implementation of within then there is a subtle difference here. If the scope Element itself drops out of the DOM and then reappears part way through executing the within block (due to Ajax or page refreshes) then Coypu will just find it again (with all the usual wait & retries) and carry on regardless.

NOTE: Nested scopes are not currently supported - the inner scope will simply replace the outer scope at the moment.

#### Fieldsets / Sections

To find this:

	<fieldset>	
		<legend>Advanced search</legend>
		...
	</fieldset>

use this:	
	
	var element = browser.FindFieldset("Advanced search");
	
To find this:

	<div>	
		<h2>Search results</h2>
		...
	</div>

or this:

	<section>
		<h3>Search results</h3>
		...
	</section>

use this:
	
	var element = browser.FindSection("Search results");

**These work particularly well when used as scopes:**

	browser.WithinFieldset("Advanced search", () =>
	{
		browser.FillIn("First name").With("Philip");
		browser.FillIn("Middle initial").With("J");
		browser.FillIn("Last name").With("Fry");

		browser.Click("Find");
	});

or:	
	
	browser.WithinSection("Search results", () =>
	{
		Assert.That(browser.HasContent("1 friend found"));
		Assert.That(browser.HasContent("Philip J Fry"));
	});	

#### Scoping within iframes

To restrict the scope to an iframe, locate the iframe by its id, title or the text of an h1 element within the frame:

	browser.WithinIFrame("@coypu_news on Twitter", () =>
	{
		Assert.That(browser.HasContent("Coypu 0.2.0 released"));
	});	
	
#### Executing javascript in the browser

You can execute javascript like so:

	browser.ExecuteScript("document.getElementById('SomeContainer').innerHTML = '<h2>Hello</h2>';");
	
Anything is returned from the javascript will be returned from `browser.ExecuteScript`

	var innerHtml = browser.ExecuteScript("return document.getElementById('SomeContainer').innerHTML;");
	
#### Querying

Look for text anywhere in the page:

	bool hasContent = browser.HasContent("In France, the coypu is known as a ragondin");
	bool hasContent = browser.HasContentMatch("In [Ss]pain, the coypu is known as a (\w*)");
	
Check for the presence of an element:

	bool hasElement = browser.HasCss("ul.menu > li");
	bool hasElement = browser.HasXPath("//ul[@class = 'menu']/li");
	
The positive queries above will wait up to the configured timeout for a matching element to appear and return as soon as it does.

The negative versions will wait for the element NOT to be present:

	bool hasNoContent = browser.HasNoContent("In France, the coypu is known as a ragondin");
	bool hasNoElement = browser.HasNoCss("ul.menu > li");
	bool hasNoElement = browser.HasNoXPath("//ul[@class = 'menu']/li");

N.B: Use the version you are expecting to ensure your test returns fast under normal circumstances

#### Dialogs

Check for the presence of a modal dialog with expected text:

	bool hasDialog = browser.HasDialog("Are you sure you want to cancel your account?");
	bool hasNoDialog = browser.HasDialog("Are you sure you want to cancel your account?");
	
Waits are as for the other Has/HasNo methods.

Interact with the current dialog like so:

	browser.AcceptDialog();
	browser.CancelDialog();
  
#### Varying the timeout

When you need an unusually long (or short) timeout for a particular interaction you can use `WithIndividualTimeout`, temporarilly overriding the `Configuration.Timeout`:

	browser.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
	browser.Click("Upload");
	browser.WithIndividualTimeout(Timespan.FromSeconds(60), () => browser.HasContent("File bigfile.mp4 (10.5mb) uploaded successfully"));
	
## More tricks/tips

So, you are using Coypu but sometimes links or buttons still don't seem to be clicked when you expect them to. Well there are a couple more techniques that Coypu can help you with in this situation. 

If the driver reports it had found and clicked your element successfully but nothing happens then it may simply be that your app isn't wiring up events at the right time. But if you have exhausted this angle and cannot fix the problem in the site itself, then you could try a couple of things:

#### Tell Coypu to keep clicking at regular intervals until you see the result you expect:

	var until = () => browser.FindCss("#SearchResults");
	var waitBetweenRetries = TimeSpan.Seconds(2);

	browser.ClickButton("Search", until, waitBetweenRetries);

This is far from ideal as you are coupling the click to the expected result rather than verifying what you expect in a separate step, but as a last resort we have found this useful.

#### Tell Coypu to wait a short time between first finding links/buttons and clicking them:

	Configuration.WaitBeforeClick = Timespan.FromMiliseconds(0.2);
		
WARNING: Setting this in global config means adding time to *every* click in your tests, so again this should be used as a last resort and kept as low as possible.

## License

(The MIT License)

Copyright &copy; ITV plc 2011

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the 'Software'), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 