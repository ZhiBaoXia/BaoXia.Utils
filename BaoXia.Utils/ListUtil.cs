using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
        public class ListUtil
        {
                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static bool IsEmpty<T>(List<T>? list)
                {
                        if (list?.Any() == true)
                        {
                                return false;
                        }
                        return true;
                }

                public static bool IsNotEmpty<T>(List<T>? list)
                {
                        return !ListUtil.IsEmpty(list);
                }

                #endregion
        }
}
