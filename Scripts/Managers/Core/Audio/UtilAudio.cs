namespace SongLib
{
    public static class UtilAudio
    {
        public static void AddSoundSetting(string soundName, int maxOverlapCount)
        {
            SoundSettingData soundItem = new SoundSettingData();
            soundItem.SoundName = soundName;
            soundItem.MaxOverlapCount = maxOverlapCount;
            Global.UtilAudio.SetSoundSetting(soundItem);
        }
    }
}