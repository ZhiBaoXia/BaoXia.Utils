using System;

namespace BaoXia.Utils.Extensions
{
	public static class DateTimeOffsetExtension
	{
		////////////////////////////////////////////////
		// @类方法
		////////////////////////////////////////////////

		#region 类方法

		/// <summary>
		/// 将“DateTimeOffset”转为本地时间的“DateTime“。
		/// </summary>
		/// <param name="dateTimeOffset">当前“DateTimeOffset”对象。</param>
		/// <param name="dateTimeDefault">当前“DateTimeOffset”对象为“null”时，返回的默认“DateTime”值，默认为“null”，返回“DateTime.MinValue”。</param>
		/// <returns>当前“DateTimeOffset”对象对应的本地时间的“DateTime“。</returns>
		public static DateTime ToLocalDateTime(
			this DateTimeOffset? dateTimeOffset,
			DateTime? dateTimeDefault = null)
		{
			var dateTime = dateTimeDefault ?? DateTime.MinValue;
			if (dateTimeOffset != null)
			{
				dateTime = dateTimeOffset.Value.LocalDateTime;
			}
			return dateTime;
		}

		#endregion
	}
}
