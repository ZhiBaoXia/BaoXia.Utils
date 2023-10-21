using System;
using System.Collections.Generic;
using System.Reflection;

namespace BaoXia.Utils.Extensions
{
        public static class TypeListExtension
        {
                ////////////////////////////////////////////////
                // @类方法
                ////////////////////////////////////////////////

                #region 类方法

                public static List<Type> RemoveTypesWithBaseClassSpecified(
                        this List<Type> typeList,
                        Type? baseClassSpecified,
                        Type? baseInterfaceSpecified = null,
                        Type? customAttributeTypeSpecified = null)
                {
                        if (baseClassSpecified != null)
                        {
                                typeList.NotRemoveIf((type) =>
                                {
                                        if (type.IsSubclassOf(baseClassSpecified))
                                        {
                                                return true;
                                        }
                                        return false;
                                });
                        }

                        if (baseInterfaceSpecified != null)
                        {
                                typeList.NotRemoveIf((type) =>
                                {
                                        if (type.IsAssignableFrom(baseInterfaceSpecified))
                                        {
                                                return true;
                                        }
                                        return false;
                                });
                        }

                        if (customAttributeTypeSpecified != null)
                        {
                                typeList.NotRemoveIf((type) =>
                                {
                                        var customAttributeSpecifed
                                        = type.GetCustomAttribute(customAttributeTypeSpecified);
                                        if (customAttributeSpecifed != null)
                                        {
                                                return true;
                                        }
                                        return false;
                                });
                        }

                        return typeList;
                }

                #endregion
        }
}
