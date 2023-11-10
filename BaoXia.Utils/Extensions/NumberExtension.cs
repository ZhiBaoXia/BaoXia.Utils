namespace BaoXia.Utils.Extensions
{
        public static class NumberExtension
        {
                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法


                public static int MinOne(int number)
                {
                        if (number >= 1)
                        {
                                return number;
                        }
                        return 1;
                }

                public static long MinOne(long number)
                {
                        if (number >= 1)
                        {
                                return number;
                        }
                        return 1;
                }

                public static float MinOne(float number)
                {
                        if (number >= 1)
                        {
                                return number;
                        }
                        return 1;
                }

                public static double MinOne(double number)
                {
                        if (number >= 1)
                        {
                                return number;
                        }
                        return 1;
                }

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

                public static int IfNullOrLessEqualThanReturn(
                        this int? number,
                        int defaultValue,
                        int compareValue = 0)
                {
                        if (number == null
                                || number <= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static float IfNullOrLessEqualThanReturn(
                        this float? number,
                        float defaultValue,
                        float compareValue = 0.0F)
                {
                        if (number == null
                                || number <= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static double IfNullOrLessEqualThanReturn(
                        this double? number,
                        double defaultValue,
                        double compareValue = 0.0)
                {
                        if (number == null
                                || number <= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static int IfNullOrGreaterThanReturn(
                        this int? number,
                        int defaultValue,
                        int compareValue = 0)
                {
                        if (number == null
                                || number > compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static float IfNullOrGreaterThanReturn(
                        this float? number,
                        float defaultValue,
                        float compareValue = 0.0F)
                {
                        if (number == null
                                || number > compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static double IfNullOrGreaterThanReturn(
                        this double? number,
                        double defaultValue,
                        double compareValue = 0.0)
                {
                        if (number == null
                                || number > compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static int IfNullOrGreaterEqualThanReturn(
                        this int? number,
                        int defaultValue,
                        int compareValue = 0)
                {
                        if (number == null
                                || number >= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static float IfNullOrGreaterEqualThanReturn(
                        this float? number,
                        float defaultValue,
                        float compareValue = 0.0F)
                {
                        if (number == null
                                || number >= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                public static double IfNullOrGreaterEqualThanReturn(
                        this double? number,
                        double defaultValue,
                        double compareValue = 0.0)
                {
                        if (number == null
                                || number >= compareValue)
                        {
                                return defaultValue;
                        }
                        return number.Value;
                }

                #endregion
        }
}
