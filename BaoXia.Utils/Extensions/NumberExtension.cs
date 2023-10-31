using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
        public static class NumberExtension
        {
                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static int IfNullOrLessThanReturn(
                        this int? number,
                        int defaultValue,
                        int compareValue = 0)
                {
                        if (number == null
                                || number < compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static float IfNullOrLessThanReturn(
                        this float? number,
                        float defaultValue,
                        float compareValue = 0.0F)
                {
                        if (number == null
                                || number < compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static double IfNullOrLessThanReturn(
                        this double? number,
                        double defaultValue,
                        double compareValue = 0.0)
                {
                        if (number == null
                                || number < compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                #endregion
        }
}
