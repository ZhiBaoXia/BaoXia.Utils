using BaoXia.Utils.Dictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.DictionariesTest;

[TestClass]
public class ConcurrentDictionaryWith4KeysTest
{
	[TestMethod]
	public void AddGetRemoveTest()
	{
		var dictionary = new ConcurrentDictionaryWith4Keys<string, int, string, int, string>();
		var testObjectValue = DateTime.Now.ToString();


		// AddTest:
		var itemAdded
			= dictionary.Add(
				0,
				"1",
				2,
				"3",
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
					out var itemTryGot));


			Assert.AreEqual(
				testObjectValue,
				itemTryGot);


			var itemGot
				= dictionary.Get(
					0,
					"1",
					2,
					"3");
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


	[TestMethod]
	public void ClearTest()
	{
		var dictionary = new ConcurrentDictionaryWith4Keys<string, double, string, double, string>();
		{
			dictionary.Add(
					0,
					"1",
					2.1,
					"3.1",
					"Value.01");
			dictionary.Add(
					0,
					"1",
					2.1,
					"3.2",
					"Value.02");


			dictionary.Add(
					0,
					"1",
					2.2,
					"3.1",
					"Value.01");
			dictionary.Add(
					0,
					"1",
					2.2,
					"3.2",
					"Value.02");
		}
		//
		Assert.AreEqual(4, dictionary.GetCount());
		//
		{
			dictionary.Clear(
					0,
					"1",
					2.1);
		}
		//
		Assert.AreEqual(2, dictionary.GetCount());
		//
		{
			dictionary.Clear(
					0,
					"1",
					2.2);
		}
		//
		Assert.AreEqual(0, dictionary.GetCount());
		//
	}
}
