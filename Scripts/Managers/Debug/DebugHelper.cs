using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SongLib
{
    public static class DebugHelper
    {
        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Log(EDebugType type, object message)
        {
            Debug.Log(GetDebugTypeToString(type) + message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning(EDebugType type, object message)
        {
            Debug.LogWarning(GetDebugTypeToString(type) + message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError(EDebugType type, object message)
        {
            Debug.LogError(GetDebugTypeToString(type) + message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(EDebugType type, bool condition, string desc = "")
        {
            Debug.Assert(condition, GetDebugTypeToString(type) + desc);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull(EDebugType type, object obj, string desc = "")
        {
            Debug.Assert(obj != null, GetDebugTypeToString(type) + desc);
        }

        public static string GetDebugTypeToString(EDebugType type)
        {
            return $"[{type.ToString()}] : ";
        }

    }
}