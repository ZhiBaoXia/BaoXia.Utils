using System;
using System.Threading.Tasks;

namespace BaoXia.Utils.Cache
{
        public class AsyncItemCacheItemContainer<ItemKeyType, ItemType, ItemCacheCreateParamType>
                : ItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>
                        where ItemKeyType : notnull
        {

                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public Task<Task<ItemType?>>? ItemCacheCreateTask { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现


                public AsyncItemCacheItemContainer(
                        ItemKeyType key,
                        ItemType? item,
                        Boolean isItemValid,
                        ItemCacheCreateParamType? itemCreateParam,
                        DateTime lastReadTime,
                        DateTime lastUpdateTime,
                        Task<Task<ItemType?>>? itemCacheCreateTask)
                        : base(key,
                          item,
                          isItemValid,
                          itemCreateParam,
                          lastReadTime,
                          lastUpdateTime)
                {
                        this.ItemCacheCreateTask = itemCacheCreateTask;
                }


                #endregion

                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                #region 事件节点

                protected override void DidSetItem(
                        ItemType? item,
                        ItemCacheCreateParamType? itemCreateParam)
                {
                        base.DidSetItem(item, itemCreateParam);

                        this.ItemCacheCreateTask = null;

                }

                #endregion
        }
}
