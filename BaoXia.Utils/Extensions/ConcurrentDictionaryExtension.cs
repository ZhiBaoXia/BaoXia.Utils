using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions
{
        /// <summary>
        /// 并发字典类扩展。
        /// </summary>
        public static class ConcurrentDictionaryExtension
        {
                /// <summary>
                /// 添加或修改值。
                /// </summary>
                /// <typeparam name="TKey">关键字类型。</typeparam>
                /// <typeparam name="TValue">值类型。</typeparam>
                /// <param name="dictionary">当前并发字典对象。</param>
                /// <param name="key">关键字对象。</param>
                /// <param name="value">值对象。</param>
                /// <returns>返回新添加，或新修改的值。</returns>
                public static TValue AddOrUpdateWithNewValue<TKey, TValue>(
                        this ConcurrentDictionary<TKey, TValue> dictionary,
                        TKey key,
                        TValue value) where TKey : notnull
                {
                        return dictionary.AddOrUpdate(
                                key,
                                value,
                                (keyExisted, valueExisted) => value);
                }

                public static bool IsEquals<KeyType, ValueType>(
                        this Dictionary<KeyType, ValueType> dictionary,
                        Dictionary<KeyType, ValueType>? anotherDictionary,
                        bool isNullEqualsEmpty = true,
                        Func<ValueType, ValueType, bool>? toEqualsValues = null)
                        where KeyType : notnull
                {
                        if (anotherDictionary == null)
                        {
                                if (isNullEqualsEmpty == true
                                        && dictionary.Count == 0)
                                {
                                        return true;
                                }
                                return false;
                        }
                        else if (anotherDictionary.Count != dictionary.Count)
                        {
                                return false;
                        }

                        foreach (var keyValue in dictionary)
                        {
                                if (!anotherDictionary.TryGetValue(
                                        keyValue.Key,
                                        out var valueInAnotherDictionary))
                                {
                                        return false;
                                }

                                if (toEqualsValues != null)
                                {
                                        if (toEqualsValues(keyValue.Value, valueInAnotherDictionary) != true)
                                        {
                                                return false;
                                        }
                                }
                                else if (keyValue.Value == null
                                        && valueInAnotherDictionary == null)
                                {
                                        continue;
                                }
                                else if (keyValue.Value?.Equals(valueInAnotherDictionary) != true)
                                {
                                        return false;
                                }
                        }

                        return true;
                }

                public static bool IsEquals(
                        this Dictionary<string, string> dictionary,
                        Dictionary<string, string>? anotherDictionary,
                        bool isNullEqualsEmpty = true,
                        StringComparison stringComparison = StringComparison.Ordinal)
                {
                        return IsEquals(
                                dictionary,
                                anotherDictionary,
                                isNullEqualsEmpty,
                                (stringA, stringB) =>
                                {
                                        return BaoXia.Utils.Extensions.StringExtension.EqualsStrings(
                                                stringA,
                                                stringB,
                                                stringComparison,
                                                isNullEqualsEmpty);
                                });
                }
        }
}
