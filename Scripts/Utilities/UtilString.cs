using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class UtilString
{
    public static StringBuilder LocalString { get; private set;} = new StringBuilder(1024);

    
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


    /// <summary>
    /// 문자열을 정수로 변환합니다.
    /// </summary>
    private static int ToInt(this string value)
    {
        return int.TryParse(value, out var result) ? result : 0;
    }


    /// <summary>
    /// 문자열을 부동소수점으로 변환합니다.
    /// </summary>
    private static float ToFloat(this string value)
    {
        return float.TryParse(value, out var result) ? result : 0f;
    }

    // public static string GetStringHHMM(this int seconds)
    // {
    //     TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
    //     return timeSpan.ToString(@"hh\:mm");
    // }


}
