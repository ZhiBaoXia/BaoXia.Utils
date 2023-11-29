using BaoXia.Utils.Test.CacheTest;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test.Models
{
	public class CacheItem<IdType>
	{
		////////////////////////////////////////////////
		// @测试配置参数
		////////////////////////////////////////////////

		static readonly CacheTestConfig Config = new();

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		public IdType? Id { get; set; }

		public int ListId { get; set; }

		public string? Title { get; set; }


		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		public static async Task<CacheItem<IdType>> CreateItemAsync(IdType itemId)
		{
			return await Task<CacheItem<IdType>>.Run(
					 () =>
					 {
						 CacheItem<IdType> item = new()
						 {
							 Id = itemId
						 };
						 return item;
					 });
		}
	}
}
