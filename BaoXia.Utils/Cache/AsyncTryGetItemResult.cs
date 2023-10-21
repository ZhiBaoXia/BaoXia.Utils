namespace BaoXia.Utils.Cache
{
        public class AsyncTryGetItemResult<ItemKeyType, ItemType, ItemCacheCreateParamType>
                        where ItemKeyType : notnull
        {
                public bool IsGotSucess { get; set; }


                public AsyncItemCacheItemContainer<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? itemContainer;

                public ItemType? Item { get; set; }
        }
}
