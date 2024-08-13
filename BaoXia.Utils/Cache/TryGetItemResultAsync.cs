namespace BaoXia.Utils.Cache;

public class TryGetItemResultAsync<ItemKeyType, ItemType, ItemCacheCreateParamType>
		where ItemKeyType : notnull
{
	public bool IsGotSucess { get; set; }


	public ItemCacheItemContainerAsync<ItemKeyType, ItemType?, ItemCacheCreateParamType?>? itemContainer;

	public ItemType? Item { get; set; }
}
