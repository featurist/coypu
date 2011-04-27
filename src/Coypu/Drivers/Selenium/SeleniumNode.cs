using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
	public class SeleniumNode : Node
	{
		private IWebElement SeleniumElement
		{
			get { return (IWebElement) Native; }
		}

		public SeleniumNode(IWebElement seleniumElement)
		{
			Native = seleniumElement;
		}

		public override string Id
		{
			get { return SeleniumElement.GetAttribute("id"); }
		}

		public override string Text
		{
			get { return SeleniumElement.Text; }
		}

		public override string Value
		{
			get { return SeleniumElement.GetAttribute("value"); }
		}

		public override string Name
		{
			get { return SeleniumElement.GetAttribute("name"); }
		}

		public override string SelectedOption
		{
			get
			{
				return SeleniumElement.FindElements(By.TagName("option"))
					.Where(e => e.Selected)
					.Select(e => e.Text)
					.FirstOrDefault();
			}
		}

	    public override bool Selected
	    {
            get { return SeleniumElement.Selected; }
	    }
	}
}