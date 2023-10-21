using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using System;

namespace BaoXia.Utils
{
        public class DateTimeSection
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public TimeSectionType Type { get; set; }

                public DateTime? BeginTime { get; set; }

                public DateTime? LastTime { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public DateTimeSection(
                        TimeSectionType type = TimeSectionType.NotLoop,
                        DateTime? beginTime = null,
                        DateTime? lastTime = null)
                {
                        Type = type;
                        BeginTime = beginTime;
                        LastTime = lastTime;
                }


                public bool IsTimeInSection(DateTime dateTime)
                {
                        var beginTime = BeginTime;
                        var lastTime = LastTime;
                        var timeSectionType = Type;
                        if (beginTime != null
                                && lastTime != null)
                        {
                                return DidIsTimeInSection(
                                        dateTime,
                                        beginTime.Value,
                                        lastTime.Value,
                                        timeSectionType);
                        }
                        else if (beginTime != null)
                        {
                                return DidIsBeginTimeLessOrEqualTime(
                                        dateTime,
                                        beginTime.Value,
                                        timeSectionType);
                        }
                        else if (lastTime != null)
                        {
                                return DidIsLastTimeGreatOrEqualTime(
                                        dateTime,
                                        lastTime.Value,
                                        timeSectionType);
                        }
                        // 起始时间和结束时间都为空时，
                        // 表示不对时间进行限制。
                        // else if (beginTime == null
                        //         && lastTime == null)
                        return true;
                }

                #endregion


                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                #region 事件节点

                protected bool DidIsTimeInSection(
                        DateTime dateTime,
                        DateTime beginTime,
                        DateTime lastTime,
                        TimeSectionType timeSectionType)
                {
                        DateTimeCycle dateTimeCompareCycle;
                        switch (timeSectionType)
                        {
                                default:
                                case TimeSectionType.NotLoop:
                                        {
                                                if (dateTime >= beginTime
                                                        && dateTime <= lastTime)
                                                {
                                                        return true;
                                                }
                                                return false;
                                        }
                                case TimeSectionType.LoopInCentury:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Century;
                                        }
                                        break;
                                case TimeSectionType.LoopInYear:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Year;
                                        }
                                        break;
                                case TimeSectionType.LoopInMonth:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Month;
                                        }
                                        break;
                                case TimeSectionType.LoopInWeek:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Week;
                                        }
                                        break;
                                case TimeSectionType.LoopInDay:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Day;
                                        }
                                        break;
                                case TimeSectionType.LoopInHour:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Hour;
                                        }
                                        break;
                                case TimeSectionType.LoopInMinute:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Minute;
                                        }
                                        break;
                        }

                        if (beginTime.CompareTo(
                                lastTime,
                                dateTimeCompareCycle)
                                <= 0)
                        {
                                if (dateTime.CompareTo(
                                        beginTime,
                                        dateTimeCompareCycle)
                                        >= 0
                                && dateTime.CompareTo(
                                        lastTime,
                                        dateTimeCompareCycle)
                                        <= 0)
                                {
                                        return true;
                                }
                        }
                        else
                        {
                                if (dateTime.CompareTo(
                                        beginTime,
                                        dateTimeCompareCycle)
                                        >= 0
                                        // && dateTime <= 当前时间范围最大值
                                        )
                                {
                                        return true;
                                }
                                if (dateTime.CompareTo(
                                        lastTime,
                                        dateTimeCompareCycle)
                                        <= 0
                                        // && dateTime >= 当前时间范围最小值
                                        )
                                {
                                        return true;
                                }
                        }
                        return false;
                }

                protected bool DidIsBeginTimeLessOrEqualTime(
                        DateTime dateTime,
                        DateTime beginTime,
                        TimeSectionType timeSectionType)
                {
                        DateTimeCycle dateTimeCompareCycle;
                        switch (timeSectionType)
                        {
                                default:
                                case TimeSectionType.NotLoop:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.All;
                                        }
                                        break;
                                case TimeSectionType.LoopInCentury:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Century;
                                        }
                                        break;
                                case TimeSectionType.LoopInYear:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Year;
                                        }
                                        break;
                                case TimeSectionType.LoopInMonth:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Month;
                                        }
                                        break;
                                case TimeSectionType.LoopInWeek:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Week;
                                        }
                                        break;
                                case TimeSectionType.LoopInDay:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Day;
                                        }
                                        break;
                                case TimeSectionType.LoopInHour:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Hour;
                                        }
                                        break;
                                case TimeSectionType.LoopInMinute:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Minute;
                                        }
                                        break;
                        }

                        if (dateTime.CompareTo(
                                beginTime,
                                dateTimeCompareCycle)
                                >= 0)
                        {
                                return true;
                        }
                        return false;
                }

                protected bool DidIsLastTimeGreatOrEqualTime(
                        DateTime dateTime,
                        DateTime lastTime,
                        TimeSectionType timeSectionType)
                {
                        DateTimeCycle dateTimeCompareCycle;
                        switch (timeSectionType)
                        {
                                default:
                                case TimeSectionType.NotLoop:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.All;
                                        }
                                        break;
                                case TimeSectionType.LoopInCentury:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Century;
                                        }
                                        break;
                                case TimeSectionType.LoopInYear:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Year;
                                        }
                                        break;
                                case TimeSectionType.LoopInMonth:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Month;
                                        }
                                        break;
                                case TimeSectionType.LoopInWeek:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Week;
                                        }
                                        break;
                                case TimeSectionType.LoopInDay:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Day;
                                        }
                                        break;
                                case TimeSectionType.LoopInHour:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Hour;
                                        }
                                        break;
                                case TimeSectionType.LoopInMinute:
                                        {
                                                dateTimeCompareCycle = DateTimeCycle.Minute;
                                        }
                                        break;
                        }

                        if (dateTime.CompareTo(
                                lastTime,
                                dateTimeCompareCycle)
                                <= 0)
                        {
                                return true;
                        }
                        return false;
                }

                #endregion
        }
}
