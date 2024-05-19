using BaoXia.Utils.Constants;

namespace BaoXia.Utils.Models
{
        public class ScheduledTaskConfig
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public ScheduledType ScheduledType { get; set; }

                public DateTimeSection? TimeSection { get; set; }

                public double TaskIntervalSeconds { get; set; }

                #endregion

        }
}
