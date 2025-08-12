using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class StringExtensionTest
{
	[TestMethod]
	public void StringWithStringsTest()
	{
		var strings = new string[]
		{
			"00",
			"11",
			"22",
			"33",
			"44",
			"55"
		};
		var stringAfterJoin = StringUtil.StringWithStrings(
			strings,
			",",
			true);
		{
			Assert.AreEqual("00,11,22,33,44,55", stringAfterJoin);
		}
	}

	[TestMethod]
	public void SubstringBeforeTest()
	{
		var testString = "0123Abc456Def";
		var stringFound = testString.SubstringBefore("0123");
		{
			Assert.IsTrue(string.IsNullOrEmpty(stringFound));
		}
		stringFound = testString.SubstringBefore("Abc");
		{
			Assert.IsTrue(stringFound!.Equals("0123"));
		}
		stringFound = testString.SubstringBefore("456");
		{
			Assert.IsTrue(stringFound!.Equals("0123Abc"));
		}
		stringFound = testString.SubstringBefore("Def");
		{
			Assert.IsTrue(stringFound!.Equals("0123Abc456"));
		}
	}

	[TestMethod]
	public void SubstringAfterTest()
	{
		var testString = "0123Abc456Def";
		var stringFound = testString.SubstringAfter("0123");
		{
			Assert.IsTrue(stringFound!.Equals("Abc456Def"));
		}
		stringFound = testString.SubstringAfter("Abc");
		{
			Assert.IsTrue(stringFound!.Equals("456Def"));
		}
		stringFound = testString.SubstringAfter("456");
		{
			Assert.IsTrue(stringFound!.Equals("Def"));
		}
		stringFound = testString.SubstringAfter("Def");
		{
			Assert.IsTrue(string.IsNullOrEmpty(stringFound));
		}
	}

	[TestMethod]
	public void SubStringBetweenTest()
	{
		string? keyWords;
		const string kKeyWordsA_0 = "Key Words";
		var testStringA_0 = "left words {" + kKeyWordsA_0 + "} right words";
		{
			keyWords = testStringA_0.SubstringBetween("{", "}");
		}
		Assert.IsTrue(keyWords!.Equals(kKeyWordsA_0));

		const string kKeyWordsA_1 = "";
		var testStringA_1 = "left words {" + kKeyWordsA_1 + "} right words";
		{
			keyWords = testStringA_1.SubstringBetween("{", "}");
		}
		Assert.IsTrue(StringUtil.EqualsStrings(keyWords, kKeyWordsA_1));



		const string kKeyWordsB_0 = "{Key Words}";
		var testStringB_0 = "left words {" + kKeyWordsB_0 + "} right words";
		{
			keyWords = testStringB_0.SubstringBetween("{", "}", true);
		}
		Assert.IsTrue(keyWords!.Equals(kKeyWordsB_0));


		const string kKeyWordsB_1 = "{}";
		var testStringB_1 = "left words {" + kKeyWordsB_1 + "} right words";
		{
			keyWords = testStringB_1.SubstringBetween("{", "}", true);
		}
		Assert.IsTrue(keyWords!.Equals(kKeyWordsB_1));
	}


	[TestMethod]
	public void StringByUriAppendRelativePathTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var testRelativePath = "/search/index";
		var testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/";
		testRelativePath = "/search/index";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com/global";
		testRelativePath = "/search/index";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/global/";
		testRelativePath = "/search/index";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com/global";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/global/";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com/global";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/global/";
		testRelativePath = "/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index?searchKey=Abc&pageIndex=0&pageSize=20#type=book";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com";
		testRelativePath = "/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/";
		testRelativePath = "/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com/global";
		testRelativePath = "/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		testUri = "https://www.baoxiaruanjian.com/global/";
		testRelativePath = "/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		testUriAfterAppended = "https://www.baoxiaruanjian.com/global/search/index#type=book?searchKey=Abc&pageIndex=0&pageSize=20";
		Assert.AreEqual(
testUriAfterAppended, testUri.StringByUriAppendRelativePath(testRelativePath)
);

		////////////////////////////////////////////////
	}

	[TestMethod]
	public void StringByUriAppendQueryParamsTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var testUriQueryParams = "searchKey=key&searchType=type";
		var testUriWithQueryParams = testUri + "?" + testUriQueryParams;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, (testUri + "?").StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("?" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com?searchKey=key";
		testUriQueryParams = "searchType=type";
		testUriWithQueryParams = testUri + "&" + testUriQueryParams;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, (testUri + "&").StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com#pageAnchorPoint=bottom";
		testUriQueryParams = "searchType=type";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchType=type#pageAnchorPoint=bottom"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com?searchKey=key#pageAnchorPoint=bottom";
		testUriQueryParams = "searchType=type";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchKey=key&searchType=type#pageAnchorPoint=bottom"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com";
		testUriQueryParams = "searchType=type#pageRef=home";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchType=type#pageRef=home"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com#pageAnchorPoint=bottom";
		testUriQueryParams = "searchType=type#pageRef=home";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchType=type#pageAnchorPoint=bottom&pageRef=home"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);




		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com?searchKey=Abc";
		testUriQueryParams = "searchType=type#pageRef=home";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchKey=Abc&searchType=type#pageRef=home"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);


		////////////////////////////////////////////////


		testUri = "https://www.baoxiaruanjian.com?searchKey=Abc#pageAnchorPoint=bottom";
		testUriQueryParams = "searchType=type#pageRef=home";
		testUriWithQueryParams = "https://www.baoxiaruanjian.com?searchKey=Abc&searchType=type#pageAnchorPoint=bottom&pageRef=home"; ;

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams(testUriQueryParams)
);

		Assert.AreEqual(
testUriWithQueryParams, testUri.StringByUriAppendQueryParams("&" + testUriQueryParams)
);
	}

	[TestMethod]
	public void StringByUriAppendQueryParamTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var objectTestUri = "https://www.baoxiaruanjian.com?searchKey=key&searchType=type";
		{
			testUri = testUri.StringByUriAppendQueryParam(
				"searchKey",
				"key");
			testUri = testUri.StringByUriAppendQueryParam(
				"searchType",
				"type");
		}
		Assert.AreEqual(
			objectTestUri,
			testUri);


		testUri = "https://www.baoxiaruanjian.com";
		objectTestUri = "https://www.baoxiaruanjian.com?searchKey=%E4%B8%80%E4%BA%8C%E4%B8%89&searchType=type";
		{
			testUri = testUri.StringByUriAppendQueryParam(
				"searchKey",
				"一二三");
			testUri = testUri.StringByUriAppendQueryParam(
				"searchType",
				"type");
		}
		Assert.AreEqual(
			objectTestUri,
			testUri);


		testUri = "https://www.baoxiaruanjian.com?pageIndex=0#anchor=1";
		objectTestUri = "https://www.baoxiaruanjian.com?pageIndex=0&searchKey=%E4%B8%80%E4%BA%8C%E4%B8%89&searchType=type#anchor=1";
		{
			testUri = testUri.StringByUriAppendQueryParam(
				"searchKey",
				"一二三");
			testUri = testUri.StringByUriAppendQueryParam(
				"searchType",
				"type");
		}
		Assert.AreEqual(
			objectTestUri,
			testUri);
	}

	[TestMethod]
	public void ToFileSystemDirectoryPathTest()
	{
		var orignialDirectoryPath = "d:\\work\\webroot";
		var directoryPath = orignialDirectoryPath.ToFileSystemDirectoryPath();
		{
			//
			Assert.IsTrue(directoryPath.Equals(
				orignialDirectoryPath
				+ System.IO.Path.DirectorySeparatorChar));
			//
		}

		var fileName = "index.html";
		var filePath = directoryPath + fileName;
		var directoryPathFromFilePath = filePath.ToFileSystemDirectoryPath(true);
		{
			//
			Assert.IsTrue(directoryPathFromFilePath.Equals(directoryPath));
			//
		}
	}

	[TestMethod]
	public void ToFileNameTest()
	{
		var fileName = "steam.exe";
		var filePath = "C:\\Program Files (x86)\\Steam"
			+ System.IO.Path.DirectorySeparatorChar
			+ fileName;
		{
			Assert.IsTrue(filePath.ToFileName()!.Equals(fileName));
		}

		filePath = "C:\\Program Files (x86)\\Steam"
			+ System.IO.Path.DirectorySeparatorChar
			+ fileName + "?gameId=1001";
		{
			Assert.IsTrue(filePath.ToFileName()!.Equals(fileName));
		}

		filePath = "C:\\Program Files (x86)\\Steam"
			+ System.IO.Path.DirectorySeparatorChar
			+ fileName + "#mission=1";
		{
			Assert.IsTrue(filePath.ToFileName()!.Equals(fileName));
		}

		filePath = "C:\\Program Files (x86)\\Steam"
			+ System.IO.Path.DirectorySeparatorChar
			+ fileName + "?gameId=1001#mission=1";
		{
			Assert.IsTrue(filePath.ToFileName()!.Equals(fileName));
		}
	}

	[TestMethod]
	public async Task GetItemsContainedAsyncTest()
	{
		var testString = "000111222333444555";
		var testItems = new string[]
		{
			"111",
			"333",
			"555",
			"aaa"
		};
		var testItemsContained
			= await testString.GetItemsContainedAsync(
			testItems,
			(str, testItem) =>
			{
				if (str.Contains(testItem, StringComparison.OrdinalIgnoreCase))
				{
					return testItem;
				}
				return null;
			});
		// !!!
		Assert.IsNotNull(testItemsContained);
		Assert.AreEqual(3, testItemsContained.Count);
		Assert.IsTrue(testItemsContained.Contains("111"));
		Assert.IsTrue(testItemsContained.Contains("333"));
		Assert.IsTrue(testItemsContained.Contains("555"));
		// !!!
	}


	[TestMethod]
	public void SplitWithOptionalSeparatorsIgnoreCaseTest()
	{
		var testString = "123@456#7@89";
		var testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(3, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals("123"));
			Assert.IsTrue(testStrings[1].Equals("456#7"));
			Assert.IsTrue(testStrings[2].Equals("89"));
		}

		testString = "123@456#7@89@";
		testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(4, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals("123"));
			Assert.IsTrue(testStrings[1].Equals("456#7"));
			Assert.IsTrue(testStrings[2].Equals("89"));
			Assert.IsTrue(testStrings[3].Equals(string.Empty));
		}

		testString = "@123@456#7@89@";
		testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(5, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals(string.Empty));
			Assert.IsTrue(testStrings[1].Equals("123"));
			Assert.IsTrue(testStrings[2].Equals("456#7"));
			Assert.IsTrue(testStrings[3].Equals("89"));
			Assert.IsTrue(testStrings[4].Equals(string.Empty));
		}

		testString = "123#456#7@89";
		testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(3, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals("123"));
			Assert.IsTrue(testStrings[1].Equals("456"));
			Assert.IsTrue(testStrings[2].Equals("7@89"));
		}

		testString = "123#456#7@89#";
		testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(4, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals("123"));
			Assert.IsTrue(testStrings[1].Equals("456"));
			Assert.IsTrue(testStrings[2].Equals("7@89"));
			Assert.IsTrue(testStrings[3].Equals(string.Empty));
		}


		testString = "#123#456#7@89#";
		testStrings = testString.SplitWithOptionalSeparatorsIgnoreCase('@', '#');
		{
			Assert.AreEqual(5, testStrings.Count);
			Assert.IsTrue(testStrings[0].Equals(string.Empty));
			Assert.IsTrue(testStrings[1].Equals("123"));
			Assert.IsTrue(testStrings[2].Equals("456"));
			Assert.IsTrue(testStrings[3].Equals("7@89"));
			Assert.IsTrue(testStrings[4].Equals(string.Empty));
		}
	}

	[TestMethod]
	public void ToAbsoluteFilePathInRootPath()
	{
		string testRootPath = AppContext.BaseDirectory;

		string testFilPath_0 = "ConfigFiles";
		string testFilPath_0_AbsoluteFilePath = AppContext.BaseDirectory?.ToFileSystemDirectoryPath() + testFilPath_0;
		{
			var testResult
				= testFilPath_0
				.ToAbsoluteFilePathInRootPath(testRootPath)
				.Equals(testFilPath_0_AbsoluteFilePath);
			// !!!
			Assert.IsTrue(testResult);
			// !!!
		}

		string testFilPath_1 = "/ConfigFiles";
		string testFilPath_1_AbsoluteFilePath = testFilPath_1;
		{
			var testResult
				= testFilPath_1
				.ToAbsoluteFilePathInRootPath(testRootPath)
				.Equals(testFilPath_1_AbsoluteFilePath);
			// !!!
			Assert.IsTrue(testResult);
			// !!!
		}

		string testFilPath_2 = string.Empty;
		string testFilPath_2_AbsoluteFilePath = testRootPath;
		{
			var testResult
				= testFilPath_2.ToAbsoluteFilePathInRootPath(testRootPath)
				== testFilPath_2_AbsoluteFilePath;
			// !!!
			Assert.IsTrue(testResult);
			// !!!
		}
	}

	[TestMethod]
	public void ToFileExtensionName()
	{
		var testFilePath = "d:\\work\\test\\file.exname0.exname1";

		if (testFilePath.ToFileExtensionName() != "exname1")
		{
			Assert.Fail();
		}
		if (testFilePath.ToFileExtensionName(true) != "exname0.exname1")
		{
			Assert.Fail();
		}

		testFilePath = "d:\\work\\test\\file.exname0.exname1?query=Abc";

		if (testFilePath.ToFileExtensionName() != "exname1")
		{
			Assert.Fail();
		}
		if (testFilePath.ToFileExtensionName(true) != "exname0.exname1")
		{
			Assert.Fail();
		}
	}


	[TestMethod]
	public void StringWithIntsTest()
	{
		var numbers = new int[] { 0, 1, 2, 3, 4, 5 };
		var numbersString = StringUtil.StringWithInts(numbers);
		{
			Assert.AreEqual("0,1,2,3,4,5", numbersString);
			var numberArray = numbersString.ToIntArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));

		}
		numbersString = StringUtil.StringWithInts(numbers, "|");
		{
			Assert.AreEqual("0|1|2|3|4|5", numbersString);
			var numberArray = numbersString.ToIntArray("|");
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithInts(numbers, null);
		{
			Assert.AreEqual("012345", numbersString);
		}
		numbersString = StringUtil.StringWithInts(numbers, ",", "D2");
		{
			Assert.AreEqual("00,01,02,03,04,05", numbersString);
			var numberArray = numbersString.ToIntArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
	}

	[TestMethod]
	public void StringWithLongsTest()
	{
		var numbers = new long[] { 0, 1, 2, 3, 4, 5 };
		var numbersString = StringUtil.StringWithLongs(numbers);
		{
			Assert.AreEqual("0,1,2,3,4,5", numbersString);
			var numberArray = numbersString.ToLongArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithLongs(numbers, "|");
		{
			Assert.AreEqual("0|1|2|3|4|5", numbersString);
			var numberArray = numbersString.ToLongArray("|");
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithLongs(numbers, null);
		{
			Assert.AreEqual("012345", numbersString);
		}
		numbersString = StringUtil.StringWithLongs(numbers, ",", "D2");
		{
			Assert.AreEqual("00,01,02,03,04,05", numbersString);
			var numberArray = numbersString.ToLongArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0, 1, 2];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
	}

	[TestMethod]
	public void StringWithFloatsTest()
	{
		var numbers = new float[] { 0.0F, 1.4F, 2.5F, 3.4F, 4.5F, 5.4F };
		var numbersString = StringUtil.StringWithFloats(numbers);
		{
			Assert.AreEqual("0,1.4,2.5,3.4,4.5,5.4", numbersString);
			var numberArray = numbersString.ToFloatArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0F, 1.4F, 2.5F];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithFloats(numbers, "|");
		{
			Assert.AreEqual("0|1.4|2.5|3.4|4.5|5.4", numbersString);
			var numberArray = numbersString.ToFloatArray("|");
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0F, 1.4F, 2.5F];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithFloats(numbers, null);
		{
			Assert.AreEqual("01.42.53.44.55.4", numbersString);
		}
		numbersString = StringUtil.StringWithFloats(numbers, ",", "F2");
		{
			Assert.AreEqual("0.00,1.40,2.50,3.40,4.50,5.40", numbersString);
			var numberArray = numbersString.ToFloatArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0F, 1.4F, 2.5F];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
	}


	[TestMethod]
	public void StringWithDoublesTest()
	{
		var numbers = new double[] { 0.0, 1.4, 2.5, 3.4, 4.5, 5.4 };
		var numbersString = StringUtil.StringWithDoubles(numbers);
		{
			Assert.AreEqual("0,1.4,2.5,3.4,4.5,5.4", numbersString);
			var numberArray = numbersString.ToDoubleArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0, 1.4, 2.5];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithDoubles(numbers, "|");
		{
			Assert.AreEqual("0|1.4|2.5|3.4|4.5|5.4", numbersString);
			var numberArray = numbersString.ToDoubleArray("|");
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0, 1.4, 2.5];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithDoubles(numbers, null);
		{
			Assert.AreEqual("01.42.53.44.55.4", numbersString);
		}
		numbersString = StringUtil.StringWithDoubles(numbers, ",", "F2");
		{
			Assert.AreEqual("0.00,1.40,2.50,3.40,4.50,5.40", numbersString);
			var numberArray = numbersString.ToDoubleArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0, 1.4, 2.5];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
	}


	[TestMethod]
	public void StringWithDecimalsTest()
	{
		var numbers = new decimal[] { 0.0M, 1.4M, 2.5M, 3.4M, 4.5M, 5.4M };
		var numbersString = StringUtil.StringWithDicemals(numbers);
		{
			Assert.AreEqual("0.0,1.4,2.5,3.4,4.5,5.4", numbersString);
			var numberArray = numbersString.ToDecimalArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0M, 1.4M, 2.5M];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithDicemals(numbers, "|");
		{
			Assert.AreEqual("0.0|1.4|2.5|3.4|4.5|5.4", numbersString);
			var numberArray = numbersString.ToDecimalArray("|");
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0M, 1.4M, 2.5M];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
		numbersString = StringUtil.StringWithDicemals(numbers, null);
		{
			Assert.AreEqual("0.01.42.53.44.55.4", numbersString);
		}
		numbersString = StringUtil.StringWithDicemals(numbers, ",", "F2");
		{
			Assert.AreEqual("0.00,1.40,2.50,3.40,4.50,5.40", numbersString);
			var numberArray = numbersString.ToDecimalArray();
			Assert.IsTrue(numbers.IsEquals(numberArray));
			numberArray = [0.0M, 1.4M, 2.5M];
			Assert.IsTrue(numbers.IsNotEquals(numberArray));
		}
	}

	[TestMethod]
	public void ToRandomString()
	{
		const int randomStringLength = 512;

		var randomString = StringUtil.StringByFillRandomCharsToLength(randomStringLength);
		{
			System.Diagnostics.Trace.WriteLine("StringExtensionTest.ToRandomString():\r\n" + randomString);
		}
		// !!!
		Assert.AreEqual(randomStringLength, randomString.Length);
		// !!!
	}

	class ToObjectModel(int id, string? name)
	{
		public int Id { get; set; } = id;

		public string? Name { get; set; } = name;
	}

	[TestMethod]
	public void GetSchemeInUriTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var scheme = testUri.GetSchemeInUri();
		{
			Assert.IsTrue(scheme!.Equals("https"));
		}

		testUri = "www.baoxiaruanjian.com:80";
		scheme = testUri.GetSchemeInUri();
		{
			Assert.IsTrue(string.IsNullOrEmpty(scheme));
		}
	}

	[TestMethod]
	public void GetHostInUriTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		testUri = "https://www.baoxiaruanjian.com:80";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com:80"));
		}
		testUri = "https://www.baoxiaruanjian.com:80";
		host = testUri.GetHostInUri();
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		testUri = "https://www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com:80"));
		}
		testUri = "https://www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri();
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "http://www.baoxiaruanjian.com?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		testUri = "http://www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com:80"));
		}
		testUri = "http://www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri();
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "www.baoxiaruanjian.com?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}

		testUri = "www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri(true);
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com:80"));
		}
		testUri = "www.baoxiaruanjian.com:80?searchKey=Abc#pageAnchorPoint=bottom";
		host = testUri.GetHostInUri();
		{
			Assert.IsTrue(host!.Equals("www.baoxiaruanjian.com"));
		}
	}

	[TestMethod]
	public void GetPathInUriTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(string.IsNullOrEmpty(path));
		}
		testUri = "https://www.baoxiaruanjian.com/search?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search?searchKey=Abc#pageAnchorPoint=bottom"));
		}
		testUri = "https://www.baoxiaruanjian.com/search/?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search/?searchKey=Abc#pageAnchorPoint=bottom"));
		}
		testUri = "https://www.baoxiaruanjian.com/search/news?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search/news?searchKey=Abc#pageAnchorPoint=bottom"));
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "www.baoxiaruanjian.com";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(string.IsNullOrEmpty(path));
		}
		testUri = "www.baoxiaruanjian.com/search?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search?searchKey=Abc#pageAnchorPoint=bottom"));
		}
		testUri = "www.baoxiaruanjian.com/search/?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search/?searchKey=Abc#pageAnchorPoint=bottom"));
		}
		testUri = "www.baoxiaruanjian.com/search/news?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri(true);
		{
			Assert.IsTrue(path!.Equals("/search/news?searchKey=Abc#pageAnchorPoint=bottom"));
		}

		////////////////////////////////////////////////
		////////////////////////////////////////////////
		////////////////////////////////////////////////

		testUri = "https://www.baoxiaruanjian.com/search?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri();
		{
			Assert.IsTrue(path!.Equals("/search"));
		}
		testUri = "https://www.baoxiaruanjian.com/search/?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri();
		{
			Assert.IsTrue(path!.Equals("/search/"));
		}
		testUri = "https://www.baoxiaruanjian.com/search/news?searchKey=Abc#pageAnchorPoint=bottom";
		path = testUri.GetPathInUri();
		{
			Assert.IsTrue(path!.Equals("/search/news"));
		}
	}

	[TestMethod]
	public void GetQueryInUriTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(string.IsNullOrEmpty(fragment));
		}

		testUri = "https://www.baoxiaruanjian.com?query=searchKey";
		fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}

		testUri = "https://www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey#anchor=news"));
		}

		testUri = "https://www.baoxiaruanjian.com?query=searchKey";
		fragment = testUri.GetQueryParamsInUri();
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}

		testUri = "https://www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetQueryParamsInUri();
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}


		////////////////////////////////////////////////

		testUri = "www.baoxiaruanjian.com";
		fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(string.IsNullOrEmpty(fragment));
		}

		testUri = "www.baoxiaruanjian.com?query=searchKey";
		fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}

		testUri = "www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetQueryParamsInUri(true);
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey#anchor=news"));
		}

		testUri = "www.baoxiaruanjian.com?query=searchKey";
		fragment = testUri.GetQueryParamsInUri();
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}

		testUri = "www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetQueryParamsInUri();
		{
			Assert.IsTrue(fragment!.Equals("query=searchKey"));
		}
	}


	[TestMethod]
	public void GetFragmentInUriTest()
	{
		var testUri = "https://www.baoxiaruanjian.com";
		var fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(string.IsNullOrEmpty(fragment));
		}

		testUri = "https://www.baoxiaruanjian.com#anchor=news";
		fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(fragment!.Equals("anchor=news"));
		}

		testUri = "https://www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(fragment!.Equals("anchor=news"));
		}

		////////////////////////////////////////////////

		testUri = "www.baoxiaruanjian.com";
		fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(string.IsNullOrEmpty(fragment));
		}

		testUri = "www.baoxiaruanjian.com#anchor=news";
		fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(fragment!.Equals("anchor=news"));
		}

		testUri = "www.baoxiaruanjian.com?query=searchKey#anchor=news";
		fragment = testUri.GetFragmentInUri();
		{
			Assert.IsTrue(fragment!.Equals("anchor=news"));
		}
	}


	[TestMethod]
	public void TryToObjectByJsonDeserializeTest()
	{
		var testObjectA = new ToObjectModel(1, "objectA");
		var testObjectAJson = testObjectA.ToJsonString();
		Assert.IsTrue(testObjectAJson.TryToObjectByJsonDeserialize<ToObjectModel>(
			out var testObjectB));
		Assert.IsNotNull(testObjectB);
		Assert.IsTrue(testObjectB.Id.Equals(testObjectA.Id));
		Assert.IsTrue(testObjectB.Name!.Equals(testObjectA.Name));
	}

	[TestMethod]
	public void ToPrivacyStringTest()
	{
		var plaintextString = "13812345678";
		var privacytextString = plaintextString.ToPrivacyString(
			5,
			StringPartType.Center,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("138*****678"));
		}
		privacytextString = plaintextString.ToPrivacyString(
			6,
			StringPartType.Center,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("13******678"));
		}
		privacytextString = plaintextString.ToPrivacyString(
			0,
			StringPartType.Center,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("***********"));
		}
		privacytextString = plaintextString.ToPrivacyString(
			11,
			StringPartType.Center,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("***********"));
		}
		privacytextString = plaintextString.ToPrivacyString(
			20,
			StringPartType.Center,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("***********"));
		}

		privacytextString = plaintextString.ToPrivacyString(
			4,
			StringPartType.Left,
			"*");
		{
			Assert.IsTrue(privacytextString.Equals("****2345678"));
		}
		privacytextString = plaintextString.ToPrivacyString(
			5,
			StringPartType.Right,
			"MAsk");
		{
			Assert.IsTrue(privacytextString.Equals("138123MAskM"));
		}

		privacytextString = plaintextString.ToPrivacyStringForPhoneNumber(null);
		{
			Assert.IsTrue(privacytextString.Equals("138****5678"));
		}

		privacytextString = "baoxiaruanjian".ToPrivacyStringForAccount(null);
		{
			Assert.IsTrue(privacytextString.Equals("b************n"));
		}

		privacytextString = "bx".ToPrivacyStringForAccount(null);
		{
			Assert.IsTrue(privacytextString.Equals("**"));
		}


		privacytextString = "130123456789012345".ToPrivacyStringForCNIdCardNumber(null);
		{
			Assert.IsTrue(privacytextString.Equals("130***********2345"));
		}
	}
}
