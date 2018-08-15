# Coypu

> #### "Theirs not to reason why, Theirs but to do and retry"
> &mdash; <cite>Alfred, Lord Selenium

> #### "haha, coypu must have reduced teh amount of shit code by 90%"
> &mdash; <cite>Anonymous</cite>




Coypu supports browser automation in .Net to help make tests readable, robust, fast to write and less tightly coupled to the UI. If your tests are littered with sleeps, retries, complex XPath expressions and IDs dug out of the source with FireBug then Coypu might help.

Coypu is on Nuget:

    PM> Install-Package Coypu

NUnit matchers (e.g. `Assert.That(browserSession, Shows.Content("Hello world"));`) are in a separate package:

    PM> Install-Package Coypu.NUnit

or

    PM> Install-Package Coypu.NUnit262 (If you really need NUnit 2.6.2)

Discuss Coypu and get help on the [Google Group](http://groups.google.com/group/coypu)

## Coypu is
* A robust wrapper for browser automation tools on .Net, such as Selenium WebDriver that eases automating ajax-heavy websites and reduces coupling to the HTML, CSS & JS
* A more intuitive DSL for interacting with the browser in the way a human being would, inspired by the ruby framework Capybara - http://github.com/jnicklas/capybara

## Demo

Check out a [demo of Coypu](http://skillsmatter.com/podcast/open-source-dot-net/london-dot-net-user-group-may) from a talk given at Skills Matter in May 2011.

## Using Coypu

#### Browser session

Open a browser session like so:

```c#
var browser = new BrowserSession();
```
	
When you are done with the browser session:

```c#
browser.Dispose();
```

or:

```c#
using (var browser = new BrowserSession()) 
{
	...
}
```
	
### Configuration

To configure Coypu pass an instance of `Coypu.SessionConfiguration` to the constructor of BrowserSession:

```c#
var browserSession = new BrowserSession(new SessionConfiguration{...});
```

#### Website under test

Configure the website you are testing as follows

```c#
var sessionConfiguration = new SessionConfiguration 
{
  AppHost = "autotrader.co.uk",
  Port = 5555,
  SSL = true|false
};
```

If you don't specify any of these, Coypu will default to http, localhost and port 80.

#### Driver

Coypu drivers implement the `Coypu.Driver` interface and read the `SessionConfiguration.Browser` setting to pick the correct browser.

Choose your driver/browser combination like so:

```c#
sessionConfiguration.Driver = typeof (SeleniumWebDriver);
sessionConfiguration.Browser = Drivers.Browser.Firefox;
```
 
These settings are the default configuration.

If you want to configure these at runtime you could replace the following strings with strings read from your environment / configuration:

```c#
sessionConfiguration.Driver = Type.GetType("Coypu.Drivers.Selenium.SeleniumWebDriver, Coypu");
sessionConfiguration.Browser = Drivers.Browser.Parse("firefox");
```

##### Selenium WebDriver
`Coypu.Drivers.Selenium.SeleniumWebDriver` tracks the latest version of WebDriver and supports Firefox, IE (slowest) and Chrome (Fastest) as the browser. Any other Selenium implementation of RemoteWebDriver can be configured by subclassing `SeleniumWebDriver` and passing an instance of RemoteWebDriver to the base constructor.

The Selenium Driver is included in the Coypu package.

###### Firefox
WebDriver is generally stable with the last but one release of FireFox in my experience

###### Internet Explorer

You will need the new standalone InternetExplorerDriver.exe in your PATH or in the bin of your test project. [Download from google code](http://code.google.com/p/selenium/wiki/InternetExplorerDriver). NOTE: as of 2013-11-03, there are known serious performance issue with IE 64 bit driver against IE 10, consider using the 32 bit version where possible.

Only IE9 supports CSS & XPath and certain HTML features. The WatiN driver is notably faster in IE than the WebDriver IE driver, so is recommended for testing in Internet Explorer. The WatiN driver comes in a seperate package (see below).

###### Chrome
You will need the chromedriver.exe on your PATH or in the bin of your test project. [Download from google code](http://code.google.com/p/chromedriver/downloads/list)

###### PhantomJS
The headless webkit browser runs under Selenium WebDriver. It runs almost everything in Coypu, iframes are a particular problem where you might have to reach for chrome/firefox.

You will need phantomjs.exe on your PATH or in the bin of your test project. You can get this from nuget or from [phantomjs.org](http://phantomjs.org/download.html)

    PM> install-package phantomjs.exe

###### HtmlUnit
You can run the headless HtmlUnit driver for Selenium on windows too, you just need to run up HtmlUnit in java:

1. Configure Coypu for HtmlUnit/HtmlUnitWithJavascript: `sessionConfiguration.Browser = Drivers.Browser.HtmlUnit/HtmlUnitWithJavascript;`
2. Install a JRE
3. Download the Selenium Server (selenium-server-standalone-x.x.x.jar) from [Selenium HQ](http://seleniumhq.org/download)
4. Run "java -jar selenium-server-standalone-x.x.x.jar"

And off you go.

##### WatiN

There is a seperate package called Coypu.WatiN containing a driver for WatiN which is now almost fully featured (thanks to citizenmatt) and runs considerably faster than the WebDriver IE driver.

This driver only supports Internet Explorer as the browser.

You will need to nuget `Install-Package Coypu.Watin` and then configure Coypu like so:

```c#
	sessionConfiguration.Driver = typeof (Coypu.Drivers.Watin.WatiNDriver);
	sessionConfiguration.Browser = Drivers.Browser.InternetExplorer;
```

### SpecFlow scenarios

If you are using SpecFlow for your acceptance tests then you will probably want to configure it to provide a single Browser Session scoped to each scenario. SpecFlow supports some basic dependency injection which you can use to achieve this as shown in [this gist](https://gist.github.com/2301407).
  
### Waits, retries and timeout

Most of the methods in the Coypu DSL are automatically retried on any driver error until a configurable timeout is reached. It just catches exceptions and retries -- mainly the `Coypu.Drivers.MissingHtmlException` that a driver should throw when it cannot find something, but also any internal driver errors that the driver might throw up. 

This is a rather blunt approach that goes well beyond WebDriver's ImplicitWait, for example, but the only truly robust strategy for heavily asynchronous websites, where elements are flying in and out of the DOM constantly, that I have found.

All methods use this wait and retry strategy *except*: `Visit()`, `FindAllCss()` and `FindAllXPath()` which call the driver once immediately unless you supply a predicate to describe the expected state.

Configure timeout/retry like so:

```c#
sessionConfiguration.Timeout = TimeSpan.FromSeconds(1);
sessionConfiguration.RetryInterval = TimeSpan.FromSeconds(0.1);
```
	
These settings are the default configuration.

All methods in the DSL take an optional final parameter of a `Coypu.Options`. By passing this in you can override any of these timing settings for just that call:

So when you need an unusually long (or short) timeout for a particular interaction you can override the timeout just for this call by passing in a `Coypu.Options` like this:

```c#
browser.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
browser.ClickButton("Upload");
Assert.That(browser, Shows.Content("File bigfile.mp4 (10.5mb) uploaded successfully", new Options { Timeout = TimeSpan.FromSeconds(60) } ));
```

The options you specify are merged with your SessionConfiguration, so you only need specify those options you wish to override.

### Visible elements

Coypu drivers filter out any elements that are not visible on the page -- this includes hidden inputs. 

Non-visible elements can get in the way of finding the elements that we are really looking for and cause often errors when trying to interact with them. 

What we are really trying to do here is interact with the browser in the way that a human would. It's probably best to avoid hacking around with elements not accessible to the user where possible to avoid invalidating our tests in any case.

#### However...

If you really need this for some intractable problem where you cannot control the browser without cheating like this, then there is `sessionConfiguration/options.ConsideringInvisibleElements = true` which overrides this restriction.

### Can't find what you need?

If there's something you need that's not part of the DSL then please you may need to dive into the native driver which you can always do by casting the native driver to whatever underlying driver you know you are using:

```c#
var selenium = ((OpenQA.Selenium.Remote.RemoteWebDriver) browserSession.Native);
```
	
But if you need to do this, please consider forking Coypu, adding what you need and sending a pull request. Thanks!

### DSL

Here are some examples to get you started using Coypu
	
#### Navigating

```c#	
browser.Visit("/used-cars");
```
	
If you need to step away and visit a site outside of the `SessionConfiguration.AppHost` then you can use a fully qualified Uri:

```c#
browser.Visit("https://gmail.com");
browser.Visit("file:///C:/users/adiel/localstuff.htm");
```

To move back or forward in the browser history:

```c#
browser.GoBack();
browser.GoForward();
```

#### Getting the page title
```c#
browser.Title
```
	
#### Completing forms

Form fields are found by label text, id, name (except radio buttons), placeholder or radio button value
	
```c#
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
```

If you need to fall back to CSS or XPath you can do:

```c#
// Text/File inputs
browser.FindCss("input[type=text].keywords").FillInWith("hybrid")

// Checkboxes
browser.FindCss("input[type=checkbox].additional-ads").Check();
browser.FindCss("input[type=checkbox].additional-ads").Uncheck();
```

To restrict `FindCss()` to only elements matching some expected text you can do

```c#
browser.FindCss("ul.model li", text: "Citroen");
```
or

```c#
browser.FindCss("ul.model li", text: new Regex("Citroen C\d"));
```

#### Clicking

Buttons are found by value/text, id or name. 

```c#
browser.ClickButton("Search");
browser.ClickButton("search-used-vehicles");
```

Links are found by the text of the link

```c#
browser.ClickLink("Reset search");
```

Click any other element by calling the Click method on the returned `ElementScope`:

```c#	
browser.FindCss("span#i-should-be-a-link", text: "Log in").Click();
```

In this example, due to the way Coypu defers execution of finders, the FindCss will also be retried, should the Click fail. For example if the DOM is shifting under the driver's feet, the link may have become stale after it is found but before the click is actioned while part of the page is reloaded.

This introduces the idea of `Scope`. The browser.Find methods return a Scope on which you may perform actions, or make further scoped queries. There is more on scope below.
	
The last way to click is to pass an element you have already found directly to `Click()`:

```c#
var allToClick = browser.FindAllCss("span.clickable")
foreach(var element in allToClick)
{
	browser.Click(element);
}
```

#### Finding single elements

Find methods return a `Coypu.ElementScope` that is scoped to the first matching element. The locator arguments are case sensitive.

```c#
var element = browser.FindField("Username");
var element = browser.FindButton("GO");
var element = browser.FindLink("Home");
var element = browser.FindCss("table#menu");
var element = browser.FindXPath("Username");
var element = browser.FindId("myElementId");
```

**N.B. For Asp.Net Web Forms testing you may need this method:**

```c#
var element = browser.FindIdEndingWith("SubmitButton");
// Matches <button id="ctl00_MainContent_SubmitButton"/>
```
You can read attributes of these elements like so:

```c#
    browser.FindLink("Home").Id
    browser.FindLink("Home").Text
    browser.FindLink("Home")["href"]
    browser.FindLink("Home")["rel"]
```

#### Finding multiple elements	
	
FindAll methods return all matching elements immediately with no retry:

```c#
	foreach(var link in browser.FindAllCss("a")) 
	{
		var attributeValue = a["href"];
		...
	}
```

If you are expecting a particular state to be reached then you can describe this in a predicate and Coypu will retry until it matches.

	foreach(var link in browser.FindAllCss("a", (links) => links.Count() == 5)) 
	{
		var attributeValue = a["href"];
		...
	}

#### Matching exactly or allowing substrings

When finding elements by their text, the `TextPrecision` option allows you to specify whether to match exact text or allow a substring match. This can be set globally and also overridden on each and every call. `TextPrecision` has three options: `Exact`, `Substring` and `PreferExact`. The default is `PreferExact`.

`TextPrecision.Exact` will only match the entire text of an element exactly.

`TextPrecision.Substring` will allow you to specify a substring to find an element.

`TextPrecision.PreferExact` which will prefer an exact text match to a substring match. **This is the default for TextPrecision**

##### Usage

```c#
browserSession.FillIn("Password", new Options{TextPrecision = TextPrecision.Exact}).With("123456");
// or
browserSession.FillIn("Password", Options.Exact).With("123456");
```

This will be respected everywhere that Coypu matches visible text, including buttons, select options, placeholder text and so on, but not anywhere else, for example when considering ids or names of fields.

#### Behaviour when multiple elements match a query

When using methods such as `ClickLink()`, and `FillIn()`, what happens when more than one element matches? With the `Match` option you have control over what happens by choosing one of the two `Match` strategies:

`Match.Single` if there is more than one matching element a `Coypu.AmbiguousException` is thrown. **This is the default for Match**

`Match.First` just returns the first matching element.


##### Usage

```c#
browserSession.ClickButton("Close", new Options{Match = Match.First});
// or
browserSession.ClickButton("Close", Options.First);
```

#### Some more examples of using TextPrecision and Match

Say we had the HTML:

```html
Some <a href="x">good things</a> or even
awfully <a href="y">good things<a> are harder to explain than
less good <a href="z">things<a>.
```

and the code:

```c#
browserSession.ClickLink(text, options);
```

then as we vary the values of text and the options these would be the results:

| text          | Match  | TextPrecision | Result |
|---------------|--------|---------------|--------------------------------------------|
| "things"      | Single | Exact         | Clicks the link to 'z'                     |
| "things"      | Single | Substring     | Throws AmbiguousException                 |
| "things"      | Single | PreferExact   | Clicks the link to 'z'   - (**DEFAULT**)   |
| | | | |
| "things"      | First  | Exact         | Clicks the link to 'z'                     |
| "things"      | First  | Substring     | Clicks the link to 'x'                     |
| "things"      | First  | PreferExact   | Clicks the link to 'z'                     |
| | | | |
| "good things" | Single | Exact         | Throws AmbiguousException                 |
| "good things" | Single | Substring     | Throws AmbiguousException                 |
| "good things" | Single | PreferExact   | Throws AmbiguousException  - (**DEFAULT**)  |
| | | | |
| "good things" | First  | Exact         | Clicks the link to 'x'                     |
| "good things" | First  | Substring     | Clicks the link to 'x'                     |
| "good things" | First  | PreferExact   | Clicks the link to 'x'                     |

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

```c#
    var advancedSearch = browser.FindFieldset("Advanced search");
    var searchResults = browser.FindSection("Search results");

    advancedSearch.FillIn("First name").With("Philip");
    advancedSearch.FillIn("Middle initial").With("J");
    advancedSearch.FillIn("Last name").With("Fry");

    advancedSearch.Click("Find");

    Assert.That(searchResults, Shows.Content("1 friend found"));
    Assert.That(searchResults, Shows.Content("Philip J Fry"));
```

The actual finding of the scope is deferred until the driver needs to interact with or find any element inside the Scope. If the scope becomes stale at any time it will be re-found.

**So in the above example, it doesn't matter what happens between clicking 'Find' and the search results loading. The search results area could be ripped out of the DOM and refreshed, there could be a full page refresh, or even a pop up window closed and reopened, so long as the session remains active.**

This means you have tests much more loosely coupled to the implementation of your website. Consider the search example above and the possible permutations of HTML and JS that would satisfy that test.

#### Beware the XPath // trap

In XPath the expression // means something very specific, and it might not be what
you think. Contrary to common belief, // means "anywhere in the document" not "anywhere
in the current context". As an example:

```c#
browser.FindXPath("//body").FindAllXPath("//script");
```

You might expect this to find all script tags in the body, but actually, it finds all
script tags in the entire document, not only those in the body! What you're looking
for is the .// expression which means "any descendant of the current node":

```c#
browser.FindXPath("//body").FindAllXPath(".//script");
```

(from https://github.com/jnicklas/capybara#beware-the-xpath--trap)


#### Scoping within frames / iframes

To restrict the scope to a frame or iframe, locate the frame by its name,id, title or the text of an h1 element within the frame:

```c#
var twitterFrame = browser.FindFrame("@coypu_news on Twitter");

Assert.That(twitterFrame, Shows.Content("Coypu 0.8.0 released"));	
```

#### Scoping within windows

To restrict the scope to a browser window (or tab), locate the window by its title or name:

```c#
var surveyPopup = browser.FindWindow("Customer Survey");

surveyPopup.Select("Not Satisfied").From("How did we handle your enquiry?");	
surveyPopup.ClickButton("Submit");

browser.ClickLink("Logout"); // Using the original window scope again - there is no need to switch back, just use the correct scope
```

If no exact match is found Coypu will consider windows were the title contains the supplied value
	
Switching between frames and windows is a particular pain in WebDriver as you may well know. Check out this example of how Coypu handles windows from a Coypu acceptance test:

```c#
browser.Visit("InteractionTestsPage.htm");

browser.ClickLink("Open pop up window");

var popUp = browser.FindWindow("Pop Up Window");
var button = popUp.FindButton("button in popup");

Assert.That(button.Exists());
Assert.That(popUp, Shows.Content("I am a pop up window"));

popUp.ExecuteScript("self.close()");

Assert.That(button.Missing());

browser.ClickLink("Open pop up window");

Assert.That(popUp, Shows.Content("I am a pop up window"));
Assert.That(button.Exists());

button.Click();
```

**N.B.** If you drop down into the native Selenium driver and use SwitchTo() (highly unrecommended), bypassing Coypu's FindWindow(), Coypu will lose track of the current window, so make sure to switch back to the previous window before dropping back to Coypu.

#### Window size

Sometimes you need to maximise the window, or to set a particular width, perhaps for testing your responsive layout:

```c#
browser.MaximiseWindow();
browser.ResizeTo(768,1000);
```

If you are dealing with multiple windows, just call these on the correct scope:

```c#
browser.FindWindow("Pop Up Window").MaximiseWindow();
```

#### Executing javascript in the browser

You can execute javascript like so:

```c#
browser.ExecuteScript("document.getElementById('SomeContainer').innerHTML = '<h2>Hello</h2>';");
```
	
Anything is returned from the javascript will be returned from `browser.ExecuteScript`

```c#
var innerHtml = browser.ExecuteScript("return document.getElementById('SomeContainer').innerHTML;");
```
	
#### Querying

Look for text anywhere in the page:

```c#
bool hasContent = browser.HasContent("In France, the coypu is known as a ragondin");
```
	
Check for the presence of an element:

```c#
bool hasElement = browser.FindCss("ul.menu > li").Exists();
bool hasElement = browser.FindCss("ul.menu > li", text: "Home").Missing();

bool hasElement = browser.FindXPath("//ul[@class = 'menu']/li").Exists();
```
	
The positive queries above will wait up to the configured timeout for a matching element to appear and return as soon as it does.

The negative versions will wait for the element NOT to be present:

```c#
bool hasNoContent = browser.HasNoContent("In France, the coypu is known as a ragondin");

bool hasNoElement = browser.FindCss("ul.menu > li").Missing();
bool hasNoElement = browser.FindCss("ul.menu > li", text: "Admin").Missing();

bool hasNoElement = browser.FindXPath("//ul[@class = 'menu']/li").Missing();
```

There are also queries for the value of an input

```c#
bool hasValue = browser.FindField("total").HasValue("147");
bool hasNoValue = browser.FindField("total").HasNoValue("0");
```

#### Matchers

There are NUnit matchers for some of the queries above to help with your assertions:

```c#
Assert.That(browser, Shows.Content("In France, the coypu is known as a ragondin");
Assert.That(browser, Shows.No.Content("In France, the coypu is known as a ragondin");

Assert.That(browser, Shows.Css("ul.menu > li");
Assert.That(browser, Shows.Css("ul.menu > li", text: "Home");
Assert.That(browser, Shows.No.Css("ul.menu > li", text: "Admin");

Assert.That(browser, Shows.ContentContaining(Some","Words","Anywhere","in","the","document"))
Assert.That(browser, Shows.CssContaining("ul.menu > li","match","in","any","order"))
Assert.That(browser, Shows.AllCssInOrder("ul.menu > li","has","exactly","these","matches"))

Assert.That(browser.FindField("total"), Shows.Value("147"));
Assert.That(browser.FindField("total"), Shows.No.Value("0"));
```

#### Inner/OuterHTML

If you just want to grab the inner or outer HTML of an element to do your own queries and assertions you can use:

```c#
var outerHTML = browser.FindCss("table#myData").OuterHTML;
var innerHTML = browser.FindCss("table#myData").InnerHTML; // Will exclude the surrounding <table> ... </table>
```

#### Dialogs

Check for the presence of a modal dialog with expected text:

```c#
bool hasDialog = browser.HasDialog("Are you sure you want to cancel your account?");
bool hasNoDialog = browser.HasDialog("Are you sure you want to cancel your account?");
```
	
Waits are as for the other Has/HasNo methods.

Interact with the current dialog like so:

```c#
browser.AcceptDialog();
browser.CancelDialog();
```
	
#### Finding states (nondeterministic testing)

Sometimes you just can't predict what state the browser will be in. Not ideal for a reliable test, but if it's unavoidable then you can use the `Session.FindState` like this:

```c#
var signedIn = new State(() => browser.HasContent("Signed in in as:"));
var signedOut = new State(() => browser.HasContent("Please sign in"));

if (browser.FindState(signedIn,signedOut) == signedIn) 
{
  browser.ClickLink("Sign out");
}
```

It will return as soon as the first from your list of states is found, and throw if none of the states are found within the `SessionConfiguration.Timeout`

Avoid this:
  
```c#
if (browser.HasContent("Signed in in as:")) 
{
  ...
}
```
  
otherwise you will have to wait for the full `SessionConfiguration.Timeout` in the negitive case.

### Screenshots

If you can't get the quality of feedback from your tests you need to tell you exactly why they are failing you might need to take a screenshot:

```c#
browser.SaveScreenshot(@"c:\screenshots\my_feature\my_scenario_2013-06-18_16_53.jpg");
browser.FindWindow("Your Popup window").SaveScreenshot(etc.);
```

The image format will be determined by the file extension.

## Using a custom Selenium WebDriver profile

Sometimes you may want to define custom profile settings for your driver.

You can do this by creating your own driver that derives from `Coypu.Drivers.Selenium.SeleniumWebDriver` using something like this:

```c#
public class CustomFirefoxProfileSeleniumWebDriver : SeleniumWebDriver
{
    public CustomFirefoxProfileSeleniumWebDriver(yourCustomProfile)
        : base(CustomProfileDriver(yourCustomProfile), Browser.Firefox)
    {
    }

    private static RemoteWebDriver CustomProfileDriver(FirefoxProfile yourCustomProfile)
    {
        return new FirefoxDriver(yourCustomProfile);
    }
}
```

and either setting the Driver type on your SessionConfiguration and letting Coypu construct it like this:


or constructing the Driver yourself and passing in your own Driver instance:

```c#
[Test]
public void CustomProfile()
{
    var customProfile = new FirefoxProfile(); // Configure as you wish
    var customDriver = new CustomFirefoxProfileSeleniumWebDriver(customProfile);
    using (var custom = new BrowserSession(customDriver))
    {
        custom.Visit("http://www.featurist.co.uk/");
        // etc.
    }

    // Or if you need to 
}
```

**When you pass a custom driver, the Driver and Browser settings in ConfigurationSettings are ignored**

## Sauce Labs

Here is an example of using a custom driver to run tests in Sauce Labs (thanks to @Br3ttl3y for this):

```c#
[TestCase("Windows 7", "firefox", "25")]
[TestCase("Windows XP", "internet explorer", "6")]
public void CustomBrowserSession(string platform, string browserName, string version)
{
    var configuration = new SessionConfiguration { Driver = typeof(CustomDriver) };

    var desiredCapabilites = new DesiredCapabilities(browserName, version, Platform.CurrentPlatform);
    desiredCapabilites.SetCapability("platform", platform);
    desiredCapabilites.SetCapability("username", "...");
    desiredCapabilites.SetCapability("accessKey", "...");
    desiredCapabilites.SetCapability("name", TestContext.CurrentContext.Test.Name);

    Driver driver = new CustomDriver(Browser.Parse(browserName), desiredCapabilites);

    using (var custom = new BrowserSession(configuration, driver))
    {
        custom.Visit("https://saucelabs.com/test/guinea-pig");
        Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo("0"));
    }
}

public class SauceLabsDriver : SeleniumWebDriver
{
    public SauceLabsDriver(Browser browser, ICapabilities capabilities)
        : base(CustomWebDriver(capabilities), browser)
    {
    }

    private static RemoteWebDriver CustomWebDriver(ICapabilities capabilities)
    {
        var remoteAppHost = new Uri("http://ondemand.saucelabs.com:80/wd/hub");
        return new RemoteWebDriver(remoteAppHost, capabilities);
    }
}
```

## More tricks/tips

So, you are using Coypu but sometimes links or buttons still don't seem to be clicked when you expect them to. Well there are a couple more techniques that Coypu can help you with in this situation. 

If the driver reports it had found and clicked your element successfully but nothing happens then it may simply be that your app isn't wiring up events at the right time. But if you have exhausted this angle and cannot fix the problem in the site itself, then you could try a couple of things:

#### Tell Coypu to keep clicking at regular intervals until you see the result you expect:

```c#
var until = () => browser.FindCss("#SearchResults").Exists();
var waitBetweenRetries = TimeSpan.Seconds(2);

browser.ClickButton("Search", until, waitBetweenRetries);
```

This is far from ideal as you are coupling the click to the expected result rather than verifying what you expect in a separate step, but as a last resort we have found this useful.

#### Tell Coypu to wait a short time between first finding links/buttons and clicking them:

```c#
sessionConfiguration.WaitBeforeClick = TimeSpan.FromMilliseconds(0.2);
```
		
WARNING: Setting this in your session configuration means adding time to *every* click in that session. You might be better off doing this just when you need it:

```c#
browser.ClickButton("Search", new Options { WaitBeforeClick = TimeSpan.FromMilliseconds(0.2) } )
```

## Additions

### Tables

Define a table record like this:
```c#
public class FundRecord : TableRecord
{
	public TableAttribute
		fundName,
		fundManager = Find("Fund/Manager");
}
```
Each table attribute is a column name. If it is not initialized with Find(), Coypu will try to match the attribute name itself (by removing spaces between words in column names and converting everything to caps). Find() allows to specify column name explicitly (no manipulation will be done on provided string).

In the example above, we expect a column "Fund Name" (but "FUNDNAME", "f und n AME" and other will be found too), and "Fund/Manager".

Define a table itself like this:
```c#
public Table<FundRecord> FundData =
	new Table<FundRecord>(driverScope, "//div[@class='gtLeadWrapper']//table");
```
Coypu will treat element found by provided XPath locator as a table (and will look for td/tr inside of it). You can merge several tables by providing several XPath locators into the constructor. First row (whether it's thead or not) will be treated as the header with column names.

To access data:
```c#
foreach (var x in FundData.Data)
	Console.WriteLine(x.fundName.Text);
```

### Containers

Define a container type like this:
```c#
public class ArticlePreview : ContainerScope
{
	public ElementScope
		Title = Css("h2"),
		Extract = XPath("//div[@id='extract']"),
		Open = Link("Open");
}
```

To find a container by XPath:
```c#
FindContainer<ArticlePreview>("//div[@class='article']");
```

If you reuse the same container in a lot of places, it makes sense to only define it's locator once; then you can omit it:
```c#
public class PageHeader : ContainerScope
{
	public PageHeader()
	{
		defaultLocator = "//div[@id='header']";
	}
}
...

PageHeader p = FindContainer();
```

To convert an already found SnapshotElementScope to a container:
```c#
foreach (var e in FindAllXPath("//div[@class='article']"))
	yield return e.AsContainer<ArticlePreview>();
```

### Page Object

Inherit from Page, define elements and set Url in constructor:
```с#
class SignIn : Page
{
	ElementScope
		Email = Field("Email Address"),
		Password = Field("Password"),
		SignInBtn = Button("Sign In");
	PageHeader
		header = Container<HeaderContainer>();
	PageFooter
		footer = Container<FooterContainer>("//span[@id='footer']");
		
	public SignIn()
	{
		Url = Url + "/sign-in";
	}
}
```

Then use it like this:
```c#
using (var p = new Coypu.Pages.SignIn().Visit(session))
{
	p.DoSomething();
}
```

Every field that implements IHaveScope (that includes ElementScope and ContainerScope) will be using the BrowserSession that you provided in Visit().

### FieldAutocomplete

TODO

### Other

ElementScope.ClickWait() - click on an element and wait until next page is loaded (i.e. until old <html> is stale). Selenium only.

## License

(The MIT License)

Copyright &copy; Adrian Longley, ITV plc & Contributors 2012

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the 'Software'), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
