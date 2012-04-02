# Coypu

Coypu supports browser automation in .Net to help make tests readable, robust, fast to write and less tightly coupled to the UI. If your tests are littered with sleeps, retries, complex XPath expressions and IDs dug out of the source with FireBug then Coypu might help.

Coypu is on Nuget:

PM> Install-Package Coypu

Discuss Coypu and get help on the [Google Group](http://groups.google.com/group/coypu)

## Coypu is
* A robust wrapper for browser automation tools on .Net, such as Selenium WebDriver that eases automating ajax-heavy websites and reduces coupling to the HTML, CSS & JS
* A more intuitive DSL for interacting with the browser in the way a human being would, inspired by the ruby framework Capybara - http://github.com/jnicklas/capybara

## Demo

Check out a [demo of Coypu](http://skillsmatter.com/podcast/open-source-dot-net/london-dot-net-user-group-may) from a talk given at Skills Matter in May 2011.

## Using Coypu

#### Browser session

Open a browser session like so:

	var browser = new BrowserSession();
	
When you are done with the browser session:

	browser.Dispose();

or:

	using (var browser = new BrowserSession()) 
	{
		...
	}
	
### Configuration

To configure Coypu pass an instance of `Coypu.SessionConfiguration` to the constructor of BrowserSession:

    var browserSession = new BrowserSession(new SessionConfiguration{...});

#### Website under test

Configure the website you are testing as follows

	var sessionConfiguration = new SessionConfiguration 
	{
	  AppHost = "autotrader.co.uk",
	  Port = "5555",
	  SSL = true|false
	};

If you don't specify any of these, Coypu will default to http, localhost and port 80.

#### Driver

Coypu drivers implement the `Coypu.Driver` interface and read the `SessionConfiguration.Browser` setting to pick the correct browser.

Choose your driver/browser combination like so:

	sessionConfiguration.Driver = typeof (SeleniumWebDriver);
	sessionConfiguration.Browser = Drivers.Browser.Firefox;
 
These settings are the default configuration.

##### Selenium WebDriver
`Coypu.Drivers.Selenium.SeleniumWebDriver` tracks the latest version of WebDriver and supports Firefox, IE (slowest) and Chrome (Fastest) as the browser. Any other Selenium implementation of RemoteWebDriver can be configured by subclassing `SeleniumWebDriver` and passing an instance of RemoteWebDriver to the base constructor.

The Selenium Driver is included in the Coypu package.

###### Firefox
WebDriver is generally stable with the last but one release of FireFox in my experience

###### Internet Explorer

You will need the new standalone InternetExplorerDriver.exe in your PATH or in the bin of your test project. [Download from google code](http://code.google.com/p/selenium/wiki/InternetExplorerDriver)

Only IE9 supports CSS & XPath and certain HTML features. The WatiN driver is notably faster in IE than the WebDriver IE driver, so is recommended for testing in Internet Explorer. The WatiN driver comes in a seperate package (see below).

###### Chrome
You will need the chromedriver.exe on your PATH or in the bin of your test project. [Download from google code](http://code.google.com/p/chromedriver/downloads/list)

###### HtmlUnit
You can run the headless HtmlUnit driver for Selenium on windows too, you just need to run up HtmlUnit in java:

1. Configure Coypu for HtmlUnit/HtmlUnitWithJavascript: `sessionConfiguration.Browser = Drivers.Browser.HtmlUnit/HtmlUnitWithJavascript;`
2. Install a JRE
3. Download the Selenium Server (selenium-server-standalone-x.x.x.jar) from [Selenium HQ](http://seleniumhq.org/download)
4. Run "java -jar selenium-server-standalone-x.x.x.jar"

And off you go.

###### Android
Selenium WebDriver also supports Android so long as you have the Android remote driver running (Selenium defaults to port 8080).

Check the driver_test_results.txt file for the latest report on driver/browser support.

##### WatiN

There is a seperate package called Coypu.WatiN containing a driver for WatiN which is now almost fully featured (thanks to citizenmatt) and runs considerably faster than the WebDriver IE driver.

This driver only supports Internet Explorer as the browser.

You will need to nuget `Install-Package Coypu.Watin` and then configure Coypu like so:

	sessionConfiguration.Driver = typeof (Coypu.Drivers.Watin.WatiNDriver);
	sessionConfiguration.Browser = Drivers.Browser.InternetExplorer;

#### Waits, retries and timeout

Most of the methods in the Coypu DSL are automatically retried on any driver error until a configurable timeout is reached. It just catches exceptions and retries -- mainly the `Coypu.Drivers.MissingHtmlException` that a driver should throw when it cannot find something, but also any internal driver errors that the driver might throw up. 

This is a rather blunt approach that goes well beyond WebDriver's ImplicitWait, for example, but the only truly robust strategy for heavily asynchronous websites, where elements are flying in and out of the DOM constantly, that I have found.

All methods use this wait and retry strategy *except*: `Visit()`, `FindAllCss()` and `FindAllXPath()` which call the driver once immediately.

Setup timeout/retry like so:

	sessionConfiguration.Timeout = TimeSpan.FromSeconds(1);
	sessionConfiguration.RetryInterval = TimeSpan.FromSeconds(0.1);
	
These settings are the default configuration.

All methods in the DSL take an optional final parameter of a `Coypu.Options`. By passing this in you can override these timing settings for just that call.

### Visible elements

Coypu drivers filter out any elements that are not visible on the page -- this includes hidden inputs. 

Non-visible elements can get in the way of finding the elements that we are really looking for and cause often errors when trying to interact with them. 

What we are really trying to do here is interact with the browser in the way that a human would. It's probably best to avoid hacking around with elements not accessible to the user where possible to avoid invalidating our tests in any case.

#### However...

If you really need this for some intractable problem where you cannot control the browser without cheating like this, then there is `sessionConfiguration/options.ConsideringInvisibleElements = true` which overrides this restriction.

### Missing features

If there's something you need that's not part of the DSL then please you may need to dive into the native driver which you can always do by casting the native driver to whatever underlying driver you know you are using:

	var selenium = ((OpenQA.Selenium.Remote.RemoteWebDriver) browserSession.Native);
	
But if you need to do this, please consider forking Coypu, adding what you need and sending a pull request. Thanks!

### DSL

Here are some examples to get you started using Coypu
	
#### Navigating
	
	browser.Visit("/used-cars")
	
If you need to step away and visit a site outside of the `SessionConfiguration.AppHost` then you can use a fully qualified Uri:

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

Click any other element by calling the Click method on the returned `ElementScope`:
	
	browser.FindCss("span#i-should-be-a-link").Click();

In this example, due to the way Coypu defers execution of finders, the FindCss will also be retried, should the Click fail. For example if the DOM is shifting under the driver's feet, the link may have become stale after it is found but before the click is actioned while part of the page is reloaded.

This introduces the idea of `Scope`. The browser.Find methods return a Scope on which you may perform actions, or make further scoped queries. There is more on scope below.
	
The last way to click is to pass an element you have already found directly to `Click()`:

	var allToClick = browser.FindAllCss("span.clickable")
	foreach(var element in allToClick)
	{
		browser.Click(element);
	}
	
#### Finding single elements

Find methods return a `Coypu.ElementScope` that is scoped to the first matching element. The locator arguments are case sensitive.

	var element = browser.FindField("Username");
	var element = browser.FindButton("GO");
	var element = browser.FindLink("Home");
	var element = browser.FindCss("table#menu");
	var element = browser.FindXPath("Username");
	
You can read attributes of these elements like so:

    browser.FindLink("Home").Id
    browser.FindLink("Home").Text
    browser.FindLink("Home")["href"]
    browser.FindLink("Home")["rel"]

#### Finding multiple elements	
	
FindAll methods return all matching elements:

	foreach(var link in browser.FindAllCss("a")) 
	{
		var attributeValue = a["href"];
		...
	}

#### Hover

Hover over an element

	browser.FindCss("span#hoverOnMe").Hover();

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

#### Scope

When you want perform operations only within a particular part of the page, find the scope you want then use this as the scope for further finds and interactions as in the previous fieldset/section example.

    var advancedSearch = browser.FindFieldset("Advanced search");
    var searchResults = browser.FindSection("Search results");

    advancedSearch.FillIn("First name").With("Philip");
    advancedSearch.FillIn("Middle initial").With("J");
    advancedSearch.FillIn("Last name").With("Fry");

    advancedSearch.Click("Find");

    Assert.That(searchResults.HasContent("1 friend found"));
    Assert.That(searchResults.HasContent("Philip J Fry"));

The actual finding of the scope is deferred until the driver needs to interact with or find any element inside the Scope. If the scope becomes stale at any time it will be re-found.

**So in the above example, it doesn't matter what happens between clicking 'Find' and the search results loading. The search results area could be ripped out of the DOM and refreshed, there could be a full page refresh, or even a pop up window closed and reopened, so long as the session remains active.**

This means you have tests much more loosely coupled to the implementation of your website. Consider the search example above and the possible permutations of HTML and JS that would satisfy that test.

#### Scoping within iframes

To restrict the scope to an iframe, locate the iframe by its id, title or the text of an h1 element within the frame:

	var twitterFrame = browser.FindIFrame("@coypu_news on Twitter");

	Assert.That(twitterFrame.HasContent("Coypu 0.8.0 released"));	


#### Scoping within windows

To restrict the scope to a browser window (or tab), locate the window by its title or name:

	var surveyPopup = browser.FindWindow("Customer Survey");

	surveyPopup.Select("Not Satisfied").From("How did we handle your enquiry?");	
	surveyPopup.ClickButton("Submit");
	
	browser.ClickLink("Logout"); // Using the original window scope again - there is no need to switch back, just use the correct scope
	
Switching between frames and windows is a particular pain in WebDriver as you may well know. Check out this example of how Coypu handles windows from a Coypu acceptance test:

    browser.Visit("InteractionTestsPage.htm");

    browser.ClickLink("Open pop up window");

    var popUp = browser.FindWindow("Pop Up Window");
    var button = popUp.FindButton("button in popup");

    Assert.That(button.Exists());
    Assert.That(popUp.HasContent("I am a pop up window"));

    popUp.ExecuteScript("self.close()");

    Assert.That(button.Missing());

    browser.ClickLink("Open pop up window");

    Assert.That(popUp.HasContent("I am a pop up window"));
    Assert.That(button.Exists());
    
    button.Click();

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

When you need an unusually long (or short) timeout for a particular interaction you can override the timeout just for this call by passing in a `Coypu.Options` like this:

	browser.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
	browser.ClickButton("Upload");
	browser.HasContent("File bigfile.mp4 (10.5mb) uploaded successfully", new Options { Timeout = TimeSpan.FromSeconds(60) } );
	
#### Finding states (nondeterministic testing)

Sometimes you just can't predict what state the browser will be in. Not ideal for a reliable test, but if it's unavoidable then you can use the `Session.FindState` like this:

	var signedIn = new State(() => browser.HasContent("Signed in in as:"));
	var signedOut = new State(() => browser.HasContent("Please sign in"));
	
	if (browser.FindState(signedIn,signedOut) == signedIn) 
	{
	  browser.ClickLink("Sign out");
	}

It will return as soon as the first from your list of states is found, and throw if none of the states are found within the `SessionConfiguration.Timeout`

Avoid this:
  
	if (browser.HasContent("Signed in in as:")) 
	{
	  ...
	}
  
otherwise you will have to wait for the full `SessionConfiguration.Timeout` in the negitive case.  
  
## More tricks/tips

So, you are using Coypu but sometimes links or buttons still don't seem to be clicked when you expect them to. Well there are a couple more techniques that Coypu can help you with in this situation. 

If the driver reports it had found and clicked your element successfully but nothing happens then it may simply be that your app isn't wiring up events at the right time. But if you have exhausted this angle and cannot fix the problem in the site itself, then you could try a couple of things:

#### Tell Coypu to keep clicking at regular intervals until you see the result you expect:

	var until = () => browser.FindCss("#SearchResults").Exists();
	var waitBetweenRetries = TimeSpan.Seconds(2);

	browser.ClickButton("Search", until, waitBetweenRetries);

This is far from ideal as you are coupling the click to the expected result rather than verifying what you expect in a separate step, but as a last resort we have found this useful.

#### Tell Coypu to wait a short time between first finding links/buttons and clicking them:

	sessionConfiguration.WaitBeforeClick = TimeSpan.FromMilliseconds(0.2);
		
WARNING: Setting this in your session configuration means adding time to *every* click in that session. You might be better off doing this just when you need it:

    browser.ClickButton("Search", new Options { WaitBeforeClick = TimeSpan.FromMilliseconds(0.2) } )

## License

(The MIT License)

Copyright &copy; Adrian Longley, ITV plc & Contributors 2012

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the 'Software'), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
