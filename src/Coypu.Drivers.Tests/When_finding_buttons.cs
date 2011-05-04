using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	public class When_finding_buttons : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it, Action<Action> setBefore)
		{
			return () =>
			{
//				it["should find a particular button by its text"] = () =>
//				{
//					driver().FindButton("first button").Id.should_be("firstButtonId");
//					driver().FindButton("second button").Id.should_be("secondButtonId");
//				};
//
//				it["should find a particular button by its id"] = () =>
//				{
//					driver().FindButton("firstButtonId").Text.should_be("first button");
//					driver().FindButton("thirdButtonId").Text.should_be("third button");
//				};
//
//				it["should find a particular button by its name"] = () =>
//				{
//					driver().FindButton("secondButtonName").Text.should_be("second button");
//					driver().FindButton("thirdButtonName").Text.should_be("third button");
//				};
//
//				it["should find a particular input button by its value"] = () =>
//				{
//					driver().FindButton("first input button").Id.should_be("firstInputButtonId");
//					driver().FindButton("second input button").Id.should_be("secondInputButtonId");
//				};
//
//				it["should find a particular input button by its id"] = () =>
//				{
//					driver().FindButton("firstInputButtonId").Value.should_be("first input button");
//					driver().FindButton("thirdInputButtonId").Value.should_be("third input button");
//				};
//
//				it["should find a particular input button by its name"] = () =>
//				{
//					driver().FindButton("secondInputButtonId").Value.should_be("second input button");
//					driver().FindButton("thirdInputButtonName").Value.should_be("third input button");
//				};
//
//				it["should find a particular submit button by its value"] = () =>
//				{
//					driver().FindButton("first submit button").Id.should_be("firstSubmitButtonId");
//					driver().FindButton("second submit button").Id.should_be("secondSubmitButtonId");
//				};
//
//				it["should find a particular submit button by its id"] = () =>
//				{
//					driver().FindButton("firstSubmitButtonId").Value.should_be("first submit button");
//					driver().FindButton("thirdSubmitButtonId").Value.should_be("third submit button");
//				};
//
//				it["should find a particular submit button by its name"] = () =>
//				{
//					driver().FindButton("secondSubmitButtonName").Value.should_be("second submit button");
//					driver().FindButton("thirdSubmitButtonName").Value.should_be("third submit button");
//				};
//
//				it["should find image buttons"] = () =>
//				{
//					driver().FindButton("firstImageButtonId").Value.should_be("first image button");
//					driver().FindButton("secondImageButtonId").Value.should_be("second image button");
//				};
//
//				it["should not find text inputs"] = () =>
//				{
//					Assert.Throws<MissingHtmlException>(() => driver().FindButton("firstTextInputId"));
//				};
//
//				it["should not find hidden inputs"] = () =>
//				{
//					Assert.Throws<MissingHtmlException>(() => driver().FindButton("firstHiddenInputId"));
//				};
//
//				it["should not find invisible inputs"] = () =>
//				{
//					Assert.Throws<MissingHtmlException>(() => driver().FindButton("firstInvisibleInputId"));
//				};

			   	describe["scoping"] = () =>
                {
					describe["within scope1"] = () =>
					{
						setBefore(() => driver().SetScope(driver().FindCss("#scope1")));

						it["should find button by name"] = () =>
						{
							driver().FindButton("scopedButtonName").Id.should_be("scope1ButtonId");
						};
						it["should find input button by value"] = () =>
						{
							driver().FindButton("scoped input button").Id.should_be("scope1InputButtonId");
						};
						it["should find button by text"] = () =>
						{
							driver().FindButton("scoped button").Id.should_be("scope1ButtonId");
						};
					};
					describe["within scope2"] = () =>
					{
						setBefore(() => driver().SetScope(driver().FindCss("#scope2")));

						it["should find button by name"] = () =>
						{
							driver().FindButton("scopedButtonName").Id.should_be("scope2ButtonId");
						};
						it["should find input button by value"] = () =>
						{
							driver().FindButton("scoped input button").Id.should_be("scope2InputButtonId");
						};
						it["should find button by text"] = () =>
						{
							driver().FindButton("scoped button").Id.should_be("scope2ButtonId");
						};
					};
                };
			};
		}
	}
}