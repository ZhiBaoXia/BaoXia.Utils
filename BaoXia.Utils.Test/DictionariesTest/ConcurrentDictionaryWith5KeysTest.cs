using BaoXia.Utils.Dictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.DictionariesTest;

[TestClass]
public class ConcurrentDictionaryWith5KeysTest
{
	[TestMethod]
	public void AddGetRemoveTest()
	{
		var dictionary = new ConcurrentDictionaryWith5Keys<string, int, string, int, string, int>();
		var testObjectValue = DateTime.Now.ToString();


		// AddTest:
		var itemAdded
			= dictionary.Add(
				0,
				"1",
				2,
				"3",
				4,
				testObjectValue);
		{
			// !!!
			Assert.AreEqual(testObjectValue, itemAdded);
			// !!!
		}

		// GetTest:
		{
			Assert.IsTrue(
				dictionary.TryGet(
					0,
					"1",
					2,
					"3",
					4,
					out var itemTryGot));


			Assert.AreEqual(
				testObjectValue,
				itemTryGot);


			var itemGot
				= dictionary.Get(
					0,
					"1",
					2,
					"3",
					4);
			Assert.AreEqual(
				testObjectValue,
				itemGot);
		}

		// RemoveTest
		{
			dictionary
				.Remove(
				0,
				"1",
				2,
				"3",
				4,
				out var itemRemoved);
			Assert.AreEqual(
				testObjectValue,
				itemRemoved);
		}

		// GetOrAddTest:
		{
			Assert.AreEqual(
				testObjectValue,
				dictionary.GetOrAdd(
					0,
					"1",
					2,
					"3",
					4,
					testObjectValue));
		}

		// TryRemoveTest:
		{
			Assert.IsTrue(
				dictionary
				.TryRemove(
					0,
					"1",
					2,
					"3",
					4,
					out var itemRemoved));
			Assert.AreEqual(
				testObjectValue,
				itemRemoved);
		}

		// GetCountTest
		{
			Assert.AreEqual(
				0,
				dictionary.GetCount());
		}
	}
}
