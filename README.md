# Coypu [![Nuget](https://img.shields.io/nuget/v/Coypu.svg)](https://www.nuget.org/packages/Coypu/) [![Nuget](https://img.shields.io/nuget/dt/Coypu.svg)](https://www.nuget.org/packages/Coypu/) ![](https://img.shields.io/badge/compatibility-.NET%20Framework%204.5%2B%20%7C%20.NET%20Standard%202.0-blue.svg)

## "Theirs not to reason why, Theirs but to do and retry"
&mdash; <cite>Alfred, Lord Selenium</cite>
## "haha, coypu must have reduced teh amount of shit code by 90%"
&mdash; <cite>Anonymous</cite>

Coypu supports browser automation in .Net to help make tests readable, robust, fast to write and less tightly coupled to the UI. If your tests are littered with sleeps, retries, complex XPath expressions and IDs dug out of the source with browser developer tools then Coypu might help.

Coypu is on Nuget:

    PM> Install-Package Coypu

NUnit matchers (e.g. `Assert.That(browserSession, Shows.Content("Hello world"));`) are in a separate package:

    PM> Install-Package Coypu.NUnit

Discuss Coypu and get help on the [Google Group](http://groups.google.com/group/coypu)

## Coypu is
* A robust wrapper for browser automation on .net platform for [Selenium WebDriver](https://www.selenium.dev/documentation/webdriver/) or [Microsoft Playwright](https://playwright.dev) that eases automating ajax-heavy websites and reduces coupling to the HTML, CSS & JS
* A more intuitive API for interacting with the browser in the way a human being would, inspired by the ruby framework Capybara - http://github.com/jnicklas/capybara

## Demo

Check out a [demo of Coypu](http://skillsmatter.com/podcast/open-source-dot-net/london-dot-net-user-group-may) from a talk given at Skills Matter way back in May 2011.

## Browser session

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

## Configuration

To configure Coypu pass an instance of `Coypu.SessionConfiguration` to the constructor of BrowserSession:

```c#
var browserSession = new BrowserSession(new SessionConfiguration{...});
```

## Website under test

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

Basic authentication can be configured with the AppHost like so:
```c#
var sessionConfiguration = new SessionConfiguration
{
  AppHost = "username@password:autotrader.co.uk"
};
```

## Driver

Coypu drivers implement the `Coypu.Driver` interface and read the `SessionConfiguration.Browser` setting to pick the correct browser.

Choose your driver/browser combination like so:

```c#
sessionConfiguration.Driver = typeof (Coypu.Drivers.Selenium.SeleniumWebDriver);
sessionConfiguration.Browser = Drivers.Browser.Firefox;
```

These settings are the default configuration.

If you want to configure these at runtime you could replace the following strings with strings read from your environment / configuration:

```c#
sessionConfiguration.Driver = Type.GetType("Coypu.Drivers.Selenium.SeleniumWebDriver, Coypu");
sessionConfiguration.Browser = Drivers.Browser.Parse("firefox");
```

## Headless mode

```c#
sessionConfiguration.Headless=true
```

**The Playwright driver in headless mode passes all the Coypu specs. Headless Playwright runs up to 5 times faster than headed Selenium.**

Headless was never enabled on the Selenium driver as it had various shortcomings in the past, from Coypu v4.1.0 it is can be enabled using the `sessionConfiguration.Headless` setting. Selenium does have issues accessing cookies and IFrames in headless mode so you may need to run tests that rely on these features in headed mode when using the Selenium driver.

Selenium only supports headless for Chrome, Edge and Firefox. IE, Safari and Opera are not supported.

## Playwright

Coypu v4.1.0 adds a driver for the Playwright framework. Playwright is a modern automation framework from Microsoft that is faster and more reliable than Selenium. Playwright supports Chrome, Edge, Firefox and Webkit (Safari).

The playwright driver will become the default driver for Coypu in the next major release.

[A note on what Playwright means for Coypu](#a-note-on-playwright)

```c#
sessionConfiguration.Driver = typeof (Coypu.Drivers.Playwright.PlaywrightDriver);
```

### Install browser binaries

You may need to install the latest playwright browser binaries using the Playwright CLI:

https://playwright.dev/dotnet/docs/browsers#install-browsers


### Playwright supports:

```c#
Copyu.Browser.Chromium
Copyu.Browser.Chrome
Copyu.Browser.Edge
Copyu.Browser.Firefox
Copyu.Browser.Webkit
```

More on these options: https://playwright.dev/dotnet/docs/browsers

## Selenium WebDriver

`Coypu.Drivers.Selenium.SeleniumWebDriver` tracks the latest version of WebDriver and supports Chrome (fastest), Firefox, Edge and IE (slowest) as the browser. Any other Selenium implementation of RemoteWebDriver can be configured by subclassing `SeleniumWebDriver` and passing an instance of RemoteWebDriver to the base constructor.

The Selenium Driver is included in the Coypu package.

### Chrome
You will need the chromedriver.exe on your PATH or in the bin of your test project. We recommend adding the nuget package `Selenium.WebDriver.ChromeDriver` to your project.

### Firefox
You will need GeckoDriver. We recommend adding the nuget package `Selenium.WebDriver.GeckoDriver.Win64` to your project.

### Edge
You will need Microsoft's WebDriver. How you install this depends on your version of Windows 10. Please see [here](https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/) for more information.

### Internet Explorer

You will need the new standalone InternetExplorerDriver.exe in your PATH or in the bin of your test project. We recommend adding the nuget package `Selenium.WebDriver.IEDriver` package to your project. Please see [Configuration Requirements](https://github.com/SeleniumHQ/selenium/wiki/InternetExplorerDriver#required-configuration) for information on how to use it.

## SpecFlow scenarios

If you are using SpecFlow for your acceptance tests then you will probably want to configure it to provide a single Browser Session scoped to each scenario. SpecFlow supports some basic dependency injection which you can use to achieve this as shown in [this gist](https://gist.github.com/2301407).

## Waits, retries and timeout

Most of the methods in the Coypu API are automatically retried on any driver error until a configurable timeout is reached. It just catches exceptions and retries -- mainly the `Coypu.Drivers.MissingHtmlException` that a driver should throw when it cannot find something, but also any internal driver errors that the driver might throw up.

This is a rather blunt approach that goes well beyond WebDriver's ImplicitWait, for example, but the only truly robust strategy for heavily asynchronous websites, where elements are flying in and out of the DOM constantly, that I have found.

All methods use this wait and retry strategy *except*: `Visit()`, `FindAllCss()` and `FindAllXPath()` which call the driver once immediately unless you supply a predicate to describe the expected state.

Configure timeout/retry like so:

```c#
sessionConfiguration.Timeout = TimeSpan.FromSeconds(1);
sessionConfiguration.RetryInterval = TimeSpan.FromSeconds(0.1);
```

These settings are the default configuration.

All methods in the API take an optional final parameter of a `Coypu.Options`. By passing this in you can override any of these timing settings for just that call:

So when you need an unusually long (or short) timeout for a particular interaction you can override the timeout just for this call by passing in a `Coypu.Options` like this:

```c#
browser.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
browser.ClickButton("Upload");
Assert.That(browser, Shows.Content("File bigfile.mp4 (10.5mb) uploaded successfully", new Options { Timeout = TimeSpan.FromSeconds(60) } ));
```

The options you specify are merged with your SessionConfiguration, so you only need specify those options you wish to override.

## Visible elements

Coypu drivers filter out any elements that are not visible on the page -- this includes hidden inputs.

Non-visible elements can get in the way of finding the elements that we are really looking for and cause often errors when trying to interact with them.

What we are really trying to do here is interact with the browser in the way that a human would. It's probably best to avoid hacking around with elements not accessible to the user where possible to avoid invalidating our tests in any case.

### However...

If you really need this for some intractable problem where you cannot control the browser without cheating like this, then there is `sessionConfiguration/options.ConsideringInvisibleElements = true` which overrides this restriction.

## Can't find what you need?

If there's something you need that's not part of the API then you may need to dive into the native driver which you can always do by casting the native driver to whatever underlying driver you know you are using:

```c#
var selenium = ((OpenQA.Selenium.Remote.RemoteWebDriver) browserSession.Native);
```

But if you need to do this, please consider forking Coypu, adding what you need and sending a pull request. Thanks!

# API

Here are some examples to get you started using Coypu

## Navigating

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

## Getting the page title
```c#
browser.Title
```

## Completing forms

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

## Clicking

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

## Finding single elements

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

## Finding multiple elements

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

## Matching exactly or allowing substrings

When finding elements by their text, the `TextPrecision` option allows you to specify whether to match exact text or allow a substring match. This can be set globally and also overridden on each and every call. `TextPrecision` has three options: `Exact`, `Substring` and `PreferExact`. The default is `PreferExact`.

`TextPrecision.Exact` will only match the entire text of an element exactly.

`TextPrecision.Substring` will allow you to specify a substring to find an element.

`TextPrecision.PreferExact` which will prefer an exact text match to a substring match. **This is the default for TextPrecision**

## Usage

```c#
browserSession.FillIn("Password", new Options{TextPrecision = TextPrecision.Exact}).With("123456");
// or
browserSession.FillIn("Password", Options.Exact).With("123456");
```

This will be respected everywhere that Coypu matches visible text, including buttons, select options, placeholder text and so on, but not anywhere else, for example when considering ids or names of fields.

## Behaviour when multiple elements match a query

When using methods such as `ClickLink()`, and `FillIn()`, what happens when more than one element matches? With the `Match` option you have control over what happens by choosing one of the two `Match` strategies:

`Match.Single` if there is more than one matching element a `Coypu.AmbiguousException` is thrown. **This is the default for Match**

`Match.First` just returns the first matching element.


## Usage

```c#
browserSession.ClickButton("Close", new Options{Match = Match.First});
// or
browserSession.ClickButton("Close", Options.First);
```

## Some more examples of using TextPrecision and Match

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

## Hover

Hover over an element

	browser.FindCss("span#hoverOnMe").Hover();

## Fieldsets / Sections

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

## Scope

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

## Beware the XPath // trap

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


## Scoping within frames / iframes

To restrict the scope to a frame or iframe, locate the frame by its name,id, title or the text of an h1 element within the frame:

```c#
var twitterFrame = browser.FindFrame("@coypu_news on Twitter");

Assert.That(twitterFrame, Shows.Content("Coypu 0.8.0 released"));
```

## Scoping within windows

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

## Window size

Sometimes you need to maximise the window, or to set a particular width, perhaps for testing your responsive layout:

```c#
browser.MaximiseWindow();
browser.ResizeTo(768,1000);
```

If you are dealing with multiple windows, just call these on the correct scope:

```c#
browser.FindWindow("Pop Up Window").MaximiseWindow();
```

## Executing javascript in the browser

You can execute javascript like so:

```c#
browser.ExecuteScript("document.getElementById('SomeContainer').innerHTML = '<h2>Hello</h2>';");
```

Anything returned from the javascript will be returned from `browser.ExecuteScript`

```c#
var innerHtml = browser.ExecuteScript("return document.getElementById('SomeContainer').innerHTML;");

```Arguments can be passed and referenced via the arguments array:

```c#
browser.ExecuteScript("document.getElementById(arguments[0]).innerHTML = '<h2>Hello</h2>'");

```Arguments can be `Coypu.Element` objects:

```c#
browser.ExecuteScript("arguments[0].innerHTML = '<h2>Hello</h2>'");

## Querying

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

## Matchers

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

## Inner/OuterHTML

If you just want to grab the inner or outer HTML of an element to do your own queries and assertions you can use:

```c#
var outerHTML = browser.FindCss("table#myData").OuterHTML;
var innerHTML = browser.FindCss("table#myData").InnerHTML; // Will exclude the surrounding <table> ... </table>
```

## Dialogs

To interact with dialogs like alerts, confirms and prompts, pass an Action that will trigger the dialog to appear to `AcceptAlert`, `Accept/CancelConfirm` or `Accept/CancelPrompt`. Optionally supply the text that must match the dialog message or an exception will be raised.

```c#
browser.AcceptAlert(() => {
  browser.ClickButton("Save");
});

browser.AcceptConfirm("Are you sure you want to cancel your account?", () => {
  browser.ClickButton("Cancel my account");
});

browser.CancelConfirm("Are you sure you want to cancel your account?", () => {
  browser.ClickButton("Cancel my account");
});

browser.AcceptPrompt("Please enter your age", "21", () => {
  browser.ClickLink("Enter site");
});
```

## Finding states (nondeterministic testing)

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

## Screenshots

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

## Tell Coypu to keep clicking at regular intervals until you see the result you expect:

```c#
var until = () => browser.FindCss("#SearchResults").Exists();
var waitBetweenRetries = TimeSpan.Seconds(2);

browser.ClickButton("Search", until, waitBetweenRetries);
```

This is far from ideal as you are coupling the click to the expected result rather than verifying what you expect in a separate step, but as a last resort we have found this useful.

## Tell Coypu to wait a short time between first finding links/buttons and clicking them:

```c#
sessionConfiguration.WaitBeforeClick = TimeSpan.FromMilliseconds(0.2);
```

WARNING: Setting this in your session configuration means adding time to *every* click in that session. You might be better off doing this just when you need it:

```c#
browser.ClickButton("Search", new Options { WaitBeforeClick = TimeSpan.FromMilliseconds(0.2) } )
```
## A note on Playwright

The main reason for adding a Playwright driver to this library thats been making Selenium tests reliable since 2010 is to support those projects with existing Coypu test suites who want to move to Playwright without having to rewrite their tests. A good percentage of these are likely legacy apps but some may have a lot of life left in them if the tests can be kept alive!

**If you're starting a new project then you should definitely consider using Playwright directly**, it implements a lot of the most useful stuff from Capybara/Coypu which is fantastic. However, if you like the Coypu API, here's a few reasons you might consider using Coypu over Playwrights own API even for a new project:

### The Capybara/Copyu API is well established and has been pretty stable in the Rails world for a long time.

The finders in Coypu remain more semantic and direct than Playwrights Locator pattern, and are designed to read like the way a human interacts with a web page without specifying `ByLabel`/`ByPlaceholder` etc e.g.:

####] Playwright
```
await page.GetByLabel("Terms & Conditions").CheckAsync();
await page.GetByLabel("User Name").FillAsync("John");
```

#### Coypu
```
window.Check("Terms & Conditions");
window.FillIn("User Name").With("John");
```

You may prefer your tests to read like this, or you might prefer Playwrights more explicit API.

### Waits and retries
Playwright will auto wait for elements to become actionable in the page for you, but where your page elements change dynamically without a full page reload, like any Single Page App, then you will still need to write an `Expect` assertion for something that will indicate the UI has updated or your test may flake out.

e.g. To Click through multiple pages loaded with async

#### Playwright
```
await Page.GetByRole(AriaRole.Link, new() { Name = "Page 2" }).ClickAsync();
// AJAX...
await Expect(Page
            .GetByRole(AriaRole.Heading, new() { Name = "Page 3" }))
            .ToBeVisibleAsync();
// Ready
await Page.GetByRole(AriaRole.Link, new() { Name = "Page 3" }).ClickAsync();
```

#### Coypu
```
window.Click("Page 2");
window.Click("Page 3");
```

### Keeping your options open
As a wrapper that decouples finding elements, interacting with the browser and waits and retries from the actual browser automation you may feel prefer to write against the Coypu API to decouple your tests from your choice of automation framework. Coypu is a small library which can be easily extended to support new drivers, whereas Playwright is a much larger library that you would be more tightly coupled to.
