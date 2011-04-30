using System;
using System.IO;
using System.Reflection;
using Coypu.Drivers.Selenium;
using NSpec;

namespace Coypu.Drivers.Tests
{
	public class driver_implementation_specs : nspec
	{
		Driver driver;

		public void testing_each_driver()
		{
			Type driverType = null;

			context["when the driver is SeleniumWebDriver"] = () => 
			{
				before = () => { driverType = typeof(SeleniumWebDriver); };
				context["and the browser is Firefox"] = () =>
				{
				    before = () => { EnsureDriver(driverType, Browser.Firefox); };
				    describe["is a valid coypu driver"] = GetDriverSpecs();
				};
				context["the browser is Chrome"] = () =>
				{
					before = () => { EnsureDriver(driverType, Browser.Chrome); };
					describe["is a valid coypu driver"] = GetDriverSpecs();
				};
				context["and the browser is Internet Explorer"] = () =>
              	{
					before = () => { EnsureDriver(driverType, Browser.InternetExplorer); };
					describe["is a valid coypu driver"] = GetDriverSpecs();
              	};
			};
		}

		private void EnsureDriver(Type driverType, Browser browser)
		{
			if (driver != null && driverType == driver.GetType() && Configuration.Browser == browser)
				return;

			if (driver != null)
				driver.Dispose();

			Configuration.Browser = browser;
			driver = (Driver)Activator.CreateInstance(driverType);
		}


		private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
		
		private string GetTestHTMLPathLocation()
		{
			var assemblyDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
			var projRoot = Path.Combine(assemblyDirectory,@"..\..\");
			return new FileInfo(Path.Combine(projRoot,INTERACTION_TESTS_PAGE)).FullName;
		}

		public Action GetDriverSpecs()
		{
			return () =>
			{
				before = () => driver.Visit(GetTestHTMLPathLocation());
				describe["finding buttons"] = () =>
				{
					it["should_find_a_particular_button_by_its_text"] = () =>
					{
						driver.FindButton("first button").Id.should_be("firstButtonId");
						driver.FindButton("second button").Id.should_be("secondButtonId");
					};

					it["should_find_a_particular_button_by_its_id"] = () =>
					{
						driver.FindButton("firstButtonId").Text.should_be("first button");
						driver.FindButton("thirdButtonId").Text.should_be("third button");
					};

					it["should_find_a_particular_button_by_its_name"] = () =>
					{
						driver.FindButton("secondButtonName").Text.should_be("second button");
						driver.FindButton("thirdButtonName").Text.should_be("third button");
					};

					it["should_find_a_particular_input_button_by_its_value"] = () =>
					{
						driver.FindButton("first input button").Id.should_be("firstInputButtonId");
						driver.FindButton("second input button").Id.should_be("secondInputButtonId");
					};

					it["should_find_a_particular_input_button_by_its_id"] = () =>
					{
						driver.FindButton("firstInputButtonId").Value.should_be("first input button");
						driver.FindButton("thirdInputButtonId").Value.should_be("third input button");
					};

					it["should_find_a_particular_input_button_by_its_name"] = () =>
					{
						driver.FindButton("secondInputButtonId").Value.should_be("second input button");
						driver.FindButton("thirdInputButtonName").Value.should_be("third input button");
					};

					it["should_find_a_particular_submit_button_by_its_value"] = () =>
					{
						driver.FindButton("first submit button").Id.should_be("firstSubmitButtonId");
						driver.FindButton("second submit button").Id.should_be("secondSubmitButtonId");
					};

					it["should_find_a_particular_submit_button_by_its_id"] = () =>
					{
						driver.FindButton("firstSubmitButtonId").Value.should_be("first submit button");
						driver.FindButton("thirdSubmitButtonId").Value.should_be("third submit button");
					};

					it["should_find_a_particular_submit_button_by_its_name"] = () =>
					{
						driver.FindButton("secondSubmitButtonId").Value.should_be("second submit button");
						driver.FindButton("thirdSubmitButtonName").Value.should_be("third submit button");
					};

					it["should_find_image_buttons"] = () =>
					{
						driver.FindButton("firstImageButtonId").Value.should_be("first image button");
						driver.FindButton("secondImageButtonId").Value.should_be("second image button");
					};

					it["should_not_find_text_inputs"] = () =>
					{

						expect<MissingHtmlException>(() => driver.FindButton("firstTextInputId"));
					};

					it["should_not_find_hidden_inputs"] = () =>
					{
						expect<MissingHtmlException>(() => driver.FindButton("firstHiddenInputId"));
					};

					it["should_not_find_invisible_inputs"] = () =>
					{
						expect<MissingHtmlException>(() => driver.FindButton("firstInvisibleInputId"));
					};

				};
				describe["finding links"] = () =>
				{
					it["should_find_link_by_text"] = () =>
					{
						driver.FindLink("first link").Id.should_be("firstLinkId");
						driver.FindLink("second link").Id.should_be("secondLinkId");
					};

					it["should not find display:none"] = () =>
					{
						expect<MissingHtmlException>(() => driver.FindLink("I am an invisible link by display"));
					};

					it["should not find visibility:hidden links"] = () =>
					{
						expect<MissingHtmlException>(() => driver.FindLink("I am an invisible link by visibility"));
					};
				};

				describe["clicking"] = () =>
				{
					it["should_click_the_underlying_node"] = () =>
					{
						var node = driver.FindButton("clickMeTest");
						driver.FindButton("clickMeTest").Text.should_be("Click me");
						driver.Click(node);
						driver.FindButton("clickMeTest").Text.should_be("Click me - clicked");
					};
				};
			};
		}
	}
}