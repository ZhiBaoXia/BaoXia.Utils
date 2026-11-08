using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class ItemsOrderByTimeBoundaryTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void TimeRangesIncludeAllRepeatedStartTimesAndExcludeAllEndTimes()
	{
		var now = new DateTimeOffset(2026, 11, 8, 0, 0, 0, TimeSpan.Zero);
		foreach (var ascending in new[] { true, false })
		{
			var times = Enumerable.Range(0, 12).Select(index => now.AddHours(index / 3)).ToArray();
			if (!ascending)
			{
				Array.Reverse(times);
			}
			for (var beginHour = -1; beginHour <= 5; beginHour++)
			{
				for (var endHour = -1; endHour <= 5; endHour++)
				{
					var begin = now.AddHours(beginHour);
					var end = now.AddHours(endHour);
					var expected = times.Where(time => time >= begin && time < end).ToArray();
					Func<DateTimeOffset, DateTimeOffset, int> comparer = (time, target) => time.CompareTo(target);
					CollectionAssert.AreEqual(expected, times.GetItemsSortByTimeInTimeRange(ascending, begin, end, comparer) ?? []);
					IList<DateTimeOffset> list = times.ToList();
					CollectionAssert.AreEqual(expected, list.GetItemsSortByTimeInTimeRange(ascending, begin, end, comparer) ?? []);
					Assert.AreEqual(expected.Length, times.GetCountOfItemsSortByTimeInTimeRange(ascending, begin, end, comparer));
					Assert.AreEqual(expected.Length, list.GetCountOfItemsSortByTimeInTimeRange(ascending, begin, end, comparer));
				}
			}
		}
	}

	[TestMethod]
	public void SingleAndEmptyTimeRangesKeepTheirResults()
	{
		var now = DateTimeOffset.UtcNow;
		foreach (var ascending in new[] { true, false })
		{
			foreach (var times in new[] { Array.Empty<DateTimeOffset>(), new[] { now }, new[] { now, now, now } })
			{
				Func<DateTimeOffset, DateTimeOffset, int> comparer = (time, target) => time.CompareTo(target);
				Assert.AreEqual(times.Length, times.GetItemsSortByTimeInTimeRange(ascending, now, now.AddTicks(1), comparer)?.Length ?? 0);
				Assert.AreEqual(times.Length, ((IList<DateTimeOffset>)times.ToList()).GetItemsSortByTimeInTimeRange(ascending, now, now.AddTicks(1), comparer)?.Length ?? 0);
				Assert.IsNull(times.GetItemsSortByTimeInTimeRange(ascending, now.AddTicks(-1), now, comparer));
			}
		}
	}

	#endregion
}
