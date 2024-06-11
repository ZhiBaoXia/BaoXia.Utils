using BaoXia.Utils.MathTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.MathToolsTest
{
	[TestClass]
	public class AverageTest
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
		public void ObjectAverageTest()
		{
			var numberList = new List<TestNumberObject<int>>();
			var numbersSum = 0;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				numbersSum += number;
				numberList.Add(new TestNumberObject<int>(number));
			}
			var numbersAverage = numberList.Count > 0
				? numbersSum / numberList.Count
				: 0;
			var averageOfNumberList
				= Average.OfList(
				(TestNumberObject<int>? numberA,
				TestNumberObject<int>? numberB) =>
				{
					return new TestNumberObject<int>(
						(numberA?.Number ?? 0)
						+ (numberB?.Number ?? 0));
				},
				(int numbersCount,
				TestNumberObject<int>? numberSum) =>
				{
					return new TestNumberObject<int>(
						numbersCount > 0
						? ((numberSum?.Number ?? 0) / numbersCount)
						: 0);
				},
				numberList)?.Number;
			Assert.IsTrue(averageOfNumberList == numbersAverage);

			numbersAverage = (32 + 64 + 128) / 3;
			averageOfNumberList
				= Average.Of(
				(TestNumberObject<int>? numberA,
				TestNumberObject<int>? numberB) =>
				{
					return new TestNumberObject<int>(
						(numberA?.Number ?? 0)
						+ (numberB?.Number ?? 0));
				},
				(int numbersCount,
				TestNumberObject<int>? numberSum) =>
				{
					return new TestNumberObject<int>(
						numbersCount > 0
						? ((numberSum?.Number ?? 0) / numbersCount)
						: 0);
				},
				new TestNumberObject<int>(32),
				new TestNumberObject<int>(64),
				new TestNumberObject<int>(128))?.Number;
			Assert.IsTrue(averageOfNumberList == numbersAverage);
		}

		[TestMethod]
		public void IntAverageTest()
		{
			var numberList = new List<int>();
			var numbersSum = 0;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				numbersSum += number;
				numberList.Add(number);
			}
			var numbersAverage = numberList.Count > 0
				? numbersSum / numberList.Count
				: 0;
			var averageOfNumberList = Average.OfList(numberList);
			Assert.IsTrue(averageOfNumberList == numbersAverage);

			numbersAverage = (32 + 64 + 128) / 3;
			averageOfNumberList = Average.Of(32, 64, 128);
			Assert.IsTrue(averageOfNumberList == numbersAverage);
		}

		[TestMethod]
		public void FloatAverageTest()
		{
			var numberList = new List<float>();
			var numbersSum = 0.0F;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = Random.Shared.Next();
				numbersSum += number;
				numberList.Add(number);
			}
			var numbersAverage = numberList.Count > 0
				? numbersSum / (float)numberList.Count
				: 0.0F;
			var averageOfNumberList = Average.OfList(numberList);
			Assert.IsTrue(averageOfNumberList == numbersAverage);

			numbersAverage = (32.0F + 64.0F + 128.0F) / 3.0F;
			averageOfNumberList = Average.Of(32.0F, 64.0F, 128.0F);
			Assert.IsTrue(averageOfNumberList == numbersAverage);
		}

		[TestMethod]
		public void DoubleAverageTest()
		{
			var numberList = new List<double>();
			var numbersSum = 0.0;
			var numberListLength = Random.Shared.Next(10000);
			for (var numberIndex = 0;
				numberIndex < numberListLength;
				numberIndex++)
			{
				var number = (double)Random.Shared.Next();
				numbersSum += number;
				numberList.Add(number);
			}
			var numbersAverage = numberList.Count > 0
				? numbersSum / (double)numberList.Count
				: 0.0;
			var averageOfNumberList = Average.OfList(numberList);
			Assert.IsTrue(averageOfNumberList == numbersAverage);

			numbersAverage = (32.0 + 64.0 + 128.0) / 3.0;
			averageOfNumberList = Average.Of(32.0, 64.0, 128.0);
			Assert.IsTrue(averageOfNumberList == numbersAverage);
		}
	}
}
