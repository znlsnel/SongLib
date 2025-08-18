using System;

namespace SongLib
{
    [Serializable]
    public class SoundSettingData
    {
        public int MaxOverlapCount;
        public string SoundName;
        public bool IsLoop;
    }
}