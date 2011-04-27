using System;
using System.Collections.Generic;

namespace Coypu.Tests.TestDoubles
{
	public class FakeDriver : Driver
	{
		private readonly IList<Node> clickedNodes = new List<Node>();
	    private readonly IList<Node> checkedNodes = new List<Node>();
	    private readonly IList<Node> uncheckedNodes = new List<Node>();
	    private readonly IList<Node> chosenNodes = new List<Node>();
	    private readonly IList<string> hasContentQueries = new List<string>();
	    private readonly IList<string> hasCssQueries = new List<string>();
	    private readonly IList<string> hasXPathQueries = new List<string>();
	    private readonly IList<string> visits = new List<string>();
	    private readonly IDictionary<Node, string> setFields = new Dictionary<Node, string>();
	    private readonly IDictionary<Node, string> selectedOptions = new Dictionary<Node, string>();
		private readonly Dictionary<string, Node> stubbedButtons = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> stubbedLinks = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> stubbedTextFields = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> stubbedCssResults = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> stubbedXPathResults = new Dictionary<string, Node>();
		private readonly IDictionary<string, IEnumerable<Node>> stubbedAllCssResults = new Dictionary<string, IEnumerable<Node>>();
		private readonly IDictionary<string, IEnumerable<Node>> stubbedAllXPathResults = new Dictionary<string, IEnumerable<Node>>();
		private readonly IDictionary<string, bool> stubbedHasContentResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasCssResults = new Dictionary<string, bool>();
	    private readonly IDictionary<string, bool> stubbedHasXPathResults = new Dictionary<string, bool>();

	    private bool disposed;

	    public IEnumerable<Node> ClickedNodes
		{
			get { return clickedNodes; }
		}

	    public IDictionary<Node, string> SetFields
		{
			get { return setFields; }
		}

	    public IDictionary<Node, string> SelectedOptions
		{
			get { return selectedOptions; }
		}

        public IEnumerable<Node> CheckedNodes
        {
            get { return checkedNodes; }
        }

        public IEnumerable<Node> ChosenNodes
        {
            get { return chosenNodes; }
        }

        public IEnumerable<Node> UncheckedNodes
        {
            get { return uncheckedNodes; }
        }

	    public IEnumerable<string> Visits
		{
			get { return visits; }
		}

	    public IEnumerable<string> HasContentQueries
		{
			get { return hasContentQueries; }
		}

	    public IEnumerable<string> HasCssQueries
		{
			get { return hasCssQueries; }
		}

	    public IEnumerable<string> HasXPathQueries
		{
			get { return hasXPathQueries; }
		}

	    public Node FindButton(string locator)
		{
			return stubbedButtons[locator];
		}

	    public Node FindLink(string locator)
		{
			return stubbedLinks[locator];
		}

	    public Node FindField(string locator)
		{
			return stubbedTextFields[locator];
		}

	    public void Click(Node node)
		{
			clickedNodes.Add(node);
		}

	    public void Visit(string url)
		{
			visits.Add(url);
		}

	    public void StubButton(string locator, Node node)
		{
			stubbedButtons[locator] = node;
		}

	    public void StubLink(string locator, Node node)
		{
			stubbedLinks[locator] = node;
		}

	    public void StubField(string locator, Node node)
		{
			stubbedTextFields[locator] = node;
		}

	    public void StubHasContent(string text, bool result)
		{
			stubbedHasContentResults.Add(text, result);
		}


	    public void StubHasCss(string cssSelector, bool result)
		{
			stubbedHasCssResults.Add(cssSelector, result);
		}


	    public void StubHasXPath(string xpath, bool result)
		{
			stubbedHasXPathResults.Add(xpath, result);
		}

	    public void StubCss(string cssSelector, Node result)
		{
			stubbedCssResults.Add(cssSelector, result);
		}

	    public void StubXPath(string cssSelector, Node result)
		{
			stubbedXPathResults.Add(cssSelector, result);
		}

	    public void StubAllCss(string cssSelector, IEnumerable<Node> result)
		{
			stubbedAllCssResults.Add(cssSelector, result);
		}

	    public void StubAllXPath(string xpath, IEnumerable<Node> result)
		{
			stubbedAllXPathResults.Add(xpath, result);
		}

	    public void Dispose()
		{
			disposed = true;
		}

	    public bool Disposed()
		{
			return disposed;
		}

	    public void Set(Node node, string value)
		{
			setFields.Add(node, value);
		}

	    public void Select(Node node, string option)
		{
			selectedOptions.Add(node, option);
		}

	    public object Native
		{
			get { return "Native driver on fake driver"; }
		}

	    public bool HasContent(string text)
		{
			hasContentQueries.Add(text);
			return stubbedHasContentResults[text];
		}


	    public bool HasCss(string cssSelector)
		{
			//hasCssQueries.Add(cssSelector);
			return stubbedHasCssResults[cssSelector];
		}

	    public bool HasXPath(string xpath)
		{
			//hasXPathQueries.Add(xpath);
			return stubbedHasXPathResults[xpath];
		}

	    public Node FindCss(string cssSelector)
		{
			return stubbedCssResults[cssSelector];
		}

	    public Node FindXPath(string xpath)
		{
			return stubbedXPathResults[xpath];
		}

	    public IEnumerable<Node> FindAllCss(string cssSelector)
		{
			return stubbedAllCssResults[cssSelector];
		}

	    public IEnumerable<Node> FindAllXPath(string xpath)
		{
			return stubbedAllXPathResults[xpath];
		}

	    public void Check(Node field)
	    {
	        checkedNodes.Add(field);
	    }

	    public void Uncheck(Node field)
	    {
            uncheckedNodes.Add(field);
	    }

	    public void Choose(Node field)
	    {
	        chosenNodes.Add(field);
	    }
	}
}