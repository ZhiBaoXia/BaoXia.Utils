using System;

namespace BaoXia.Utils.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ObjectToBytesOptionAttribute : Attribute
{
        ////////////////////////////////////////////////
        // @自身属性
        ////////////////////////////////////////////////

        #region 自身属性

        public bool IsPropertyIgnored { get; set; }

        #endregion
}