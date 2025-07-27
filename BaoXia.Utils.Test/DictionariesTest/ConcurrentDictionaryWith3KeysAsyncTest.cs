using BaoXia.Utils.Dictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.DictionariesTest;

[TestClass]
public class ConcurrentDictionaryWith3KeysAsyncTest
{
	[TestMethod]
	public async Task AddGetRemoveAsyncTest()
	{
		var dictionary = new ConcurrentDictionaryWith3KeysAsync<string, int, string, int>();
		var testObjectValue = DateTime.Now.ToString();


		// AddTest:
		var itemAdded
			= await dictionary.AddAsync(
				0,
				"1",
				2,
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
					out var itemTryGot));


			Assert.AreEqual(
				testObjectValue,
				itemTryGot);


			var itemGot
				= dictionary.Get(
					0,
					"1",
					2);
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
				2);
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
					2);
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
