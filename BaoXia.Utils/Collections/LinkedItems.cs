using System;
using System.Collections;
using System.Collections.Generic;

namespace BaoXia.Utils.Collections
{
	public class LinkedItems<ListItemType>
			: IEnumerable<ListItemType>, IDisposable
			where ListItemType : LinkedItem<ListItemType>
	{

		////////////////////////////////////////////////
		// @静态常量
		////////////////////////////////////////////////

		#region 静态常量

		protected class LinkedListEnumerator : IEnumerator<ListItemType>
		{
			////////////////////////////////////////////////
			// @自身属性
			////////////////////////////////////////////////

			#region 自身属性

			protected LinkedItems<ListItemType>? _list;

			protected ListItemType? _current;

			#endregion


			////////////////////////////////////////////////
			// @自身实现
			////////////////////////////////////////////////

			#region 自身实现

			public LinkedListEnumerator(LinkedItems<ListItemType> list)
			{
				_list = list;
			}

			~LinkedListEnumerator()
			{
				_list = null;
				_current = null;
			}

			#endregion


			////////////////////////////////////////////////
			// @实现“IEnumerator”
			////////////////////////////////////////////////

			#region 实现“IEnumerator”

			public ListItemType Current => _current!;

			object IEnumerator.Current => _current!;

			public void Dispose()
			{
				_list = null;
				_current = null;

				////////////////////////////////////////////////
				GC.SuppressFinalize(this);
				////////////////////////////////////////////////
			}

			public bool MoveNext()
			{
				if (_current == null)
				{
					// !!!
					_current = _list?.First;
					// !!!
				}
				else
				{
					// !!!
					_current = _current.Next;
					// !!!
				}
				if (_current == null)
				{
					return false;
				}
				return true;
			}

			public void Reset()
			{
				_current = _list?.First;
			}

			#endregion
		}

		#endregion



		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		protected ListItemType? _first;
		public ListItemType? First { get => _first; }

		protected ListItemType? _last;
		public ListItemType? Last { get => _last; }

		protected int _count;
		public int Count { get => _count; }

		#endregion


		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		~LinkedItems()
		{
			this.Clear();
		}

		public LinkedItem<ListItemType> Add(ListItemType listItem)
		{
			////////////////////////////////////////////////
			if (listItem.OwnerList != null)
			{
				listItem.OwnerList.Remove(listItem);
			}
			listItem.OwnerList = this;
			////////////////////////////////////////////////


			if (_first == null)
			{
				listItem.Prev = null;
				_first = listItem;
			}
			else // if (_last != null)
			{
				listItem.Prev = _last;
				_last!.Next = listItem;
			}
			listItem.Next = null;

			_last = listItem;
			_count++;

			return listItem;
		}

		public int AddRange(ICollection<ListItemType> listItems)
		{
			var listItemsCountAdded = 0;
			foreach (var listItem in listItems)
			{
				if (this.Add(listItem) != null)
				{
					listItemsCountAdded++;
				}
			}
			return listItemsCountAdded;
		}

		public ListItemType InsertBefore(
			ListItemType? nextListItem,
			ListItemType listItem)
		{
			if (nextListItem != null
				&& nextListItem.OwnerList != this)
			{
				throw new ArgumentException("nextListItem 不是当前链表的元素。");
			}

			////////////////////////////////////////////////
			if (listItem.OwnerList != null)
			{
				listItem.OwnerList.Remove(listItem);
			}
			listItem.OwnerList = this;
			////////////////////////////////////////////////


			if (nextListItem == null)
			{
				listItem.Prev = _last;
				listItem.Next = null;

				if (_last == null)
				{
					_first = listItem;
				}
				else
				{
					_last.Next = listItem;
				}
				_last = listItem;
			}
			else
			{
				listItem.Prev = nextListItem.Prev;
				listItem.Next = nextListItem;

				if (nextListItem.Prev != null)
				{
					nextListItem.Prev.Next = listItem;
				}
				nextListItem.Prev = listItem;

				if (_first == nextListItem)
				{
					_first = listItem;
				}
			}
			_count++;

			return listItem;
		}

		public int InsertRangeBefore(
			ListItemType? nextListItem,
			ICollection<ListItemType> listItems)
		{
			var listItemsCountInserted = 0;
			foreach (var listItem in listItems)
			{
				if (this.InsertBefore(
					nextListItem,
					listItem) != null)
				{
					listItemsCountInserted++;
				}
			}
			return listItemsCountInserted;
		}

		public ListItemType InsertAfter(
			ListItemType? prevListItem,
			ListItemType listItem)
		{
			if (prevListItem != null
				&& prevListItem.OwnerList != this)
			{
				throw new ArgumentException("prevListItem 不是当前链表的元素。");
			}

			////////////////////////////////////////////////
			if (listItem.OwnerList != null)
			{
				listItem.OwnerList.Remove(listItem);
			}
			listItem.OwnerList = this;
			////////////////////////////////////////////////


			if (prevListItem == null)
			{
				listItem.Prev = null;
				listItem.Next = _first;

				if (_first == null)
				{
					_last = listItem;
				}
				else
				{
					_first.Prev = listItem;
				}
				_first = listItem;
			}
			else
			{
				listItem.Prev = prevListItem;
				listItem.Next = prevListItem.Next;

				if (prevListItem.Next != null)
				{
					prevListItem.Next.Prev = listItem;
				}
				prevListItem.Next = listItem;

				if (_last == prevListItem)
				{
					_last = listItem;
				}
			}
			_count++;

			return listItem;
		}

		public int InsertRangeAfter(
			ListItemType? prevListItem,
			ICollection<ListItemType> listItems)
		{
			var listItemsCountInserted = 0;
			foreach (var listItem in listItems)
			{
				if (this.InsertAfter(
					prevListItem,
					listItem) != null)
				{
					listItemsCountInserted++;
				}
			}
			return listItemsCountInserted;
		}

		public ListItemType? Remove(ListItemType listItem)
		{
			if (listItem.OwnerList != this)
			{
				throw new ArgumentException("listItem 不是当前链表的元素。");
			}

			if (_first == listItem)
			{
				_first = listItem.Next;
			}
			if (_last == listItem)
			{
				_last = listItem.Prev;
			}

			if (listItem.Prev != null)
			{
				listItem.Prev.Next = listItem.Next;
			}
			if (listItem.Next != null)
			{
				listItem.Next.Prev = listItem.Prev;
			}
			_count--;

			listItem.OwnerList = null;
			listItem.Prev = null;
			listItem.Next = null;

			return listItem;
		}

		public int Clear()
		{
			var count = _count;
			{
				ListItemType? nextListItem = null;
				for (var listItem = _first;
				listItem != null;
				listItem = nextListItem)
				{
					nextListItem = listItem.Next;
					//
					listItem.OwnerList = null;
					listItem.Prev = null;
					listItem.Next = null;
					//
				}
				_first = null;
				_last = null;
				_count = 0;
			}
			return count;
		}

		public int IndexOf(ListItemType listItem)
		{
			if (listItem == null
				|| listItem.OwnerList != this)
			{
				return -1;
			}
			int indexOfListItem = 0;
			for (var listItemExisted = _first;
				listItemExisted != null;
				listItemExisted = listItem.Next)
			{
				if (listItemExisted == listItem)
				{
					return indexOfListItem;
				}
				indexOfListItem++;
			}
			return -1;
		}

		public ListItemType this[int index]
		{
			get
			{
				if (index < 0
					|| index >= this.Count)
				{
					throw new IndexOutOfRangeException();
				}


				if (index <= this.Count / 2)
				{
					var itemIndex = 0;
					for (var item = _first;
						item != null;
						item = item.Next)
					{
						if (itemIndex == index)
						{
							return item;
						}
						itemIndex++;
					}
				}
				else
				{
					var itemIndex = this.Count - 1;
					for (var item = _last;
						item != null;
						item = item.Prev)
					{
						if (itemIndex == index)
						{
							return item;
						}
						itemIndex--;
					}
				}

				throw new Exception("未知的链表异常，没有在指定位置上找到对应的链表元素。");
			}
			set
			{
				if (index == this.Count)
				{
					this.Add(value);
				}

				var itemReplaced = this[index];
				if (itemReplaced == null)
				{
					throw new Exception("未知的链表异常，没有在指定位置上找到对应的链表元素。");
				}

				var prevItem = itemReplaced.Prev;
				this.Remove(itemReplaced);
				this.InsertAfter(prevItem, value);
			}
		}

		#endregion

		////////////////////////////////////////////////
		// @实现“IDispose”
		////////////////////////////////////////////////

		#region 实现“IDispose”

		public void Dispose()
		{
			this.Clear();

			////////////////////////////////////////////////
			GC.SuppressFinalize(this);
			////////////////////////////////////////////////
		}


		#endregion

		////////////////////////////////////////////////
		// @实现“IEnumerator”
		////////////////////////////////////////////////

		#region 实现“IEnumerator”

		public IEnumerator<ListItemType> GetEnumerator()
		{
			return new LinkedListEnumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
