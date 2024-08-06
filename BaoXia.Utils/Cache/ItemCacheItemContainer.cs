using System;
namespace BaoXia.Utils.Cache
{
	public class ItemCacheItemContainer<ItemKeyType, ItemType, ItemCacheCreateParamType>(
		ItemKeyType key,
		ItemType? item,
		Boolean isItemValid,
		ItemCacheCreateParamType? itemCreateParam,
		DateTime lastReadTime,
		DateTime lastUpdateTime)
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public ItemKeyType Key { get; set; } = key;

		public ItemType? _item = item;
		public ItemType? Item
		{
			get
			{
				return _item;
			}
		}

		public bool IsItemValid { get; set; } = isItemValid;

		public Object ItemLocker
		{
			get
			{
				return this;
			}
		}

		public ItemCacheCreateParamType? ItemCreateParam { get; set; } = itemCreateParam;

		public DateTime LastReadTime { get; set; } = lastReadTime;

		public DateTime LastUpdateTime { get; set; } = lastUpdateTime;

		#endregion

		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public ItemCacheItemContainer(
			ItemKeyType key,
			ItemCacheCreateParamType? itemCreateParam)
			: this(key,
				  default,
				  false,
				  itemCreateParam,
				  DateTime.MinValue,
				  DateTime.MinValue)
		{ }

		public void SetItem(
			ItemType? item,
			ItemCacheCreateParamType? itemCreateParam,
			bool isNeedUpdateItemLastReadTime)
		{
			lock (this)
			{
				_item = item;
				this.IsItemValid = true;
				var now = DateTime.Now;
				this.LastUpdateTime = now;
				if (isNeedUpdateItemLastReadTime)
				{
					this.LastReadTime = now;
				}
				//
				this.DidSetItem(item, itemCreateParam);
				//
			}
		}

		#endregion


		////////////////////////////////////////////////
		// @事件节点
		////////////////////////////////////////////////

		#region 事件节点

		protected virtual void DidSetItem(
			ItemType? item,
			ItemCacheCreateParamType? itemCreateParam)
		{ }

		#endregion
	}
}