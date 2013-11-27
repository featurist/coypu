using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu
{   

    /// <summary>
    /// The scope for any browser interaction: a browser window, frame or element.
    /// </summary>
    public interface Scope
    {
        /// <summary>
        /// Click a button, input of type button|submit|image or div with the css class "button"
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickButton(string locator, Options options = null);

        /// <summary>
        /// Click the first matching link
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickLink(string locator, Options options = null);

        /// <summary>
        /// Find the first input of type button|submit|image or div with the css class "button" to appear within the configured timeout .
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>A button</returns>
        ElementScope FindButton(string locator, Options options = null);

        /// <summary>
        /// Find the first matching link to appear within the configured timeout
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>A link</returns>
        ElementScope FindLink(string locator, Options options = null);

        /// <summary>
        /// Find the first form field of any type to appear within the configured timeout
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name (except radio buttons), the placeholder text, the value of a radio button or checkbox</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>A form field</returns>
        ElementScope FindField(string locator, Options options = null);

        /// <summary>
        /// Find the first matching text field to appear within the configured timeout to fill in.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>With</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        FillInWith FillIn(string locator, Options options = null);

        /// <summary>
        /// Select an option from a select element
        /// </summary>
        /// <param name="option">The text or value of the option to select</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>From</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        SelectFrom Select(string option, Options options = null);

        /// <summary>
        /// Query whether text appears on the page within the configured timeout
        /// </summary>
        /// <param name="text">The exact text to find</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether the text appears</returns>
        bool HasContent(string text, Options options = null);

        /// <summary>
        /// Query whether text appears on the page using a regular expression within the configured timeout
        /// </summary>
        /// <param name="pattern">The regular expression to match</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether the page text matches</returns>
        bool HasContentMatch(Regex pattern, Options options = null);

        /// <summary>
        /// Query whether text does not appear on the page. Returns as soon as the text does not appear, or when the <see cref="SessionConfiguration.Timeout"/> is reached.
        /// </summary>
        /// <param name="text">The exact text expected not to be found</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether the text does not appear</returns>
        bool HasNoContent(string text, Options options = null);

        /// <summary>
        /// Query whether text does not appear on the page using a regular expression. Returns as soon as the text does not appear, or when the <see cref="SessionConfiguration.Timeout"/> is reached.
        /// </summary>
        /// <param name="pattern">The regular expression expected not to match</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether the text does not appear</returns>
        bool HasNoContentMatch(Regex pattern, Options options = null);

        /// <summary>
        /// Query whether an element matching a CSS selector appears on the page within the configured timeout
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element appears</returns>
        bool HasCss(string cssSelector, Options options = null);

        /// <summary>
        /// Query whether an element matching a CSS selector appears on the page within the configured timeout
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="text">The text of the element must exactly match this text</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element appears</returns>
        bool HasCss(string cssSelector, string text, Options options = null);

        /// <summary>
        /// Query whether an element matching a CSS selector appears on the page within the configured timeout
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="text">The text of the element must match this pattern</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element appears</returns>
        bool HasCss(string cssSelector, Regex text, Options options = null);

        /// <summary>
        /// Query whether an element matching a CSS selector does not appear on the page. Returns as soon as the element does not appear, or when the <see cref="SessionConfiguration.Timeout"/> is reached.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element does not appear</returns>
        bool HasNoCss(string cssSelector, Options options = null);
        bool HasNoCss(string cssSelector, Regex trext, Options options = null);
        bool HasNoCss(string cssSelector, string text, Options options = null);

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page within the configured timeout
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element appears</returns>
        bool HasXPath(string xpath, Options options = null);

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page. Returns as soon as the element appears, or when the <see cref="SessionConfiguration.Timeout"/> is reached.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>Whether an element appears</returns>
        bool HasNoXPath(string xpath, Options options = null);

        /// <summary>
        /// Find an element matching a CSS selector
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching element</returns>
        ElementScope FindCss(string cssSelector, Options options = null);

        /// <summary>
        /// Find an element matching a CSS selector
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="text">The text of the element must exactly match this text</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching element</returns>
        ElementScope FindCss(string cssSelector, string text, Options options = null);

        /// <summary>
        /// Find an element matching a CSS selector
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="text">The text of the element must match this pattern</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching element</returns>
        ElementScope FindCss(string cssSelector, Regex text, Options options = null);

        /// <summary>
        /// Find an element matching an XPath query
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching element</returns>
        ElementScope FindXPath(string xpath, Options options = null);

        /// <summary>
        /// Find all elements matching a CSS selector. If a predicate is supplied this will wait until the predicate matches, otherwise this will return immediately.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <param name="predicate">A predicate to test the entire collection against. It will wait for this predicate before returning a list of matching elements.</param>
        /// <param name="options">
        ///   <para>Override the way Coypu is configured to find elements for this call only.</para>
        ///   <para>E.g. A longer wait:</para>
        /// 
        ///   <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>All matching elements as snapshot scopes which will not respect future changes in the document</returns>
        IEnumerable<SnapshotElementScope> FindAllCss(string cssSelector, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null,
                                             Options options = null);

        /// <summary>
        /// Find all elements matching an XPath query. If a predicate is supplied this will wait until the predicate matches, otherwise this will return immediately.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <param name="options">
        ///   <para>Override the way Coypu is configured to find elements for this call only.</para>
        ///   <para>E.g. A longer wait:</para>
        /// 
        ///   <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>All matching elements</returns>
        IEnumerable<SnapshotElementScope> FindAllXPath(string xpath, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null, Options options = null);

        /// <summary>
        /// <para>Find the first matching section to appear within the configured timeout.</para>
        /// <para>Sections are identified by the text of their child heading element, or by id.</para>
        /// <para>E.g. to find this:
        /// 
        /// <code>    
        ///     &lt;div&gt;	
        ///         &lt;h2&gt;Search results&lt;/h2&gt;
        ///         ...
        ///     &lt;/div&gt;</code>
        ///
        /// or this:
        ///
        /// <code>
        ///     &lt;section&gt;
        ///         &lt;h3&gt;Search results&lt;/h3&gt;
        ///         ...
        ///     &lt;/section&gt;</code>
        /// </para>
        /// <para>use this:</para>
        /// <para>
        /// <code>    FindSection("Search results")</code>
        /// </para>
        /// </summary>
        /// <param name="locator">The text of a child heading element or section id</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>An element</returns>
        ElementScope FindSection(string locator, Options options = null);

        /// <summary>
        /// <para>Find the first matching fieldset to appear within the configured timeout.</para>
        /// <para>Fieldsets are identified by the text of their child legend element, or by id.</para>
        /// <para>E.g. to find this:
        /// 
        /// <code>    
        ///     &lt;fieldset&gt;	
        ///         &lt;legend&gt;Advanced search&lt;/legend&gt;
        ///         ...
        ///     &lt;/fieldset&gt;</code>
        /// </para>
        /// <para>use this:</para>
        /// <para>
        /// <code>    FindFieldset("Advanced search")</code>
        /// </para>
        /// </summary>
        /// <param name="locator">The text of a child legend element or fieldset id</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>An element</returns>
        ElementScope FindFieldset(string locator, Options options = null);

        /// <summary>
        /// <para>Find the first matching frame or iframe to appear within the configured timeout.</para>
        /// <para>Frames are identified by the their id, name or title attributes, by their loaded page title, or by the text of the &lt;h1&gt; element in their content.</para>
        /// <para>E.g. to find this:
        /// 
        /// <code>    
        ///     &lt;iframe id="myFrame" title="My I Frame" src="..."&gt;	
        ///         &lt;h1&gt;My Frame Header&lt;/h1&gt;
        ///         ...
        ///     &lt;/iframe&gt;
        /// </code>
        /// </para>
        /// <para>use one of these:
        /// 
        /// <code>    
        ///         Findframe("myFrame")
        ///         Findframe("My I Frame")
        ///         Findframe("My Frame Header")
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="locator">The text of a child legend element or fieldset id</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>An element</returns>
        ElementScope FindFrame(string locator, Options options = null);

        /// <summary>
        /// Find the first matching element with specified id to appear within the configured timeout
        /// </summary>
        /// <param name="id">Element id</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>An element</returns>
        ElementScope FindId(string id, Options options = null);

        /// <summary>
        /// Check the first checkbox to appear within the configured timeout matching the text of the associated label element, the id, name or value.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Check(string locator, Options options = null);

        /// <summary>
        /// Uncheck the first checkbox to appear within the configured timeout matching the text of the associated label element, the id, name.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Uncheck(string locator, Options options = null);

        /// <summary>
        /// Choose the first radio button to appear within the configured timeout matching the text of the associated label element, the id or the value.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Choose(string locator, Options options = null);

        /// <summary>
        /// Query whether an element appears within the configured timeout
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        bool Has(ElementScope findElement);

        /// <summary>
        /// Query whether an element does not appear. Returns as soon as the element does not appear or after the <see cref="SessionConfiguration.Timeout"/>
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        bool HasNo(ElementScope findElement);

        /// <summary>
        /// <para>Retry an action on any exception until it succeeds. Once the <see cref="SessionConfiguration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="SessionConfiguration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="action">An action</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        void RetryUntilTimeout(Action action, Options options = null);

        /// <summary>
        /// <para>Retry an action on any exception until it succeeds. Once the <see cref="SessionConfiguration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="SessionConfiguration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="action">An action</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        void RetryUntilTimeout(BrowserAction action);


        /// <summary>
        /// <para>Retry a function on any exception until it succeeds. Once the <see cref="SessionConfiguration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="SessionConfiguration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="function">A function</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null);

        /// <summary>
        /// <para>Execute a query repeatedly until either the expected result is returned or the <see cref="SessionConfiguration.Timeout"/> is passed.</para>
        /// <para>Once the <see cref="SessionConfiguration.Timeout"/> is passed any result will be returned or any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="SessionConfiguration.RetryInterval"/> between retries.</para>
        /// </summary>
        /// <param name="query">A query</param>
        T Query<T>(Query<T> query);

        /// <summary>
        /// <para>Execute a query repeatedly until either the expected result is returned or the <see cref="SessionConfiguration.Timeout"/> is passed.</para>
        /// <para>Once the <see cref="SessionConfiguration.Timeout"/> is passed any result will be returned or any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="SessionConfiguration.RetryInterval"/> between retries.</para>
        /// </summary>
        /// <param name="query">A query</param>
        /// <param name="expecting">Expected result</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        T Query<T>(Func<T> query, T expecting, Options options = null);

        /// <summary>
        /// <para>Execute an action repeatedly until a condition is met.</para>
        /// <para>Allows the time specified in <paramref name="waitBeforeRetry"/> for the <paramref name="until"/> condition to be met before each retry.</para>
        /// <para>Once the <see cref="SessionConfiguration.Timeout"/> is passed a Coypu.MissingHtmlException will be thrown.</para>
        /// </summary>
        /// <param name="tryThis">The action to try</param>
        /// <param name="until">The condition to be met</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition to be met before retrying</param>        
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the until condition is never met</exception>
        void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null);

        /// <summary>
        /// <para>Execute an action repeatedly until a condition is met.</para>
        /// <para>Allows the time specified in <paramref name="waitBeforeRetry"/> for the <paramref name="until"/> query to return the expected value before each retry.</para>
        /// <para>Once the <see cref="SessionConfiguration.Timeout"/> is passed a Coypu.MissingHtmlException will be thrown.</para>
        /// </summary>
        /// <param name="tryThis">The action to try</param>
        /// <param name="until">The condition to be met</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition to be met before retrying</param>        
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the until condition is never met</exception>
        void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null);

        /// <summary>
        /// <para>Find the first from a list of possible states that your page may arrive at.</para>
        /// <para>Returns as soon as any of the possible states is found.</para>
        /// <para>E.g.:</para>
        /// 
        /// <code>  
        ///  var signedIn = new State(() => browser.HasContent("Signed in as:"));
        ///  var signedOut = new State(() => browser.HasContent("Please sign in"));
        ///  
        ///  if (browser.FindState(signedIn,signedOut) == signedIn) 
        ///  {
        ///    browser.ClickLink("Sign out");
        ///  }
        ///  </code>
        ///  </summary>
        /// <param name="states">The possible states you are expecting</param>
        /// <returns></returns>
        State FindState(params State[] states);

        /// <summary>
        /// <para>Find the first from a list of possible states that your page may arrive at.</para>
        /// <para>Returns as soon as any of the possible states is found.</para>
        /// <para>E.g.:</para>
        /// 
        /// <code>  
        ///  var signedIn = new State(browser.HasContent("Signed in as:"));
        ///  var signedOut = new State(browser.HasContent("Please sign in"));
        ///  
        ///  if (browser.FindState(signedIn,signedOut) == signedIn) 
        ///  {
        ///    browser.ClickLink("Sign out");
        ///  }
        ///  </code>
        ///  </summary>
        /// <param name="states">The possible states you are expecting</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns></returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the none of the states are reached within the timeout</exception>
        State FindState(State[] states, Options options = null);

        /// <summary>
        /// <para>Click a button, input of type button|submit|image or div with the css class "button".</para>
        /// <para>Wait for a condition to be satisfied for a specified time otherwise click and wait again.</para>
        /// <para>Continues until the expected condition is satisfied or the <see cref="SessionConfiguration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition to be met before retrying</param>   
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Scope ClickButton(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null);

        /// <summary>
        /// <para>Click a link and wait for a condition to be satisfied for a specified time otherwise click and wait again.</para> 
        /// <para>Continues until the expected condition is satisfied or the <see cref="SessionConfiguration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition to be met before retrying</param>   
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Scope ClickLink(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null);

        /// <summary>
        /// Try and find this scope now
        /// </summary>
        /// <returns></returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        ElementFound Now();

        /// <summary>
        /// The location of the current browser window
        /// </summary>
        Uri Location { get; }

        /// <summary>
        /// <para>By default Coypu will exclude any invisible elements, this allows you to override that behaviour</para>
        /// <para>Default: true</para>
        /// </summary>
        bool ConsiderInvisibleElements { get; }

        /// <summary>
        /// <para>Whether to consider a partial match when finding elements by text, or just an exact match.</para>
        /// <para>The following elements currently support partial matching:</para>
        /// <para>FillIn (label text)</para>
        /// <para>FindField (label text)</para>
        /// <para></para>
        /// <para>ClickLink (link text)</para>
        /// <para>FindLink (link text)</para>
        /// <para></para>
        /// <para>ClickButton (link text)</para>
        /// <para>FindButton (link text)</para>
        /// </summary>
        bool Exact { get; }

        /// <summary>
        /// <para>With Match you can control how Coypu behaves when multiple elements all match a query. There are currently two different strategies:</para>
        /// <para>Match.First: The default strategy. If multiple matches are found, some of which are exact, and some of which are not, then the first exactly matching element is returned.</para>
        /// <para>Match.Single: If the Exact option is true, raises an error if more than one element matches, just like one. If Exact is false, it will first try to find an exact match. An error is raised if more than one element is found. If no element is found, a new search is performed which allows partial matches. If that search returns multiple matches, an error is raised.</para>
        /// </summary>
        Match Match { get; }

        /// <summary>
        /// Options for how Coypu interacts with the browser.
        /// </summary>
        Options Options { get; }
    }
}