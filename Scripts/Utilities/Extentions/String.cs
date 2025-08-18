using UnityEngine;

public static class StringExtention
{
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
}