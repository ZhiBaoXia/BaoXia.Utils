using BaoXia.Utils.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Models;

public class ItemPropertyGetInfo<ItemType>
	(ItemPropertyRelation propertyRelation,
	ItemType hostItem,
	PropertyInfo? propertyInfo,
	object? objectProperty,
	int objectProperty_Index,
	object? objectProperty_Key)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ItemPropertyRelation PropertyRelation { get; set; } = propertyRelation;

	public ItemType HosttItem { get; set; } = hostItem;

	public PropertyInfo? PropertyInfo { get; set; } = propertyInfo;


	////////////////////////////////////////////////

	public object? ItemInCollection { get; set; } = objectProperty;

	public int ItemInCollection_Index { get; set; } = objectProperty_Index;

	public object? ItemInCollection_Key { get; set; } = objectProperty_Key;

	////////////////////////////////////////////////


	public int PropertyValueClonedIndex { get; set; }

	public object? PropertyValueClonedKey { get; set; }

	public object? PropertyValueCloned { get; set; }

	#endregion



	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public object? GetPropertyValue()
	{
		switch (PropertyRelation)
		{
			default:
			case ItemPropertyRelation.Unknow:
			case ItemPropertyRelation.ValueItemInIEnumerable:
				break;
			case ItemPropertyRelation.Property:
				{
					if (PropertyInfo != null)
					{
						return PropertyInfo.GetValue(HosttItem);
					}
				}
				break;
			case ItemPropertyRelation.ObjectItemInIEnumerable:
				{
					if (ItemInCollection != null)
					{
						return ItemInCollection;
					}
				}
				break;
		}
		return null;
	}

	#endregion
}