
namespace SongLib
{
    public interface IAudioManager
    {
        bool IsAudioOn { get; set; }
        float VolumeRate { get; set; }
        void AllStop();
        
        bool IsBGMOn { get; set; }
        float BGMVolumeRate { get; set; }
        string GetCurBGMName();
        
        void PlayBGM(string bgmName, bool isLoop = true, float endTime = 0.5f);
        void PlayBGMWithDelay(string bgmName, float delay, bool isLoop);
        void PauseBGM(bool isPause);
        void StopBGM(float endTime = 0.5f, bool isBgmInit = false);
        
        bool IsSFXOn { get; set; }
        float SFXVolumeRate { get; set; }
        void PlaySFX(string sfxName, bool isLoop = false, bool isOneShot = false);
        void PlaySFXWithDelay(string sfxName, float delay);
        void StopSFX(string sfxName);
        int GetAudioSourceCount();        

        void SetSoundSetting(SoundSettingData setData);
    }
}