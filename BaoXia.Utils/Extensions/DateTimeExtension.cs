using System;

namespace BaoXia.Utils.Extensions
{
        /// <summary>
        /// “DateTime”扩展类。
        /// </summary>
        public static class DateTimeExtension
        {
                public enum DateTimeField
                {
                        Year,
                        Month,
                        Day,
                        Hour,
                        Minute,
                        Second,
                        Millisecond
                }

                /// <summary>
                /// 通过指定距离1970年1月1日零时的毫秒数，创建当前时区的时间。
                /// </summary>
                /// <param name="millisecondsFrom1970">距离1970年1月1日零时的毫秒数。</param>
                /// <returns>距离1970年1月1日零时的毫秒数对应的当前时区时间。</returns>
                public static DateTime DateTimeWithMillisecondsFrom1970(
                    long millisecondsFrom1970)
                {
                        var dateTime = new DateTime(1970, 1, 1);
                        {
                                dateTime = dateTime.AddMilliseconds(millisecondsFrom1970);
                                //
                                dateTime = dateTime.ToLocalTime();
                        }
                        return dateTime;
                }

                /// <summary>
                /// 获取当前时间的UTC时间戳，当前时间的UTC时间距离1970年1月1日零时的毫秒数。
                /// </summary>
                /// <param name="dateTime">当前时间对象。</param>
                /// <returns>返回当前时间的UTC时间距离1970年1月1日零时的毫秒数。</returns>
                public static long MillisecondsFrom1970(this DateTime dateTime)
                {
                        var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
                        var zeroTime = new DateTime(1970, 1, 1);

                        var millisecondsFrom1970 = (long)(dateTimeUtc - zeroTime).TotalMilliseconds;
                        { }
                        return millisecondsFrom1970;
                }

                /// <summary>
                /// 在置顶的时间精度上比较两个时间对象。
                /// </summary>
                /// <param name="dateTime">当前时间对象。</param>
                /// <param name="anotherDateTime">另一个时间对象。</param>
                /// <param name="compareFieldMin">指定的时间精度，类型为：DateTimeField。</param>
                /// <returns></returns>
                public static int CompareTo(
                        this DateTime dateTime,
                        DateTime anotherDateTime,
                        DateTimeField compareFieldMin = DateTimeField.Millisecond)
                {
                        if (compareFieldMin >= DateTimeField.Year)
                        {
                                if (dateTime.Year > anotherDateTime.Year)
                                {
                                        return 1;
                                }
                                else if (dateTime.Year < anotherDateTime.Year)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Month)
                        {
                                if (dateTime.Month > anotherDateTime.Month)
                                {
                                        return 1;
                                }
                                else if (dateTime.Month < anotherDateTime.Month)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Day)
                        {
                                if (dateTime.Day > anotherDateTime.Day)
                                {
                                        return 1;
                                }
                                else if (dateTime.Day < anotherDateTime.Day)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Hour)
                        {
                                if (dateTime.Hour > anotherDateTime.Hour)
                                {
                                        return 1;
                                }
                                else if (dateTime.Hour < anotherDateTime.Hour)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Minute)
                        {
                                if (dateTime.Minute > anotherDateTime.Minute)
                                {
                                        return 1;
                                }
                                else if (dateTime.Minute < anotherDateTime.Minute)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Second)
                        {
                                if (dateTime.Second > anotherDateTime.Second)
                                {
                                        return 1;
                                }
                                else if (dateTime.Second < anotherDateTime.Second)
                                {
                                        return -1;
                                }
                        }
                        if (compareFieldMin >= DateTimeField.Millisecond)
                        {
                                if (dateTime.Millisecond > anotherDateTime.Millisecond)
                                {
                                        return 1;
                                }
                                else if (dateTime.Millisecond < anotherDateTime.Millisecond)
                                {
                                        return -1;
                                }
                        }
                        return 0;
                }


                public static bool IsEarlierInYear(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Year)
                                < 0;
                }

                public static bool EqualsInYear(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Year)
                                == 0;
                }

                public static bool IsLaterInYear(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Year)
                                > 0;
                }

                public static bool IsEarlierInMonth(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Month)
                                < 0;
                }

                public static bool EqualsInMonth(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Month)
                                == 0;
                }

                public static bool IsLaterInMonth(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Month)
                                > 0;
                }

                public static bool IsEarlierInDay(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Day)
                                < 0;
                }

                public static bool EqualsInDay(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Day)
                                == 0;
                }

                public static bool IsLaterInDay(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Day)
                                > 0;
                }


                public static bool IsEarlierInHour(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Hour)
                                < 0;
                }

                public static bool EqualsInHour(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Hour)
                                == 0;
                }

                public static bool IsLaterInHour(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Hour)
                                > 0;
                }


                public static bool IsEarlierInMinute(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Minute)
                                < 0;
                }

                public static bool EqualsInMinute(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Minute)
                                == 0;
                }

                public static bool IsLaterInMinute(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Minute)
                                > 0;
                }


                public static bool IsEarlierInSecond(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Second)
                                < 0;
                }

                public static bool EqualsInSecond(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Second)
                                == 0;
                }

                public static bool IsLaterInSecond(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Second)
                                > 0;
                }


                public static bool IsEarlierInMillisecond(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Millisecond)
                                < 0;
                }

                public static bool EqualsInMillisecond(
                        this DateTime dateTime,
                        DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Millisecond)
                                == 0;
                }

                public static bool IsLaterInMillisecond(
                       this DateTime dateTime,
                       DateTime anotherDateTime)
                {
                        return dateTime.CompareTo(
                                anotherDateTime,
                                DateTimeField.Millisecond)
                                > 0;
                }
        }
}
