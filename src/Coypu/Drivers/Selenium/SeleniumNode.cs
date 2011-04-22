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
			Update();
		}

		public override void Update()
		{
			Text = SeleniumElement.Text;
			Id = SeleniumElement.GetAttribute("id");
			Value = SeleniumElement.GetAttribute("value");
			Name = SeleniumElement.GetAttribute("name");
			SetSelectedOptionForSelectElements();
		}

		private void SetSelectedOptionForSelectElements()
		{
			var selectedOption = SeleniumElement.FindElements(By.TagName("option"))
												.Where(e => e.Selected)
												.FirstOrDefault();
			if (selectedOption != null)
				SelectedOption = selectedOption.Text;
		}
	}
}