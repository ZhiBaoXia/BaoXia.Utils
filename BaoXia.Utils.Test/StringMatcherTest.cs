using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test
{
        [TestClass]
        public class StringMatcherTest
        {
                [TestMethod]
                public void IsMatched_Default_SameChars_Test()
                {
                        var testString = "01234Abcdefghijklmn56789";
                        var stringMatcher = new StringMatcher(testString);

                        Assert.IsTrue(stringMatcher.IsMatched(testString));

                }

                [TestMethod]
                public void IsMatched_AnyChars_Test()
                {
                        ////////////////////////////////////////////////
                        // 1/3，中间是任意字符：
                        ////////////////////////////////////////////////

                        var stringMatcherWithAnyCharsIn_Begin = new StringMatcher("*56789");

                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Begin.IsMatched(
                                "01234Abcdefghijklmn56789"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Begin.IsMatched(
                                "****_56789"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Begin.IsNotMatched(
                                "**_567890"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Begin.IsNotMatched(
                                "01234_**_"));

                        ////////////////////////////////////////////////
                        // 2/3，中间是任意字符：
                        ////////////////////////////////////////////////

                        var stringMatcherWithAnyCharsIn_Mid = new StringMatcher("01234*56789");

                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Mid.IsMatched(
                                "01234Abcdefghijklmn56789"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Mid.IsMatched(
                                "01234_**_56789"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Mid.IsNotMatched(
                                "01234_**_567890"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_Mid.IsNotMatched(
                                "01234_**_"));

                        ////////////////////////////////////////////////
                        // 3/3，结尾是任意字符：
                        ////////////////////////////////////////////////

                        var stringMatcherWithAnyCharsIn_End = new StringMatcher("01234*");


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_End.IsMatched(
                                "01234Ab"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_End.IsMatched(
                                "01234Abcdefghijklmn56789"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_End.IsMatched(
                                "01234****__"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_End.IsNotMatched(
                                "0123_**abcdefghijklmnopqrstuvwxyz"));


                        Assert.IsTrue(stringMatcherWithAnyCharsIn_End.IsNotMatched(
                                "*01234_**abcdefghijklmnopqrstuvwxyz"));
                }


                [TestMethod]
                public void IsMatched_Expression_Test()
                {
                        ////////////////////////////////////////////////
                        // 纯数值比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_PureNumber = new StringMatcher("0123[$Substring==456]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber.IsMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber.IsNotMatched(
                                        "0123654789"));
                        }

                        ////////////////////////////////////////////////
                        // 纯数值，或操作比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_PureNumber_Or = new StringMatcher("0123[($Substring==456) || ($Substring==654)]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber_Or.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber_Or.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber_Or.IsMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_PureNumber_Or.IsMatched(
                                        "0123654789"));
                        }

                        ////////////////////////////////////////////////
                        // 多个，纯数值，或操作比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_MultiplePureNumbers_Or
                                = new StringMatcher("0[($Substring==12) || ($Substring==21)]3[($Substring==456) || ($Substring==654)]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsMatched(
                                        "0123654789"));


                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "02134Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "021346789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsMatched(
                                        "0213456789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsMatched(
                                        "0213654789"));



                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "01134Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "011346789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "0113456789"));
                                Assert.IsTrue(stringMatcherWithExpression_MultiplePureNumbers_Or.IsNotMatched(
                                        "0113654789"));
                        }

                        ////////////////////////////////////////////////
                        // 数值计算比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_NumberCalculation = new StringMatcher("0123[$Substring%2==0]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsNotMatched(
                                        "01236543789"));
                        }
                        stringMatcherWithExpression_NumberCalculation = new StringMatcher("0123[$Substring/2==4]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsMatched(
                                        "01238789"));
                        }

                        stringMatcherWithExpression_NumberCalculation = new StringMatcher("[$Substring%2==1]");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsMatched(
                                        "949969"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation.IsNotMatched(
                                        "949970"));
                        }

                        ////////////////////////////////////////////////
                        // 数值计算，或，与操作比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_NumberCalculation_Or = new StringMatcher("0123[($Substring%2==0) || ($Substring%5==0)]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "01236543789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "012315789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "0123515789"));
                        }
                        stringMatcherWithExpression_NumberCalculation_Or = new StringMatcher("0123[($Substring/2==4) || ($Substring/5==4)]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "01238789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "012320789"));
                        }
                        stringMatcherWithExpression_NumberCalculation_Or = new StringMatcher("0123[($Substring/4==5) && ($Substring/5==4)]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "01234Abcdefghijklmn56789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "012346789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsNotMatched(
                                        "01238789"));
                                Assert.IsTrue(stringMatcherWithExpression_NumberCalculation_Or.IsMatched(
                                        "012320789"));
                        }


                        ////////////////////////////////////////////////
                        // 字符串计算，比较：
                        ////////////////////////////////////////////////

                        var stringMatcherWithExpression_StringCalculation = new StringMatcher("0123[Abc]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation.IsMatched(
                                        "0123Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation.IsNotMatched(
                                        "0123Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation.IsNotMatched(
                                        "01234Def789"));
                        }

                        var stringMatcherWithExpression_StringCalculation_Or = new StringMatcher("[Abc || def]");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "ABC"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "Def"));
                        }

                        stringMatcherWithExpression_StringCalculation_Or = new StringMatcher("0123[Abc || def]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "0123Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "0123Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "01234Def789"));
                        }

                        stringMatcherWithExpression_StringCalculation_Or = new StringMatcher("[Abc || def]789");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "01234Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Abc"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Def"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Abc456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Def456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "123456789"));
                        }

                        stringMatcherWithExpression_StringCalculation_Or = new StringMatcher("0123[Abc || def]");
                        {
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123456789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Abc789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "0123Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "01234Def789"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Abc"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "Def"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "0123Abc"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsMatched(
                                        "0123Def"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "01234Abc"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "01234Def"));
                                Assert.IsTrue(stringMatcherWithExpression_StringCalculation_Or.IsNotMatched(
                                        "123456789"));
                        }
                }
        }
}
