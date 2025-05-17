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
	public void ValueNamesOfEnum()
	{
		var valueNamesOfEnum = EnumUtil.ValueAndNamesOf<TestEnum>();
		Assert.IsTrue(valueNamesOfEnum?.Count > 0);
		////////////////////////////////////////////////

		foreach (var valueName in valueNamesOfEnum)
		{
			switch (valueName.Key)
			{
				default:
					{
						Assert.IsTrue(false);
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
	public void EnumValueName()
	{
		var enumValue_1_Name = TestEnum.Value_1.Name();
		{
			Assert.AreEqual(
TestEnum.Value_1, enumValue_1_Name?.EnumValue(TestEnum.Value_0)
);
		}
	}
}
