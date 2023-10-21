using System;

namespace BaoXia.Utils.TestUtils.Attributes
{
        [AttributeUsage(AttributeTargets.All)]
        public class NameAttribute : Attribute
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public string Name { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public NameAttribute(string name)
                {
                        Name = name;
                }

                #endregion
        }
}
