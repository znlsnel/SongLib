using System;

namespace SongLib
{
    public struct DebugTag<T> : IDebugTag where T : Enum
    {
        #region << =========== FIELDS =========== >>
        
        private readonly T _enumValue;
        private readonly Func<T, string> _iconMapper;
        
        #endregion

        #region << =========== PROPERTIES =========== >>
        
        public string Name => _enumValue.ToString();
        public int Value => Convert.ToInt32(_enumValue);
        public string Icon => _iconMapper?.Invoke(_enumValue) ?? "🏷️";
        
        #endregion

        #region << =========== CONSTRUCTOR =========== >>
        
        public DebugTag(T enumValue, Func<T, string> iconMapper = null)
        {
            _enumValue = enumValue;
            _iconMapper = iconMapper ?? GetDefaultIconMapper();
        }
        
        #endregion

        #region << =========== METHODS =========== >>
        
        private static Func<T, string> GetDefaultIconMapper()
        {
            if (typeof(T) == typeof(EDebugType))
            {
                return (enumValue) =>
                {
                    var debugType = (EDebugType)(object)enumValue;
                    return debugType switch
                    {
                        
                        EDebugType.System => "⚙️",
                        EDebugType.Init => "🚀",
                        EDebugType.UI => "🖥️",
                        EDebugType.Popup => "📋",
                        EDebugType.Audio => "🔊",
                        EDebugType.Table => "📊",
                        EDebugType.Info => "ℹ️",
                        EDebugType.Test => "🧪",
                        EDebugType.Object => "📦",
                        EDebugType.Tutorial => "📚",
                        EDebugType.Event => "⚡",
                        EDebugType.Localize => "🌍",
                        EDebugType.Backend => "🌐",
                        _ => "🏷️"
                    };
                };
            }
            
            return null;
        }
        
        #endregion

        #region << =========== IMPLICIT OPERATORS =========== >>
        
        public static implicit operator DebugTag<T>(T enumValue) => new(enumValue);
        
        #endregion
    }
}