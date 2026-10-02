using BaoXia.Utils.Dictionaries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.DictionariesTest;

[TestClass]
public class ConcurrentDictionaryWithValueTypeTest
{
	[TestMethod]
	public void ConcurrentDictionaryWith2KeysTest()
	{
		var dictionary = new ConcurrentDictionaryWith2Keys<int, int, int>();
		Assert.IsFalse(dictionary.TryGet(0, 0, out _));

		var itemsCreatedCount = 0;
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, (_, _) =>
		{
			itemsCreatedCount++;
			return 0;
		}));
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, (_, _) =>
		{
			itemsCreatedCount++;
			return 1;
		}));
		Assert.AreEqual(1, itemsCreatedCount);
		Assert.IsTrue(dictionary.TryGet(0, 0, out var item));
		Assert.AreEqual(0, item);
		Assert.IsTrue(dictionary.TryRemove(0, 0, out var itemRemoved));
		Assert.AreEqual(0, itemRemoved);
		Assert.IsFalse(dictionary.TryRemove(0, 0, out _));

		dictionary.Add(0, 0, 0);
		dictionary.Add(0, 1, 0);
		dictionary.Add(1, 0, 0);
		dictionary.Clear(0);
		Assert.AreEqual(1, dictionary.GetCount());
		dictionary.Clear();
		Assert.AreEqual(0, dictionary.GetCount());
	}

	[TestMethod]
	public void ConcurrentDictionaryWith3KeysTest()
	{
		var dictionary = new ConcurrentDictionaryWith3Keys<int, int, int, int>();
		Assert.IsFalse(dictionary.TryGet(0, 0, 0, out _));

		var itemsCreatedCount = 0;
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, (_, _, _) =>
		{
			itemsCreatedCount++;
			return 0;
		}));
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, (_, _, _) =>
		{
			itemsCreatedCount++;
			return 1;
		}));
		Assert.AreEqual(1, itemsCreatedCount);
		Assert.IsTrue(dictionary.TryGet(0, 0, 0, out var item));
		Assert.AreEqual(0, item);
		Assert.IsTrue(dictionary.TryRemove(0, 0, 0, out var itemRemoved));
		Assert.AreEqual(0, itemRemoved);
		Assert.IsFalse(dictionary.TryRemove(0, 0, 0, out _));

		dictionary.Add(0, 0, 0, 0);
		dictionary.Add(0, 0, 1, 0);
		dictionary.Add(0, 1, 0, 0);
		dictionary.Add(1, 0, 0, 0);
		dictionary.Clear(0, 0);
		Assert.AreEqual(2, dictionary.GetCount());
		dictionary.Clear(0);
		Assert.AreEqual(1, dictionary.GetCount());
		dictionary.Clear();
		Assert.AreEqual(0, dictionary.GetCount());
	}

	[TestMethod]
	public void ConcurrentDictionaryWith4KeysTest()
	{
		var dictionary = new ConcurrentDictionaryWith4Keys<int, int, int, int, int>();
		Assert.IsFalse(dictionary.TryGet(0, 0, 0, 0, out _));

		var itemsCreatedCount = 0;
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, (_, _, _, _) =>
		{
			itemsCreatedCount++;
			return 0;
		}));
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, (_, _, _, _) =>
		{
			itemsCreatedCount++;
			return 1;
		}));
		Assert.AreEqual(1, itemsCreatedCount);
		Assert.IsTrue(dictionary.TryGet(0, 0, 0, 0, out var item));
		Assert.AreEqual(0, item);
		Assert.IsTrue(dictionary.TryRemove(0, 0, 0, 0, out var itemRemoved));
		Assert.AreEqual(0, itemRemoved);
		Assert.IsFalse(dictionary.TryRemove(0, 0, 0, 0, out _));

		dictionary.Add(0, 0, 0, 0, 0);
		dictionary.Add(0, 0, 0, 1, 0);
		dictionary.Add(0, 0, 1, 0, 0);
		dictionary.Add(0, 1, 0, 0, 0);
		dictionary.Add(1, 0, 0, 0, 0);
		dictionary.Clear(0, 0, 0);
		Assert.AreEqual(3, dictionary.GetCount());
		dictionary.Clear(0, 0);
		Assert.AreEqual(2, dictionary.GetCount());
		dictionary.Clear(0);
		Assert.AreEqual(1, dictionary.GetCount());
		dictionary.Clear();
		Assert.AreEqual(0, dictionary.GetCount());
	}

	[TestMethod]
	public void ConcurrentDictionaryWith5KeysTest()
	{
		var dictionary = new ConcurrentDictionaryWith5Keys<int, int, int, int, int, int>();
		Assert.IsFalse(dictionary.TryGet(0, 0, 0, 0, 0, out _));

		var itemsCreatedCount = 0;
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, 0, (_, _, _, _, _) =>
		{
			itemsCreatedCount++;
			return 0;
		}));
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, 0, (_, _, _, _, _) =>
		{
			itemsCreatedCount++;
			return 1;
		}));
		Assert.AreEqual(1, itemsCreatedCount);
		Assert.IsTrue(dictionary.TryGet(0, 0, 0, 0, 0, out var item));
		Assert.AreEqual(0, item);
		Assert.IsTrue(dictionary.TryRemove(0, 0, 0, 0, 0, out var itemRemoved));
		Assert.AreEqual(0, itemRemoved);
		Assert.IsFalse(dictionary.TryRemove(0, 0, 0, 0, 0, out _));

		dictionary.Add(0, 0, 0, 0, 0, 0);
		dictionary.Add(0, 0, 0, 0, 1, 0);
		dictionary.Add(0, 0, 0, 1, 0, 0);
		dictionary.Add(0, 0, 1, 0, 0, 0);
		dictionary.Add(0, 1, 0, 0, 0, 0);
		dictionary.Add(1, 0, 0, 0, 0, 0);
		dictionary.Clear(0, 0, 0, 0);
		Assert.AreEqual(4, dictionary.GetCount());
		dictionary.Clear(0, 0, 0);
		Assert.AreEqual(3, dictionary.GetCount());
		dictionary.Clear(0, 0);
		Assert.AreEqual(2, dictionary.GetCount());
		dictionary.Clear(0);
		Assert.AreEqual(1, dictionary.GetCount());
		dictionary.Clear();
		Assert.AreEqual(0, dictionary.GetCount());
	}

	[TestMethod]
	public void ConcurrentDictionaryWith6KeysTest()
	{
		var dictionary = new ConcurrentDictionaryWith6Keys<int, int, int, int, int, int, int>();
		Assert.IsFalse(dictionary.TryGet(0, 0, 0, 0, 0, 0, out _));

		var itemsCreatedCount = 0;
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, 0, 0, (_, _, _, _, _, _) =>
		{
			itemsCreatedCount++;
			return 0;
		}));
		Assert.AreEqual(0, dictionary.GetOrAdd(0, 0, 0, 0, 0, 0, (_, _, _, _, _, _) =>
		{
			itemsCreatedCount++;
			return 1;
		}));
		Assert.AreEqual(1, itemsCreatedCount);
		Assert.IsTrue(dictionary.TryGet(0, 0, 0, 0, 0, 0, out var item));
		Assert.AreEqual(0, item);
		Assert.IsTrue(dictionary.TryRemove(0, 0, 0, 0, 0, 0, out var itemRemoved));
		Assert.AreEqual(0, itemRemoved);
		Assert.IsFalse(dictionary.TryRemove(0, 0, 0, 0, 0, 0, out _));

		dictionary.Add(0, 0, 0, 0, 0, 0, 0);
		dictionary.Add(0, 0, 0, 0, 0, 1, 0);
		dictionary.Add(0, 0, 0, 0, 1, 0, 0);
		dictionary.Add(0, 0, 0, 1, 0, 0, 0);
		dictionary.Add(0, 0, 1, 0, 0, 0, 0);
		dictionary.Add(0, 1, 0, 0, 0, 0, 0);
		dictionary.Add(1, 0, 0, 0, 0, 0, 0);
		dictionary.Clear(0, 0, 0, 0, 0);
		Assert.AreEqual(5, dictionary.GetCount());
		dictionary.Clear(0, 0, 0, 0);
		Assert.AreEqual(4, dictionary.GetCount());
		dictionary.Clear(0, 0, 0);
		Assert.AreEqual(3, dictionary.GetCount());
		dictionary.Clear(0, 0);
		Assert.AreEqual(2, dictionary.GetCount());
		dictionary.Clear(0);
		Assert.AreEqual(1, dictionary.GetCount());
		dictionary.Clear();
		Assert.AreEqual(0, dictionary.GetCount());
	}
}
