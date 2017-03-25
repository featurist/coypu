# Introducing Coypu 2.0
## 1.0 was overdue 

Coypu has been pretty stable for a while now. It should have bumped to 1.0 as soon as the project was made public but this never happened, for no good reason. Despite this mistake, the API is now considered as stable by the community and recent updates have respected semver for Minor and Patch releases. Now that there is a Major release which breaks backward compatibility it seemed important I first make version 1.0 of the API that which has been around a while.

So first of all Coypu 1.0 was released recently. It has no changes from 0.23.1 except for a Selenium.WebDriver update.

Apologies for any confusion around this.

## On to 2.0

Since Capybara 2.1 was released it's been clear that Coypu should to provide the same consistency, clarity and configurability around two key areas:

* Matching exactly or allowing substrings
* Behaviour when multiple elements match a query

In addressing these I have tried to enable as smooth an upgrade path as possible. There are still changes which break compatibility between 1.x and 2.x, but most of them should be fairly easy to deal with and I have added some directions in new exception messages you might come across.

The two new configuration options are:

* **`TextPrecision (TextPrecision.Exact | TextPrecision.Substring | TextPrecision.PreferExact)`**
* **`Match (Match.Single, Match.First)`**

If you are familiar with Capybara 2.1 you might notice these options are different, but the same behaviour can be achieved with these options as in Capybara.

### Matching exactly or allowing substrings

Coypu has previously been rather inconsistent about matching text, sometimes allowing substrings sometimes not. To resolve this a new option called `TextPrecision` has been added which allows you to specify this globally and to override on each and every call. `TextPrecision` has three options: `Exact`, `Substring` and `PreferExact`. The default is `PreferExact`.

`TextPrecision.Exact` will only match the entire text of an element exactly.

`TextPrecision.Substring` will allow you to specify a substring to find an element.

`TextPrecision.PreferExact` which will prefer an exact text match to a substring match. **This is the default for TextPrecision**

#### Usage

```c#
browserSession.FillIn("Password", new Options{TextPrecision = TextPrecision.Exact}).With("123456");
// or
browserSession.FillIn("Password", Options.Exact).With("123456");
```

This will be respected everywhere that Coypu matches visible text, including buttons, select options, placeholder text and so on, but not anywhere else, for example when considering ids or names of fields.

#### FindCss

`BrowserSession.FindCss()` has had overloads to specify text of the element for a while. In Coypu 1.x the `FindCss(string cssSelector, string text)` overload matched exact text and the `FindCss(String cssSelector, Regex text)` overload was the only way to match substrings of the text of an element found with a CSS selector.

In 2.x the string overload respects the value of the `TextPrecision` option (`PreferExact` by default) so this is one particular area you will see looser matching by default than Coypu 1.x.


### Behaviour when multiple elements match a query

When using methods such as `ClickLink()`, and `FillIn()`, what happens when more than one element matches? Under Coypu 1.0, we simply returned the first one and moved on. This has the obvious problem that sometimes, the element you end up interacting with isn’t the one you expected at all.

In some cases we tried to find an element that matched exactly first. Now that in Coypu 2.0 you have control of how text is matched with the `TextPrecision` option, deciding what to do when there are multiple matches using that `TextPrecision` is as simple as choosing one of these two `Match` strategies:

`Match.Single` if there is more than one matching element a `Coypu.AmbiguousException` is thrown.

`Match.First` just returns the first matching element. **This is the default for Match**


#### Usage

```c#
browserSession.ClickButton("Close", new Options{Match = Match.First});
// or
browserSession.ClickButton("Close", Options.First);
```

### Some more examples of using TextPrecision and Match

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
