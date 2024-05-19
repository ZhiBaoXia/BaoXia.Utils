using BaoXia.Utils.MathTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.MathToolsTest
{
        [TestClass]
        public class MinTest
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
                public void ObjectMinTest()
                {
                        var numberList = new List<TestNumberObject<int>>();
                        var numbersMin = int.MaxValue;
                        var numberListLength = Random.Shared.Next(10000);
                        for (var numberIndex = 0;
                                numberIndex < numberListLength;
                                numberIndex++)
                        {
                                var number = Random.Shared.Next();
                                if (numbersMin > number)
                                {
                                        numbersMin = number;
                                }
                                numberList.Add(new TestNumberObject<int>(number));
                        }
                        var minOfNumberList
                                = Min.OfList(
                                        (numberA, numberB) =>
                                        {
                                                if ((numberA?.Number ?? int.MaxValue)
                                                < (numberB?.Number ?? int.MaxValue))
                                                {
                                                        return numberA;
                                                }
                                                return numberB;
                                        },
                                        numberList)
                                ?.Number;
                        Assert.IsTrue(minOfNumberList == numbersMin);

                        numbersMin = 32;
                        minOfNumberList
                                = Min.Of(
                                        (numberA, numberB) =>
                                        {
                                                if ((numberA?.Number ?? int.MaxValue)
                                                < (numberB?.Number ?? int.MaxValue))
                                                {
                                                        return numberA;
                                                }
                                                return numberB;
                                        },
                                new TestNumberObject<int>(32),
                                new TestNumberObject<int>(64),
                                new TestNumberObject<int>(128))?.Number;
                        Assert.IsTrue(minOfNumberList == numbersMin);
                }

                [TestMethod]
                public void IntMinTest()
                {
                        var numberList = new List<int>();
                        var numbersMin = int.MaxValue;
                        var numberListLength = Random.Shared.Next(10000);
                        for (var numberIndex = 0;
                                numberIndex < numberListLength;
                                numberIndex++)
                        {
                                var number = Random.Shared.Next();
                                if (numbersMin > number)
                                {
                                        numbersMin = number;
                                }
                                numberList.Add(number);
                        }
                        var minOfNumberList = Min.OfList(numberList);
                        Assert.IsTrue(minOfNumberList == numbersMin);

                        numbersMin = 32;
                        minOfNumberList = Min.Of(32, 64, 128);
                        Assert.IsTrue(minOfNumberList == numbersMin);
                }

                [TestMethod]
                public void FloatMinTest()
                {
                        var numberList = new List<float>();
                        var numbersMin = float.MaxValue;
                        var numberListLength = Random.Shared.Next(10000);
                        for (var numberIndex = 0;
                                numberIndex < numberListLength;
                                numberIndex++)
                        {
                                var number = Random.Shared.Next();
                                if (numbersMin > number)
                                {
                                        numbersMin = number;
                                }
                                numberList.Add(number);
                        }
                        var minOfNumberList = Min.OfList(numberList);
                        Assert.IsTrue(minOfNumberList == numbersMin);

                        numbersMin = 32.0F;
                        minOfNumberList = Min.Of(32.0F, 64.0F, 128.0F);
                        Assert.IsTrue(minOfNumberList == numbersMin);
                }

                [TestMethod]
                public void DoubleMinTest()
                {
                        var numberList = new List<double>();
                        var numbersMin = double.MaxValue;
                        var numberListLength = Random.Shared.Next(10000);
                        for (var numberIndex = 0;
                                numberIndex < numberListLength;
                                numberIndex++)
                        {
                                var number = (double)Random.Shared.Next();
                                if (numbersMin > number)
                                {
                                        numbersMin = number;
                                }
                                numberList.Add(number);
                        }
                        var minOfNumberList = Min.OfList(numberList);
                        Assert.IsTrue(minOfNumberList == numbersMin);

                        numbersMin = 32.0;
                        minOfNumberList = Min.Of(32.0, 64.0, 128.0);
                        Assert.IsTrue(minOfNumberList == numbersMin);
                }
        }
}
