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
		Assert.IsFalse(EnumerableUtil.IsEmpty(testItems));
		Assert.IsTrue(EnumerableUtil.IsNotEmpty(testItems));
		// !!! 

		testItems.Clear();

		// !!!
		Assert.IsTrue(EnumerableUtil.IsEmpty(testItems));
		Assert.IsFalse(EnumerableUtil.IsNotEmpty(testItems));
		// !!! 
	}
}
