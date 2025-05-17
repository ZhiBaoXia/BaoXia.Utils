using BaoXia.Utils.MathTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.MathToolsTest
{
	[TestClass]
	public class MaxTest
	{
		public class TestNumberObject<NumberType>
		{
			public NumberType Number { get; set; }

			public TestNumberObject(NumberType number)
			{
				Number = number;
			}
		}

		[TestMethod]
		public void ObjectMaxTest()
		{
			var numberList = new List<TestNumberObject<int>>();
			var numbersMax = 0;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				if (numbersMax < number)
				{
					numbersMax = number;
				}
				numberList.Add(new TestNumberObject<int>(number));
			}
			var maxOfNumberList
				= Max.OfList(
					(numberA, numberB) =>
					{
						if ((numberA?.Number ?? int.MinValue)
						> (numberB?.Number ?? int.MinValue))
						{
							return numberA;
						}
						return numberB;
					},
					numberList)
				?.Number;
			Assert.AreEqual(numbersMax, maxOfNumberList);

			numbersMax = 128;
			maxOfNumberList
				= Max.Of(
					(numberA, numberB) =>
					{
						if ((numberA?.Number ?? int.MinValue)
						> (numberB?.Number ?? int.MinValue))
						{
							return numberA;
						}
						return numberB;
					},
				new TestNumberObject<int>(32),
				new TestNumberObject<int>(64),
				new TestNumberObject<int>(128))?.Number;
			Assert.AreEqual(numbersMax, maxOfNumberList);
		}

		[TestMethod]
		public void IntMaxTest()
		{
			var numberList = new List<int>();
			var numbersMax = int.MinValue;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				if (numbersMax < number)
				{
					numbersMax = number;
				}
				numberList.Add(number);
			}
			var maxOfNumberList = Max.OfList(numberList);
			Assert.AreEqual(numbersMax, maxOfNumberList);

			numbersMax = 128;
			maxOfNumberList = Max.Of(32, 64, 128);
			Assert.AreEqual(numbersMax, maxOfNumberList);
		}

		[TestMethod]
		public void FloatMaxTest()
		{
			var numberList = new List<float>();
			var numbersMax = float.MinValue;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				if (numbersMax < number)
				{
					numbersMax = number;
				}
				numberList.Add(number);
			}
			var maxOfNumberList = Max.OfList(numberList);
			Assert.AreEqual(numbersMax, maxOfNumberList);

			numbersMax = 128.0F;
			maxOfNumberList = Max.Of(32.0F, 64.0F, 128.0F);
			Assert.AreEqual(numbersMax, maxOfNumberList);
		}

		[TestMethod]
		public void DoubleMaxTest()
		{
			var numberList = new List<double>();
			var numbersMax = double.MinValue;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = (double)Random.Shared.Next();
				if (numbersMax < number)
				{
					numbersMax = number;
				}
				numberList.Add(number);
			}
			var maxOfNumberList = Max.OfList(numberList);
			Assert.AreEqual(numbersMax, maxOfNumberList);

			numbersMax = 128.0;
			maxOfNumberList = Max.Of(32.0, 64.0, 128.0);
			Assert.AreEqual(numbersMax, maxOfNumberList);
		}
	}
}
