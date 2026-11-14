using BaoXia.Utils.Extensions;
using BaoXia.Utils.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ObjectExtensionCloneDeepTest
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	[TestMethod]
	public void CloneDeepWithEqualItemsOfDifferentTypesTest()
	{
		var item = new CloneDeepTestItem
		{
			Id = 1,
			Name = "Item"
		};
		var group = new CloneDeepTestGroup
		{
			Id = 1,
			Name = "Group",
			Items = [item]
		};
		Assert.AreEqual(group, item);

		var clonedGroup = group.Clone(true);

		Assert.AreNotSame(group, clonedGroup);
		Assert.AreEqual(group.Name, clonedGroup.Name);
		Assert.AreNotSame(group.Items, clonedGroup.Items);
		Assert.AreEqual(1, clonedGroup.Items.Count);
		Assert.AreEqual(typeof(CloneDeepTestItem), clonedGroup.Items[0].GetType());
		Assert.AreNotSame(item, clonedGroup.Items[0]);
		Assert.AreEqual(item.Id, clonedGroup.Items[0].Id);
		Assert.AreEqual(item.Name, clonedGroup.Items[0].Name);
	}

	[TestMethod]
	public void CloneDeepWithEqualItemsOfSameTypeTest()
	{
		var firstItem = new CloneDeepTestItem
		{
			Id = 1,
			Name = "First"
		};
		var secondItem = new CloneDeepTestItem
		{
			Id = 1,
			Name = "Second"
		};
		var group = new CloneDeepTestGroup
		{
			Id = 2,
			Items = [firstItem, secondItem]
		};
		Assert.AreEqual(firstItem, secondItem);

		var clonedGroup = group.CloneDeep();

		Assert.AreEqual(2, clonedGroup.Items.Count);
		Assert.AreNotSame(firstItem, clonedGroup.Items[0]);
		Assert.AreNotSame(secondItem, clonedGroup.Items[1]);
		Assert.AreNotSame(clonedGroup.Items[0], clonedGroup.Items[1]);
		Assert.AreEqual(firstItem.Id, clonedGroup.Items[0].Id);
		Assert.AreEqual(secondItem.Id, clonedGroup.Items[1].Id);
		Assert.AreEqual(firstItem.Name, clonedGroup.Items[0].Name);
		Assert.AreEqual(secondItem.Name, clonedGroup.Items[1].Name);

		clonedGroup.Items[0].Name = "Changed";
		Assert.AreEqual("First", firstItem.Name);
		Assert.AreEqual("Second", clonedGroup.Items[1].Name);
	}

	[TestMethod]
	public void CloneDeepPreservesSharedAndCircularReferencesTest()
	{
		var item = new CloneDeepTestItem
		{
			Id = 1,
			Name = "Shared"
		};
		var group = new CloneDeepTestGroup
		{
			Id = 2,
			Items = [item, item]
		};
		group.RelatedItem = group;
		item.RelatedItem = group;

		var clonedGroup = group.CloneDeep();

		Assert.AreNotSame(group, clonedGroup);
		Assert.AreEqual(2, clonedGroup.Items.Count);
		Assert.AreNotSame(item, clonedGroup.Items[0]);
		Assert.AreEqual(item.Id, clonedGroup.Items[0].Id);
		Assert.AreEqual(item.Name, clonedGroup.Items[0].Name);
		Assert.AreSame(clonedGroup.Items[0], clonedGroup.Items[1]);
		Assert.AreSame(clonedGroup, clonedGroup.RelatedItem);
		Assert.AreSame(clonedGroup, clonedGroup.Items[0].RelatedItem);
	}

	#endregion
}
