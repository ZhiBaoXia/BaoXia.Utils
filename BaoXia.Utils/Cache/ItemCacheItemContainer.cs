using System;
namespace BaoXia.Utils.Cache
{
        public class ItemCacheItemContainer<ItemKeyType, ItemType, ItemCacheCreateParamType>
        {

                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public ItemKeyType Key { get; set; }

                public ItemType? _item;
                public ItemType? Item
                {
                        get
                        {
                                return _item;
                        }
                }

                public bool IsItemValid { get; set; }

                public Object ItemLocker
                {
                        get
                        {
                                return this;
                        }
                }

                public ItemCacheCreateParamType? ItemCreateParam { get; set; }

                public DateTime LastReadTime { get; set; }

                public DateTime LastUpdateTime { get; set; }

                #endregion

                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public ItemCacheItemContainer(
                        ItemKeyType key,
                        ItemType? item,
                        Boolean isItemValid,
                        ItemCacheCreateParamType? itemCreateParam,
                        DateTime lastReadTime,
                        DateTime lastUpdateTime)
                {
                        this.Key = key;
                        _item = item;
                        this.IsItemValid = isItemValid;
                        this.ItemCreateParam = itemCreateParam;
                        this.LastReadTime = lastReadTime;
                        this.LastUpdateTime = lastUpdateTime;
                }

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