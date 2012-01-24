using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Coypu
{
    public interface Scope
    {
        /// <summary>
        /// Click a button, input of type button|submit|image or div with the css class "button"
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickButton(string locator);

        /// <summary>
        /// Click the first matching link
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickLink(string locator);

        /// <summary>
        /// Click a previously found element
        /// </summary>
        /// <param name="element">The element to click</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Click(Element element);

        /// <summary>
        /// Find and click an element robustly
        /// </summary>
        /// <param name="findElement">How to find the element</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Click(Func<Element> findElement);

        /// <summary>
        /// <para>Click a button, input of type button|submit|image or div with the css class "button".</para>
        /// <para>Wait for a condition to be satisfied for a specified time otherwise click and wait again.</para>
        /// <para>Continues until the expected condition is satisfied or the <see cref="Configuration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBetweenRetries">How long to wait for the condition to be satisfied before clicking again</param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries);

        /// <summary>
        /// <para>Click a link and wait for a condition to be satisfied for a specified time otherwise click and wait again.</para> 
        /// <para>Continues until the expected condition is satisfied or the <see cref="Configuration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBetweenRetries">How long to wait for the condition to be satisfied before clicking again</param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries);

        /// <summary>
        /// Visit a url in the browser
        /// </summary>
        /// <param name="virtualPath">Virtual paths will use the Configuration.AppHost,Port,SSL settings. Otherwise supply a fully qualified URL.</param>
        void Visit(string virtualPath);

        /// <summary>
        /// Find the first input of type button|submit|image or div with the css class "button" to appear within the <see cref="Configuration.Timeout"/> .
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <returns>A button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Element FindButton(string locator);

        /// <summary>
        /// Find the first matching link to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <returns>A link</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        ElementScope FindLink(string locator);

        /// <summary>
        /// Find the first form field of any type to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the value of a radio button, the last part of the id (for asp.net forms testing)</param>
        /// <returns>A form field</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Element FindField(string locator);

        /// <summary>
        /// Find the first matching text field to appear within the <see cref="Configuration.Timeout"/> to fill in.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the last part of the id (for asp.net forms testing)</param>
        /// <returns>With</returns>
        FillInWith FillIn(string locator);

        /// <summary>
        /// Fill in a previously found text field
        /// </summary>
        /// <param name="element">The text field</param>
        /// <returns>With</returns>
        FillInWith FillIn(Element element);

        /// <summary>
        /// Select an option from a select element
        /// </summary>
        /// <param name="option">The text or value of the option to select</param>
        /// <returns>From</returns>
        SelectFrom Select(string option);

        /// <summary>
        /// CQuery whether text appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="text">The exact text to find</param>
        /// <returns>Whether the text appears</returns>
        bool HasContent(string text);

        /// <summary>
        /// Query whether text appears on the page using a regular expression within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="pattern">The regular expression to match</param>
        /// <returns>Whether the page text matches</returns>
        bool HasContentMatch(Regex pattern);

        /// <summary>
        /// Query whether text does not appear on the page. Returns as soon as the text does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="text">The exact text expected not to be found</param>
        /// <returns>Whether the text does not appear</returns>
        bool HasNoContent(string text);

        /// <summary>
        /// Query whether text does not appear on the page using a regular expression. Returns as soon as the text does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="pattern">The regular expression expected not to match</param>
        /// <returns>Whether the text does not appear</returns>
        bool HasNoContentMatch(Regex pattern);

        /// <summary>
        /// Query whether an element matching a CSS selector appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element appears</returns>
        bool HasCss(string cssSelector);

        /// <summary>
        /// Query whether an element matching a CSS selector does not appear on the page. Returns as soon as the element does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element does not appear</returns>
        bool HasNoCss(string cssSelector);

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        bool HasXPath(string xpath);

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page. Returns as soon as the element appears, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        bool HasNoXPath(string xpath);

        /// <summary>
        /// Find an element matching a CSS selector
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>The first matchin element</returns>
        Element FindCss(string cssSelector);

        /// <summary>
        /// Find an element matching an XPath query
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>The first matchin element</returns>
        Element FindXPath(string xpath);

        /// <summary>
        /// Find all elements matching a CSS selector at the current moment. Does not wait until the <see cref="Configuration.Timeout"/> but returns as soon as the driver does.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>All matching elements</returns>
        IEnumerable<Element> FindAllCss(string cssSelector);

        /// <summary>
        /// Find all elements matching an XPath query at the current moment. Does not wait until the <see cref="Configuration.Timeout"/> but returns as soon as the driver does.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>All matching elements</returns>
        IEnumerable<Element> FindAllXPath(string xpath);

        /// <summary>
        /// <para>Find the first matching section to appear within the <see cref="Configuration.Timeout"/></para>
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
        /// <returns>An element</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Element FindSection(string locator);

        /// <summary>
        /// <para>Find the first matching fieldset to appear within the <see cref="Configuration.Timeout"/></para>
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
        /// <returns>An element</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Element FindFieldset(string locator);

        /// <summary>
        /// Find the first matching element with specified id to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="id">Element id</param>
        /// <returns>An elemenet</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        Element FindId(string id);

        /// <summary>
        /// Check the first checkbox to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, name or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Check(string locator);

        /// <summary>
        /// Uncheck the first checkbox to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, name or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Uncheck(string locator);

        /// <summary>
        /// Choose the first radio button to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, the name, the value or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        void Choose(string locator);

        /// <summary>
        /// Executes custom javascript in the browser
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        /// <returns>Anything returned from the script</returns>
        string ExecuteScript(string javascript);

        /// <summary>
        /// Hover the mouse over an element
        /// </summary>
        /// <param name="findElement">A function to find the element</param>
        void Hover(Func<Element> findElement);

        /// <summary>
        /// Query whether an element appears within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        bool Has(Func<Element> findElement);

        /// <summary>
        /// Query whether an element does not appear. Returns as soon as the element does not appear or after the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        bool HasNo(Func<Element> findElement);

        /// <summary>
        /// <para>Retry an action on any exception until it succeeds. Once the <see cref="Configuration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="action">An action</param>
        void RetryUntilTimeout(Action action);

        /// <summary>
        /// <para>Retry a function on any exception until it succeeds. Once the <see cref="Configuration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="function">A function</param>
        TResult RetryUntilTimeout<TResult>(Func<TResult> function);

        /// <summary>
        /// <para>Execute a query repeatedly until either the expected result is returned or the <see cref="Configuration.Timeout"/> is passed.</para>
        /// <para>Once the <see cref="Configuration.Timeout"/> is passed any result will be returned or any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries.</para>
        /// </summary>
        /// <param name="query">A query</param>
        /// <param name="expecting">Expected result</param>
        T Query<T>(Func<T> query, T expecting);

        /// <summary>
        /// <para>Execute an action repeatedly until a condition is met.</para>
        /// <para>Allows the time specified in <paramref name="waitBeforeRetry"/> for the <paramref name="until"/> condition to be met before each retry.</para>
        /// <para>Once the <see cref="Configuration.Timeout"/> is passed a Coypu.MissingHtmlException will be thrown.</para>
        /// </summary>
        /// <param name="tryThis">The action to try</param>
        /// <param name="until">The condition to be met</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the until condition is never met</exception>
        void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry);

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

        TScope ConsideringInvisibleElements<TScope>() where TScope : Scope;
    }
}