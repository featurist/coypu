using System;
using System.Collections.Generic;

namespace Coypu.Tests.TestDoubles
{
	public class FakeDriver : Driver
	{
		private readonly IList<Element> clickedElements = new List<Element>();
		private readonly IList<Element> checkedElements = new List<Element>();
		private readonly IList<Element> uncheckedElements = new List<Element>();
		private readonly IList<Element> chosenElements = new List<Element>();
		private readonly IList<string> hasContentQueries = new List<string>();
		private readonly IList<string> hasCssQueries = new List<string>();
		private readonly IList<string> hasXPathQueries = new List<string>();
		private readonly IList<string> visits = new List<string>();
		private readonly IDictionary<Element, string> setFields = new Dictionary<Element, string>();
		private readonly IDictionary<Element, string> selectedOptions = new Dictionary<Element, string>();
		private readonly Dictionary<string, Element> stubbedButtons = new Dictionary<string, Element>();
		private readonly Dictionary<string, Element> stubbedLinks = new Dictionary<string, Element>();
		private readonly Dictionary<string, Element> stubbedTextFields = new Dictionary<string, Element>();
		private readonly Dictionary<string, Element> stubbedCssResults = new Dictionary<string, Element>();
		private readonly Dictionary<string, Element> stubbedXPathResults = new Dictionary<string, Element>();
		private readonly IDictionary<string, IEnumerable<Element>> stubbedAllCssResults = new Dictionary<string, IEnumerable<Element>>();
		private readonly IDictionary<string, IEnumerable<Element>> stubbedAllXPathResults = new Dictionary<string, IEnumerable<Element>>();
		private readonly IDictionary<string, bool> stubbedHasContentResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasCssResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasXPathResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasDialogResults = new Dictionary<string, bool>();

		public IEnumerable<Element> ClickedElements
		{
			get { return clickedElements; }
		}

		public IDictionary<Element, string> SetFields
		{
			get { return setFields; }
		}

		public IDictionary<Element, string> SelectedOptions
		{
			get { return selectedOptions; }
		}

		public IEnumerable<Element> CheckedElements
		{
			get { return checkedElements; }
		}

		public IEnumerable<Element> ChosenElements
		{
			get { return chosenElements; }
		}

		public IEnumerable<Element> UncheckedElements
		{
			get { return uncheckedElements; }
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

		public Element FindButton(string locator)
		{
			return stubbedButtons[locator];
		}

		public Element FindLink(string locator)
		{
			return stubbedLinks[locator];
		}

		public Element FindField(string locator)
		{
			return stubbedTextFields[locator];
		}

		public void Click(Element element)
		{
			clickedElements.Add(element);
		}

		public void Visit(string url)
		{
			visits.Add(url);
		}

		public void StubButton(string locator, Element element)
		{
			stubbedButtons[locator] = element;
		}

		public void StubLink(string locator, Element element)
		{
			stubbedLinks[locator] = element;
		}

		public void StubField(string locator, Element element)
		{
			stubbedTextFields[locator] = element;
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

		public void StubDialog(string text, bool result)
		{
			stubbedHasDialogResults.Add(text, result);
		}

		public void StubCss(string cssSelector, Element result)
		{
			stubbedCssResults.Add(cssSelector, result);
		}

		public void StubXPath(string cssSelector, Element result)
		{
			stubbedXPathResults.Add(cssSelector, result);
		}

		public void StubAllCss(string cssSelector, IEnumerable<Element> result)
		{
			stubbedAllCssResults.Add(cssSelector, result);
		}

		public void StubAllXPath(string xpath, IEnumerable<Element> result)
		{
			stubbedAllXPathResults.Add(xpath, result);
		}

		public void Dispose()
		{
			Disposed = true;
		}

		public bool Disposed { get; private set; }
		public void AcceptModalDialog()
		{
			ModalDialogsAccepted++;
		}

		public void CancelModalDialog()
		{
			ModalDialogsCancelled++;
		}

		public void Set(Element element, string value)
		{
			setFields.Add(element, value);
		}

		public void Select(Element element, string option)
		{
			selectedOptions.Add(element, option);
		}

		public object Native
		{
			get { return "Native driver on fake driver"; }
		}

		public int ModalDialogsAccepted { get; private set; }
		public int ModalDialogsCancelled { get; private set; }

		public bool HasContent(string text)
		{
			hasContentQueries.Add(text);
			return stubbedHasContentResults[text];
		}

		public bool HasCss(string cssSelector)
		{
			return stubbedHasCssResults[cssSelector];
		}

		public bool HasXPath(string xpath)
		{
			return stubbedHasXPathResults[xpath];
		}

		public bool HasDialog(string withText)
		{
			return stubbedHasDialogResults[withText];
		}

		public Element FindCss(string cssSelector)
		{
			return stubbedCssResults[cssSelector];
		}

		public Element FindXPath(string xpath)
		{
			return stubbedXPathResults[xpath];
		}

		public IEnumerable<Element> FindAllCss(string cssSelector)
		{
			return stubbedAllCssResults[cssSelector];
		}

		public IEnumerable<Element> FindAllXPath(string xpath)
		{
			return stubbedAllXPathResults[xpath];
		}

		public void Check(Element field)
		{
			checkedElements.Add(field);
		}

		public void Uncheck(Element field)
		{
			uncheckedElements.Add(field);
		}

		public void Choose(Element field)
		{
			chosenElements.Add(field);
		}
	}
}