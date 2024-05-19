using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Extensions
{
        public static class CharExtension
        {
                public static bool IsCharIntegralNumber(this char currentChar)
                {
                        if (currentChar >= '0'
                                && currentChar <= '9')
                        {
                                return true;
                        }
                        return false;
                }

                public static bool IsCharFloatNumber(this char currentChar)
                {
                        if (currentChar.IsCharIntegralNumber())
                        {
                                return true;
                        }
                        if (currentChar == '.')
                        {
                                return true;
                        }
                        return false;
                }

                public static bool EqualsIgnoreCase(this char currentChar, char anotherChar)
                {
                        if (currentChar == anotherChar)
                        {
                                return true;
                        }
                        else if (currentChar >= 'a'
                                && currentChar <= 'z')
                        {
                                if (anotherChar >= 'A'
                                        && anotherChar <= 'Z')
                                {
                                        return true;
                                }
                        }
                        else if (currentChar >= 'A'
                                && currentChar <= 'Z')
                        {
                                if (anotherChar >= 'a'
                                        && anotherChar <= 'z')
                                {
                                        return true;
                                }
                        }
                        return false;
                }

                public static bool IsCharsOfStringEqualsKey(
                        string? key,
                        string str,
                        int beginCharIndex,
                        out int charsCountEqualed,
                        StringComparison stringComparison = StringComparison.Ordinal,
                        bool isIgnoreSpace = false)
                {
                        charsCountEqualed = 0;

                        if (key == null
                                || key.Length < 1)
                        {
                                return false;
                        }
                        if (str == null
                                || str.Length < 1)
                        {
                                return false;
                        }
                        if (beginCharIndex < 0)
                        {
                                return false;
                        }
                        if (beginCharIndex + key.Length > str.Length)
                        {
                                return false;
                        }

                        if (isIgnoreSpace)
                        {
                                if (stringComparison == StringComparison.CurrentCultureIgnoreCase
                                        || stringComparison == StringComparison.InvariantCultureIgnoreCase
                                        || stringComparison == StringComparison.OrdinalIgnoreCase)
                                {
                                        var keyCharIndex = 0;
                                        for (var charIndex = 0;
                                                charIndex < str.Length
                                                && keyCharIndex < key.Length;
                                                charIndex++)
                                        {
                                                var strChar = str[beginCharIndex + charIndex];
                                                var keyChar = key[keyCharIndex];
                                                if (strChar.EqualsIgnoreCase(keyChar) == true)
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!!
                                                        keyCharIndex++;
                                                }
                                                else if (strChar == ' ')
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!!
                                                }
                                                else
                                                {
                                                        return false;
                                                }
                                        }
                                }
                                else
                                {
                                        var keyCharIndex = 0;
                                        for (var charIndex = 0;
                                                charIndex < str.Length
                                                && keyCharIndex < key.Length;
                                                charIndex++)
                                        {
                                                var strChar = str[beginCharIndex + charIndex];
                                                var keyChar = key[keyCharIndex];
                                                if (strChar == keyChar)
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!!
                                                        keyCharIndex++;
                                                }
                                                else if (strChar == ' ')
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!!
                                                }
                                                else
                                                {
                                                        return false;
                                                }
                                        }
                                }
                        }
                        else
                        {
                                if (stringComparison == StringComparison.CurrentCultureIgnoreCase
                                        || stringComparison == StringComparison.InvariantCultureIgnoreCase
                                        || stringComparison == StringComparison.OrdinalIgnoreCase)
                                {
                                        for (var charIndex = 0;
                                                charIndex < key.Length;
                                                charIndex++)
                                        {
                                                var strChar = str[beginCharIndex + charIndex];
                                                var keyChar = key[charIndex];
                                                if (strChar.EqualsIgnoreCase(keyChar) == true)
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!! 
                                                }
                                                else
                                                {
                                                        return false;
                                                }
                                        }
                                }
                                else
                                {
                                        for (var charIndex = 0;
                                                charIndex < key.Length;
                                                charIndex++)
                                        {
                                                var strChar = str[beginCharIndex + charIndex];
                                                var keyChar = key[charIndex];
                                                if (strChar == keyChar)
                                                {
                                                        // !!!
                                                        charsCountEqualed++;
                                                        // !!!
                                                }
                                                else
                                                {
                                                        return false;
                                                }
                                        }
                                }
                        }
                        return true;
                }

                public static string? IsCharsOfStringEqualsKeys(
                        ICollection<string> keys,
                        string str,
                        int beginCharIndex,
                        StringComparison stringComparison = StringComparison.Ordinal,
                        bool isIgnoreSpace = false)
                {
                        if (keys == null
                                || keys.Count < 1)
                        {
                                return null;
                        }

                        foreach (var key in keys)
                        {
                                if (CharExtension.IsCharsOfStringEqualsKey(
                                        key,
                                        str,
                                        beginCharIndex,
                                        out _,
                                        stringComparison,
                                        isIgnoreSpace)
                                        == true)
                                {
                                        return key;
                                }
                        }
                        return null;
                }

                public static int GetIntegralNumberCharsCountFromString(
                        string str,
                        int beginCharIndex,
                        bool isIgnoreSpace,
                        out int intNumber)
                {
                        intNumber = 0;

                        if (str == null
                                || str.Length < 1)
                        {
                                return 0;
                        }
                        if (beginCharIndex >= str.Length)
                        {
                                return 0;
                        }

                        var integralNumberCharsCount = 0;
                        var spaceCharsCount = 0;
                        for (var charIndex = beginCharIndex;
                                charIndex < str.Length;
                                charIndex++)
                        {
                                var strChar = str[charIndex];
                                if (strChar.IsCharFloatNumber() == true)
                                {
                                        integralNumberCharsCount++;
                                }
                                else if (strChar == ' '
                                                && isIgnoreSpace == true)
                                {
                                        integralNumberCharsCount++;
                                        spaceCharsCount++;
                                }
                                else
                                {
                                        break;
                                }
                        }

                        if (integralNumberCharsCount > 0)
                        {
                                var intNumberString = str.Substring(beginCharIndex, integralNumberCharsCount);
                                if (spaceCharsCount > 0)
                                {
                                        intNumberString = intNumberString.Replace(" ", null);
                                }
                                if (intNumberString?.Length > 0)
                                {
                                        // !!!
                                        intNumber = int.Parse(intNumberString);
                                        // !!!
                                }
                        }
                        return integralNumberCharsCount;
                }

                public static int GetFloatNumberCharsCountFromString(
                        string str,
                        int beginCharIndex,
                        bool isIgnoreSpace,
                        out double floatNumber)
                {
                        floatNumber = 0.0;

                        if (str == null
                                || str.Length < 1)
                        {
                                return 0;
                        }
                        if (beginCharIndex >= str.Length)
                        {
                                return 0;
                        }

                        var floatNumberCharsCount = 0;
                        var spaceCharsCount = 0;
                        for (var charIndex = beginCharIndex;
                                charIndex < str.Length;
                                charIndex++)
                        {
                                var strChar = str[charIndex];
                                if (strChar.IsCharFloatNumber() == true)
                                {
                                        floatNumberCharsCount++;
                                }
                                else if (strChar == ' '
                                                && isIgnoreSpace == true)
                                {
                                        floatNumberCharsCount++;
                                        spaceCharsCount++;
                                }
                                else
                                {
                                        break;
                                }
                        }

                        if (floatNumberCharsCount > 0)
                        {
                                var intNumberString = str.Substring(beginCharIndex, floatNumberCharsCount);
                                if (spaceCharsCount > 0)
                                {
                                        intNumberString = intNumberString.Replace(" ", null);
                                }
                                if (intNumberString?.Length > 0)
                                {
                                        // !!!
                                        floatNumber = double.Parse(intNumberString);
                                        // !!!
                                }
                        }
                        return floatNumberCharsCount;
                }

                public static int GetStringCharsCountFromStringBeforeKeys(
                        ICollection<string> endKeys,
                        bool isStringEndEqualsKey,
                        string str,
                        int beginCharIndex,
                        bool isIgnoreSpace,
                        StringComparison stringComparison,
                        out string? stringChars,
                        out string? endKeyFound)
                {
                        stringChars = null;
                        endKeyFound = null;

                        if (str == null
                                || str.Length < 1)
                        {
                                return -1;
                        }
                        if (beginCharIndex >= str.Length)
                        {
                                return -1;
                        }

                        var stringCharsCount = -1;
                        if (endKeys == null
                                || endKeys.Count < 1)
                        {
                                stringCharsCount = str.Length - beginCharIndex;
                                stringChars = str[beginCharIndex..];
                                if (isIgnoreSpace == true)
                                {
                                        stringChars = stringChars.Replace(" ", null);
                                }
                        }
                        else
                        {
                                var spaceCharsCount = 0;
                                var lastCharIndex = str.Length - 1;
                                for (var charIndex = beginCharIndex;
                                        charIndex < str.Length;
                                        charIndex++)
                                {
                                        foreach (var endKey in endKeys)
                                        {
                                                if (CharExtension.IsCharsOfStringEqualsKey(
                                                        endKey,
                                                        str,
                                                        charIndex,
                                                        out _,
                                                        stringComparison))
                                                {
                                                        // !!!
                                                        endKeyFound = endKey;
                                                        break;
                                                        // !!!
                                                }
                                        }

                                        if (endKeyFound != null)
                                        {
                                                // !!!
                                                stringCharsCount = charIndex - beginCharIndex;
                                                break;
                                                // !!!
                                        }
                                        else if (isStringEndEqualsKey == true
                                                && charIndex == lastCharIndex)
                                        {
                                                // !!!
                                                stringCharsCount = charIndex + 1 - beginCharIndex;
                                                break;
                                                // !!!
                                        }
                                        else if (str[charIndex] == ' ')
                                        {
                                                spaceCharsCount++;
                                        }
                                }
                                if (stringCharsCount > 0)
                                {
                                        stringChars = str.Substring(beginCharIndex, stringCharsCount);
                                        if (spaceCharsCount > 0
                                                && isIgnoreSpace == true)
                                        {
                                                stringChars = stringChars.Replace(" ", null);
                                        }
                                }
                        }
                        return stringCharsCount;
                }


                public static int GetStringCharsCountFromStringBeforeKey(
                        string endKey,
                        bool isStringEndEqualsKey,
                        string str,
                        int beginCharIndex,
                        bool isIgnoreSpace,
                        StringComparison stringComparison,
                        out string? stringChars)
                {
                        stringChars = null;
                        if (str == null
                                || str.Length < 1)
                        {
                                return -1;
                        }
                        if (beginCharIndex >= str.Length)
                        {
                                return -1;
                        }

                        var stringCharsCount = -1;
                        if (endKey == null
                                || endKey.Length < 1)
                        {
                                stringCharsCount = str.Length - beginCharIndex;
                                stringChars = str[beginCharIndex..];
                                if (isIgnoreSpace == true)
                                {
                                        stringChars = stringChars.Replace(" ", null);
                                }
                        }
                        else
                        {
                                var spaceCharsCount = 0;
                                var lastCharIndex = str.Length - 1;
                                for (var charIndex = beginCharIndex;
                                        charIndex < str.Length;
                                        charIndex++)
                                {
                                        if (CharExtension.IsCharsOfStringEqualsKey(
                                                         endKey,
                                                         str,
                                                         charIndex,
                                                        out _,
                                                         stringComparison))
                                        {
                                                // !!!
                                                stringCharsCount = charIndex - beginCharIndex;
                                                break;
                                                // !!!
                                        }
                                        else if (isStringEndEqualsKey == true
                                                && charIndex == lastCharIndex)
                                        {
                                                // !!!
                                                stringCharsCount = charIndex + 1 - beginCharIndex;
                                                break;
                                                // !!!
                                        }
                                        else if (str[charIndex] == ' ')
                                        {
                                                spaceCharsCount++;
                                        }
                                }
                                if (stringCharsCount > 0)
                                {
                                        stringChars = str.Substring(beginCharIndex, stringCharsCount);
                                        if (spaceCharsCount > 0
                                                && isIgnoreSpace == true)
                                        {
                                                stringChars = stringChars.Replace(" ", null);
                                        }
                                }
                        }
                        return stringCharsCount;
                }
        }
}
