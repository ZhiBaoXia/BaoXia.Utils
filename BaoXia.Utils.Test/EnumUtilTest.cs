using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class EnumUtilTest
{
	enum TestEnum
	{
		Value_0 = 0,
		Value_1 = 1,
		Value_2 = 2,
		Value_3 = 3,
	}

	[TestMethod]
	public void ValueNamesOfEnumTest()
	{
		var valueNamesOfEnum = EnumUtil.ValueAndNamesOf<TestEnum>();
		Assert.IsNotNull(valueNamesOfEnum);
		Assert.IsTrue(valueNamesOfEnum.Count > 0);
		////////////////////////////////////////////////

		foreach (var valueName in valueNamesOfEnum)
		{
			switch (valueName.Key)
			{
				default:
					{
						Assert.Fail();
					}
					break;
				case TestEnum.Value_0:
					{
						Assert.IsTrue(TestEnum.Value_0
						    .ToString()
						    .EqualsIgnoreCase(valueName.Value));
					}
					break;
				case TestEnum.Value_1:
					{
						Assert.IsTrue(TestEnum.Value_1
						    .ToString()
						    .EqualsIgnoreCase(valueName.Value));
					}
					break;
				case TestEnum.Value_2:
					{
						Assert.IsTrue(TestEnum.Value_2
						    .ToString()
						    .EqualsIgnoreCase(valueName.Value));
					}
					break;
				case TestEnum.Value_3:
					{
						Assert.IsTrue(TestEnum.Value_3
						    .ToString()
						    .EqualsIgnoreCase(valueName.Value));
					}
					break;
			}
		}
		////////////////////////////////////////////////
	}

	[TestMethod]
	public void EnumValueNameTest()
	{
		var enumValue_1_Name = TestEnum.Value_1.Name();
		{
			Assert.IsNotNull(enumValue_1_Name);
			Assert.AreEqual(
				TestEnum.Value_1,
				enumValue_1_Name.EnumValue(TestEnum.Value_0));
		}
	}


	[TestMethod]
	public void NameOfTest()
	{
		var nameOfEnumValue = EnumUtil.NameOf(TestEnum.Value_1);
		Assert.AreEqual(
			"Value_1",
			nameOfEnumValue);

		nameOfEnumValue = EnumUtil.NameOf(TestEnum.Value_2);
		Assert.AreEqual(
			"Value_2",
			nameOfEnumValue);

		nameOfEnumValue = EnumUtil.NameOf((TestEnum)(-1));
		Assert.AreEqual(
			"-1",
			nameOfEnumValue);

		nameOfEnumValue = EnumUtil.NameOf((TestEnum)(999));
		Assert.AreEqual(
			"999",
			nameOfEnumValue);
	}
}
