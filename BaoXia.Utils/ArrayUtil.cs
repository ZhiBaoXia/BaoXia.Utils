using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
        public class ArrayUtil
        {

                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static bool IsEmpty<T>(T[]? array)
                {
                        if (array?.Any() == true)
                        {
                                return false;
                        }
                        return true;
                }

                public static bool IsNotEmpty<T>(T[]? array)
                {
                        return !ArrayUtil.IsEmpty(array);
                }

                #endregion
        }
}
