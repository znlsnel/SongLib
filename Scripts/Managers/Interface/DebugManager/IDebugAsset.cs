using System.Collections.Generic;

namespace SongLib
{
    public interface IDebugAsset
    {
        bool IsTagEnabled(IDebugTag tag);
        bool IsTagEnabled(EDebugType type);
        bool IsEnabled { get; }
        IReadOnlyList<DebugTagSetting> TagSettings { get;  }
    }
}