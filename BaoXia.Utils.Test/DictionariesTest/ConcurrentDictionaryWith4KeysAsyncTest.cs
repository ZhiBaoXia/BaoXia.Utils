using BaoXia.Utils.Dictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.DictionariesTest;

[TestClass]
public class ConcurrentDictionaryWith4KeysAsyncTest
{
	[TestMethod]
	public async Task AddGetRemoveAsyncTest()
	{
		var dictionary = new ConcurrentDictionaryWith4KeysAsync<int, string, int, string, string>();
		var testObjectValue = DateTime.Now.ToString();


		// AddTest:
		var itemAdded
			= await dictionary.AddAsync(
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
			var itemRemoved
				= await dictionary
				.RemoveAsync(
				0,
				"1",
				2,
				"3");
			Assert.AreEqual(
				testObjectValue,
				itemRemoved);
		}

		// GetOrAddTest:
		{
			var itemGot
				= await dictionary.GetOrAddAsync(
					0,
					"1",
					2,
					"3",
					testObjectValue);
			Assert.AreEqual(
				testObjectValue,
				itemGot);
		}

		// TryRemoveTest:
		{
			var itemRemoved
				= await
				dictionary
				.TryRemoveAsync(
					0,
					"1",
					2,
					"3");
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
		var dictionary = new ConcurrentDictionaryWith4Keys<double, string, double, string, string>();
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
