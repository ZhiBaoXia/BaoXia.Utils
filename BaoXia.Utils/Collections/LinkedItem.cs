namespace BaoXia.Utils.Collections
{
	public class LinkedItem<ListItemType>
			where ListItemType : LinkedItem<ListItemType>
	{
		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public LinkedItems<ListItemType>? OwnerList { get; set; }

		public ListItemType? Prev { get; set; }

		public ListItemType? Next { get; set; }

		#endregion


	}
}
