using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test;

[TestClass]
public class ArrayUtilTest
{
	[TestMethod]
	public void IsEmptyTest()
	{
		int[]? items = null;
		{
			Assert.IsTrue(ArrayUtil.IsEmpty(items));
			Assert.IsFalse(ArrayUtil.IsNotEmpty(items));
		}
		items = System.Array.Empty<int>();
		{
			Assert.IsTrue(ArrayUtil.IsEmpty(items));
			Assert.IsFalse(ArrayUtil.IsNotEmpty(items));
		}
		items = [1];
		{
			Assert.IsFalse(ArrayUtil.IsEmpty(items));
			Assert.IsTrue(ArrayUtil.IsNotEmpty(items));
		}
		items = [1, 2, 3];
		{
			Assert.IsFalse(ArrayUtil.IsEmpty(items));
			Assert.IsTrue(ArrayUtil.IsNotEmpty(items));
		}
	}
}
