using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SongLib
{
    public static class DebugHelper
    {
        #region << =========== NONE TAG (CHAIN) =========== >>

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Log(object message) => Log(EDebugType.None, message);

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogFormat(string format, params object[] args) => LogFormat(EDebugType.None, format, args);

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning(object message) => LogWarning(EDebugType.None, message);

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError(object message) => LogError(EDebugType.None, message);

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(bool condition, string desc = "") => Assert(EDebugType.None, condition, desc);

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull(object obj, string desc = "") => AssertNotNull(EDebugType.None, obj, desc);

        #endregion

        #region << =========== IDebugTag (TERMINAL) =========== >>
        // 터미널: 여기서만 Debug.Log* 를 "직접" 호출합니다.
        // 절대 LogOption.NoStacktrace를 쓰지 마세요(링크 죽음).

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Log(IDebugTag tag, object message)
        {
            if (Global.Debug?.IsTagEnabled(tag) != true) return;
            Debug.Log($"[{tag.Icon} {tag.Name}] {message}");
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogFormat(IDebugTag tag, string format, params object[] args)
        {
            if (Global.Debug?.IsTagEnabled(tag) != true) return;
            Debug.Log(string.Format($"[{tag.Icon} {tag.Name}] {format}", args));
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning(IDebugTag tag, object message)
        {
            if (Global.Debug?.IsTagEnabled(tag) != true) return;
            Debug.LogWarning($"[{tag.Icon} {tag.Name}] {message}");
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError(IDebugTag tag, object message)
        {
            Debug.LogError($"[{tag.Icon} {tag.Name}] {message}");
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(IDebugTag tag, bool condition, string desc = "")
        {
            if (Global.Debug?.IsTagEnabled(tag) != true) return;
            if (condition) return;
            Debug.LogError($"[{tag.Icon} {tag.Name}][Assert] {desc}");
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull(IDebugTag tag, object obj, string desc = "")
        {
            if (Global.Debug?.IsTagEnabled(tag) != true) return;
            if (obj != null) return;
            Debug.LogError($"[{tag.Icon} {tag.Name}][AssertNotNull] {desc}");
        }

        #endregion

        #region << =========== ENUM BASED (CHAIN) =========== >>

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Log(EDebugType type, object message)
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<EDebugType> debugTag = type;
            Log(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogFormat(EDebugType type, string format, params object[] args)
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<EDebugType> debugTag = type;
            LogFormat(debugTag, format, args);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning(EDebugType type, object message)
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<EDebugType> debugTag = type;
            LogWarning(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError(EDebugType type, object message)
        {
            DebugTag<EDebugType> debugTag = type;
            LogError(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(EDebugType type, bool condition, string desc = "")
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<EDebugType> debugTag = type;
            Assert(debugTag, condition, desc);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull(EDebugType type, object obj, string desc = "")
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<EDebugType> debugTag = type;
            AssertNotNull(debugTag, obj, desc);
        }

        #endregion

        #region << =========== GENERIC (CHAIN) =========== >>

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Log<T>(T enumValue, object message) where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            Log(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogFormat<T>(T enumValue, string format, params object[] args) where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            LogFormat(debugTag, format, args);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogWarning<T>(T enumValue, object message) where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            LogWarning(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void LogError<T>(T enumValue, object message) where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            LogError(debugTag, message);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert<T>(T enumValue, bool condition, string desc = "") where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            Assert(debugTag, condition, desc);
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.NoInlining)]
        public static void AssertNotNull<T>(T enumValue, object obj, string desc = "") where T : Enum
        {
            if (Global.Debug?.IsEnabled() != true) return;
            DebugTag<T> debugTag = enumValue;
            AssertNotNull(debugTag, obj, desc);
        }

        #endregion
    }
}