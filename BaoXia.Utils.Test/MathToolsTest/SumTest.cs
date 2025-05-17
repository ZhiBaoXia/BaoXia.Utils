using BaoXia.Utils.MathTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.MathToolsTest
{
	[TestClass]
	public class SumTest
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
		public void ObjectSumTest()
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
			var sumOfNumberList
				= Sum.OfList(
				(TestNumberObject<int>? numberA,
				TestNumberObject<int>? numberB) =>
				{
					return new TestNumberObject<int>(
						(numberA?.Number ?? 0)
						+ (numberB?.Number ?? 0));
				},
				numberList)
				?.Number;
			Assert.AreEqual(numbersSum, sumOfNumberList);

			numbersSum = (32 + 64 + 128);
			sumOfNumberList
				= Sum.Of(
				(TestNumberObject<int>? numberA,
				TestNumberObject<int>? numberB) =>
				{
					return new TestNumberObject<int>(
						(numberA?.Number ?? 0)
						+ (numberB?.Number ?? 0));
				},
				new TestNumberObject<int>(32),
				new TestNumberObject<int>(64),
				new TestNumberObject<int>(128))?.Number;
			Assert.AreEqual(numbersSum, sumOfNumberList);
		}

		[TestMethod]
		public void IntSumTest()
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
			var sumOfNumberList = Sum.OfList(numberList);
			Assert.AreEqual(numbersSum, sumOfNumberList);

			numbersSum = (32 + 64 + 128);
			sumOfNumberList = Sum.Of(32, 64, 128);
			Assert.AreEqual(numbersSum, sumOfNumberList);
		}

		[TestMethod]
		public void FloatSumTest()
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
			var sumOfNumberList = Sum.OfList(numberList);
			Assert.AreEqual(numbersSum, sumOfNumberList);

			numbersSum = (32.0F + 64.0F + 128.0F);
			sumOfNumberList = Sum.Of(32.0F, 64.0F, 128.0F);
			Assert.AreEqual(numbersSum, sumOfNumberList);
		}

		[TestMethod]
		public void DoubleSumTest()
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
			var sumOfNumberList = Sum.OfList(numberList);
			Assert.AreEqual(numbersSum, sumOfNumberList);

			numbersSum = 32.0 + 64.0 + 128.0;
			sumOfNumberList = Sum.Of(32.0, 64.0, 128.0);
			Assert.AreEqual(numbersSum, sumOfNumberList);
		}
	}
}
