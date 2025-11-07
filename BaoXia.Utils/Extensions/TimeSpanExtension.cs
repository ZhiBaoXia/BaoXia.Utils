using BaoXia.Utils.Constants;
using System;

namespace BaoXia.Utils.Extensions;

public static class TimeSpanExtension
{
        ////////////////////////////////////////////////
        // @类方法
        ////////////////////////////////////////////////

        #region 类方法

        [Obsolete("当前函数，已更名，推荐使用“TitleOfListElementDefault”方法替代。")]
        public static string CaptionOfListElementDefault(this TimeSpan timeSpan)
        {
                return TimeSpanExtension.TitleOfListElementDefault(timeSpan);
        }

        [Obsolete("当前函数，已更名，推荐使用“TitleOfDetailPageDefault”方法替代。")]
        public static string CaptionOfDetailPageDefault(this TimeSpan timeSpan)
        {
                return TimeSpanExtension.TitleOfDetailPageDefault(timeSpan);
        }

        public static string TitleOfListElementDefault(this TimeSpan timeSpan)
        {
                string caption;
                var totalMinutes = timeSpan.TotalMinutes;
                if (totalMinutes < 1)
                {
                        caption = string.Format("{0:F0}秒", timeSpan.TotalSeconds);
                }
                else if (totalMinutes >= TimeDefinition.Forever.TotalMinutes)
                {
                        caption = "永久";
                }
                else
                {
                        var totalHours = timeSpan.TotalHours;
                        if (totalHours < 1)
                        {
                                caption = string.Format("{0:F0}分钟", totalMinutes);
                        }
                        else
                        {
                                var totalDays = timeSpan.TotalDays;
                                if (totalDays < 1)
                                {
                                        caption = string.Format(
                                            "{0:0}小时, {1:F0}分钟",
                                            timeSpan.Hours,
                                            timeSpan.Minutes);
                                }
                                else
                                {
                                        caption = string.Format(
                                            "{0:0}天, {1:F0}小时",
                                            timeSpan.Days,
                                            timeSpan.Hours);
                                }
                        }
                }
                return caption;
        }

        public static string TitleOfListElementDefault(this double totalSeconds)
        {
                return TimeSpanExtension.TitleOfListElementDefault(TimeSpan.FromSeconds(totalSeconds));
        }

        public static string TitleOfDetailPageDefault(this TimeSpan timeSpan)
        {
                string caption;
                var totalMinutes = timeSpan.TotalMinutes;
                if (totalMinutes < 1)
                {
                        caption = string.Format("{0:F0}秒", timeSpan.TotalSeconds);
                }
                else if (totalMinutes >= TimeDefinition.Forever.TotalMinutes)
                {
                        caption = "永久";
                }
                else
                {
                        var totalHours = timeSpan.TotalHours;
                        if (totalHours < 1)
                        {
                                caption = string.Format("{0:0}分钟, {1:F0}秒",
                                    timeSpan.Minutes,
                                    timeSpan.Seconds);
                        }
                        else
                        {
                                var totalDays = timeSpan.TotalDays;
                                if (totalDays < 1)
                                {
                                        caption = string.Format(
                                            "{0:0}小时, {1:0}分钟, {2:F0}秒",
                                            timeSpan.Hours,
                                            timeSpan.Minutes,
                                            timeSpan.Seconds);
                                }
                                else
                                {
                                        caption = string.Format(
                                            "{0:0}天, {1:0}小时, {2:0}分钟, {3:F0}秒",
                                            timeSpan.Days,
                                            timeSpan.Hours,
                                            timeSpan.Minutes,
                                            timeSpan.Seconds);
                                }
                        }
                }
                return caption;
        }

        public static string TitleOfDetailPageDefault(this double totalSeconds)
        {
                return TimeSpanExtension.TitleOfDetailPageDefault(TimeSpan.FromSeconds(totalSeconds));
        }

        #endregion
}