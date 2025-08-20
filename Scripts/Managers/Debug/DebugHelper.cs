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
            #if UNITY_EDITOR
            Debug.Log(GetDebugTypeToString(type) + message);
            #endif
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning(EDebugType type, object message)
        {
            #if UNITY_EDITOR
            Debug.LogWarning(GetDebugTypeToString(type) + message);
            #endif
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError(EDebugType type, object message)
        {
            #if UNITY_EDITOR
            Debug.LogError(GetDebugTypeToString(type) + message);
            #endif
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(EDebugType type, bool condition, string desc = "")
        {
            #if UNITY_EDITOR
            Debug.Assert(condition, GetDebugTypeToString(type) + desc);
            #endif
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull(EDebugType type, object obj, string desc = "")
        {
            #if UNITY_EDITOR
            Debug.Assert(obj != null, GetDebugTypeToString(type) + desc);
            #endif
        }

        private static string GetDebugTypeToString(EDebugType type)
        {
            return $"[{type.ToString()}] : ";
        }

    }
}