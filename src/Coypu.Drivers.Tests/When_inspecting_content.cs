using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_inspecting_content : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it)
		{
			return () =>
			{
				it["should not find missing text"] = () =>
				{
					driver().HasContent("Some missing text").should_be_false();
				};

				it["should find text with parts marked up variously"] = () =>
				{
					driver().HasContent("Some text with parts marked up variously").should_be_true();
				};

				it["should find text in a table row"] = () =>
				{
					driver().HasContent("Some text in a table row").should_be_true();
				};

				it["should find text in a list"] = () =>
				{
					driver().HasContent("Some text in a list").should_be_true();
				};

				it["should find text split over multiple lines in source"] = () =>
				{
					driver().HasContent("Some text split over multiple lines in source").should_be_true();
				};

				it["should find text displayed over multiple lines in source"] = () =>
				{
					driver().HasContent("Some text displayed over\r\nmultiple lines").should_be_true();
					driver().HasContent("Some text displayed over\r\ntwo paragraphs").should_be_true();
				};

				it["should not find single line text displayed over multiple lines in source"] = () =>
				{
					driver().HasContent("Some text displayed over multiple lines").should_be_false();
				};
			};
		}
	}
}