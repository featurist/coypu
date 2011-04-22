using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
	public class SeleniumNode : Node
	{
		private IWebElement SeleniumElement
		{
			get { return (IWebElement) UnderlyingNode; }
		}
		public SeleniumNode(IWebElement seleniumElement)
		{
			UnderlyingNode = seleniumElement;
			Update();
		}

		public override void Update()
		{
			Text = SeleniumElement.Text;
			Id = SeleniumElement.GetAttribute("id");
			Value = SeleniumElement.GetAttribute("value");
		}
	}
}