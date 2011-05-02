using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
	public class SeleniumElement : Element
	{
		private IWebElement NativeElement
		{
			get { return (IWebElement) Native; }
		}

		public SeleniumElement(IWebElement seleniumElement)
		{
			Native = seleniumElement;
		}

		public override string Id
		{
			get { return NativeElement.GetAttribute("id"); }
		}

		public override string Text
		{
			get { return NativeElement.Text; }
		}

		public override string Value
		{
			get { return NativeElement.GetAttribute("value"); }
		}

		public override string Name
		{
			get { return NativeElement.GetAttribute("name"); }
		}

		public override string SelectedOption
		{
			get
			{
				return NativeElement.FindElements(By.TagName("option"))
					.Where(e => e.Selected)
					.Select(e => e.Text)
					.FirstOrDefault();
			}
		}

		public override bool Selected
		{
			get { return NativeElement.Selected; }
		}
	}
}