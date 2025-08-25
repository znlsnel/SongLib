using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class UtilString
{
    public static StringBuilder LocalString { get; private set; } = new StringBuilder(1024);


    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    public static bool IsNullOrWhiteSpace(this string input) => string.IsNullOrWhiteSpace(input);

    public static void ParsingStringSplit(this List<string> parsingList, string input, char splitCondition)
    {
        if (input.IsNullOrWhiteSpace()) return;

        parsingList.AddRange(
            input.Split(splitCondition)
                .Select(part => part.Trim())
                .Where(part => !part.IsNullOrEmpty())
        );
    }


    /// <summary>
    /// 문자열을 구분자로 나누어 정수 리스트로 반환합니다.
    /// </summary>
    public static List<int> ParsingStringSplitToInt(this string target, char splitCondition)
    {
        return string.IsNullOrEmpty(target)
            ? new List<int>()
            : target.Split(splitCondition).Select(part => part.Trim().ToInt()).ToList();
    }


    /// <summary>
    /// 문자열을 구분자로 나누어 부동소수점 리스트로 반환합니다.
    /// </summary>
    public static List<float> ParsingStringSplitToFloat(this string target, char splitCondition)
    {
        return string.IsNullOrEmpty(target)
            ? new List<float>()
            : target.Split(splitCondition).Select(part => part.Trim().ToFloat()).ToList();
    }

    /// <summary>
    /// 문자열의 대괄호를 제거합니다.
    /// </summary>
    private static string ReplaceBracket(this string value)
    {
        return value.Replace('[', ' ').Replace(']', ' ').Trim();
    }




    #region << =========== CONVERTER =========== >>
    public static short ToShort(this string _value)
    {
        short result = 0;
        short.TryParse(_value, out result);
        return result;
    }

    public static ushort ToUShort(this string _value)
    {
        ushort result = 0;
        ushort.TryParse(_value, out result);
        return result;
    }

    public static int ToInt(this string _value)
    {
        int result = 0;
        int.TryParse(_value, out result);
        return result;
    }

    public static uint ToUInt(this string _value)
    {
        uint result = 0;
        uint.TryParse(_value, out result);
        return result;
    }

    public static long ToLong(this string _value)
    {
        long result = 0;
        long.TryParse(_value, out result);
        return result;
    }

    public static ulong ToULong(this string _value)
    {
        ulong result = 0;
        ulong.TryParse(_value, out result);
        return result;
    }

    public static float ToFloat(this string _value)
    {
        float result = 0.0f;
        float.TryParse(_value, out result);
        return result;
    }

    public static double ToDouble(this string _value)
    {
        double result = 0.0;
        double.TryParse(_value, out result);
        return result;
    }

    public static Decimal ToDecimal(this string _value)
    {
        Decimal result = 0M;
        Decimal.TryParse(_value, out result);
        return result;
    }

    public static bool IsNumber(this string _value)
    {
        double result = 0.0;
        return double.TryParse(_value, out result);
    }

    /// <summary>
    /// 문자열을 열거형으로 변환합니다.
    /// </summary>
    public static T ToEnum<T>(this string _value)
    {
        System.Type enumType = typeof(T);
        return !string.IsNullOrEmpty(_value) && Enum.IsDefined(enumType, (object)_value)
            ? (T)Enum.Parse(enumType, _value)
            : default(T);
    }
    
        public static bool ToBool(this string _value)
    {
        bool result = false;
        bool.TryParse(_value.ToLower(), out result);
        return result;
    }

    public static bool ToBool(this int _value) => _value != 0;

    public static sbyte ToSByte(this string _value)
    {
        sbyte result = 0;
        sbyte.TryParse(_value, out result);
        return result;
    }

    public static byte ToByte(this string _value)
    {
        byte result = 0;
        byte.TryParse(_value, out result);
        return result;
    }

    public static byte ToByte(this char _value) => Convert.ToByte(_value);

    public static byte[] ToBytes(this string _str) => Encoding.UTF8.GetBytes(_str);

    public static void ToBytes(this string sz, byte[] _dst)
    {
        int length = _dst.Length;
        byte[] bytes = Encoding.UTF8.GetBytes(sz);
        int count = Math.Min(bytes.Length, length);
        Buffer.BlockCopy((Array)bytes, 0, (Array)_dst, 0, count);
    }
#endregion
}
