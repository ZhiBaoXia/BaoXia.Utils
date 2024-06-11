using BaoXia.Utils.Test.CacheTest;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.Models
{
	public class CacheListItem<ListIdType>
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		public int Id { get; set; }

		public ListIdType? ListId { get; set; }

		public string? Title { get; set; }


		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////


		protected static int GlobalListItemId;

		public static int NextListItemId()
		{
			return Interlocked.Increment(ref GlobalListItemId);
		}

		public static async Task<CacheListItem<ListIdType>[]> CreateItemListAsync(ListIdType listId)
		{
			return await Task.Run(
				() =>
				{
					List<CacheListItem<ListIdType>> list = new();
					for (var listItemIndex = 0;
					listItemIndex < CacheTestConfig.TestListItemsCount;
					listItemIndex++)
					{
						var listItem = new CacheListItem<ListIdType>()
						{
							Id = CacheListItem<ListIdType>.NextListItemId(),
							ListId = listId
						};
						list.Add(listItem);
					}
					return list.ToArray();
				});
		}
	}
}
