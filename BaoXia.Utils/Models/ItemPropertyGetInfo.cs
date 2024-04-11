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
	(ItemPropertyGetInfoType type,
	ItemType hostItem,
	PropertyInfo? propertyInfo,
	object? objectProperty)
{
	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	public ItemPropertyGetInfoType Type { get; set; } = type;

	public ItemType HosttItem { get; set; } = hostItem;

	public PropertyInfo? PropertyInfo { get; set; } = propertyInfo;

	public object? ObjectProperty { get; set; } = objectProperty;

	#endregion



	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public object? GetPropertyOfHostItem() 
	{
		switch (Type)
		{
			default:
			case ItemPropertyGetInfoType.Unknow:
			case ItemPropertyGetInfoType.ValueItemInIEnumerable:
				break;
			case ItemPropertyGetInfoType.NormalProperty:
				{
					if (PropertyInfo != null)
					{
						return PropertyInfo.GetValue(HosttItem);
					}
				} break;
			case ItemPropertyGetInfoType.ObjectItemInIEnumerable:
				{
					if (ObjectProperty != null)
					{
						return ObjectProperty;
					}
				} break;
		}
		return null;
	}

	#endregion
}