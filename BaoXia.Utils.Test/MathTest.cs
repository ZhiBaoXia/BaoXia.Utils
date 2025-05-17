using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class MathTest
	{
		[TestMethod]
		public void GreatestCommonDivisorTest()
		{
			var numberA = 5;
			var numberB = 25;

			var greatestCommonDivisor = BigInteger.GreatestCommonDivisor(
				numberA,
				numberB);

			Assert.AreEqual(5, greatestCommonDivisor);
		}
	}
}
