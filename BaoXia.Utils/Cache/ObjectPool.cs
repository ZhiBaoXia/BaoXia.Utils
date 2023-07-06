using System;
using System.Collections.Concurrent;

namespace BaoXia.Utils.Cache
{
	public class ObjectPool<ObjectType> where ObjectType : class
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected ConcurrentQueue<ObjectType> _objects = new();

		public Func<ObjectType> ToGetObject { get; set; }


		#endregion



		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public ObjectPool(Func<ObjectType> toGetObject)
		{
			this.ToGetObject = toGetObject;
		}

		public ObjectType GetObject()
		{
			if (!_objects.TryDequeue(out var objectItem)
				|| objectItem == null)
			{
				var toGetObject = this.ToGetObject;
				// !!!
				objectItem = toGetObject();
				// !!!
			}
			return objectItem;
		}

		public ObjectPoolItemContainer<ObjectType> GetObjectToUsing(
			out ObjectType? objectItem)
		{
			// !!!
			objectItem = this.GetObject();
			// !!!
			return new ObjectPoolItemContainer<ObjectType>(this, objectItem);
		}

		public void ReleaseObject(ObjectType objectItem)
		{
			_objects.Enqueue(objectItem);
		}

		#endregion
	}
}
