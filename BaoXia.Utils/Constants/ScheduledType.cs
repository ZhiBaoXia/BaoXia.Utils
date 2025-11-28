namespace BaoXia.Utils.Constants
{
        /// <summary>
        /// 定时类型。
        /// </summary>
        public enum ScheduledType
        {
                /// <summary>
                /// 固定任务开始之间的间隔秒数。
                /// </summary>
                FixedTaskStartIntervalSeconds,

                /// <summary>
                /// 固定任务之间的间隔秒数。
                /// </summary>
                FixedTaskIntervalSeconds,

                /// <summary>
                /// 固定的时间区间。
                /// </summary>
                FixedTimeSection
        }
}
