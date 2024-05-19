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
                        Assert.IsTrue(ArrayUtil.IsEmpty(items) == true);
                        Assert.IsTrue(ArrayUtil.IsNotEmpty(items) == false);
                }
                items = System.Array.Empty<int>();
                {
                        Assert.IsTrue(ArrayUtil.IsEmpty(items) == true);
                        Assert.IsTrue(ArrayUtil.IsNotEmpty(items) == false);
                }
                items = new int[] { 1 };
                {
                        Assert.IsTrue(ArrayUtil.IsEmpty(items) == false);
                        Assert.IsTrue(ArrayUtil.IsNotEmpty(items) == true);
                }
                items = new int[] { 1, 2, 3 };
                {
                        Assert.IsTrue(ArrayUtil.IsEmpty(items) == false);
                        Assert.IsTrue(ArrayUtil.IsNotEmpty(items) == true);
                }
        }
}
