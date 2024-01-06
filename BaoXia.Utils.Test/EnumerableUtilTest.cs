using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BaoXia.Utils.Test;

[TestClass]
public class EnumerableUtilTest
{
	[TestMethod]
	public void IsEmptyTest()
	{
		var testItems = new List<int>()
		{
			0,
			1,
			2,
			3,
			4,
			5
		};

		// !!!
		Assert.IsTrue(EnumerableUtil.IsEmpty(testItems) == false);
		Assert.IsTrue(EnumerableUtil.IsNotEmpty(testItems) == true);
		// !!! 

		testItems.Clear();

		// !!!
		Assert.IsTrue(EnumerableUtil.IsEmpty(testItems) == true);
		Assert.IsTrue(EnumerableUtil.IsNotEmpty(testItems) == false);
		// !!! 
	}
}
