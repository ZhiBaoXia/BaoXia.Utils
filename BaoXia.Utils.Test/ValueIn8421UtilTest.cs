
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class ValueIn8421UtilTest
{
	[TestMethod]
	public void SetTest()
	{
		int testValue = 0;

		testValue = ValueIn8421Util.SetValue(testValue, 0x01);
		{
			Assert.AreEqual(0x01, testValue);
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x01));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x02));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x04));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x08));
		}

		testValue = ValueIn8421Util.SetValue(testValue, 0x02);
		{
			Assert.AreEqual(0x01 | 0x02, testValue);
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x01));
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x02));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x04));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x08));
		}

		testValue = ValueIn8421Util.SetValue(testValue, 0x04);
		{
			Assert.AreEqual(0x01 | 0x02 | 0x04, testValue);
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x01));
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x02));
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x04));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x08));
		}

		testValue = ValueIn8421Util.RemoveValue(testValue, 0x02);
		{
			Assert.AreEqual(0x01 | 0x04, testValue);
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x01));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x02));
			Assert.IsTrue(ValueIn8421Util.IsContains(testValue, 0x04));
			Assert.IsTrue(ValueIn8421Util.IsNotContains(testValue, 0x08));
		}
	}
}