using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public class EffectSound
    {
        private GameObject _owner;
        private List<AudioSourceInfo> _audioSourceInfos = new();
        
        public float _soundLifeTime = 5f;
        private bool _isActive = true;

        #region << =========== STATUS =========== >>

        private const string SFX_STATE_KEY = "SFXState";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const int DEFAULT_SFX_STATE = 1;
        private const float DEFAULT_SFX_VOLUME = 1f;
        
        private bool _isAudioOn;
        public bool IsAudioOn
        {
            get => _isAudioOn = PlayerPrefs.GetInt(SFX_STATE_KEY, DEFAULT_SFX_STATE) == 1;
            set
            {
                _isAudioOn = value;
                PlayerPrefs.SetInt(SFX_STATE_KEY, _isAudioOn ? 1 : 0);
                UpdateSFXVolume();
            }
        }
        
        private float _volumeRate = 1f;
        public float VolumeRate
        {
            get => _volumeRate = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_SFX_VOLUME);
            set
            {
                _volumeRate = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _volumeRate);
                UpdateSFXVolume();
            }
        }
        
        private void UpdateSFXVolume()
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                audioSourceInfo.VolumeRate = GetVolumeRate();
            }
        }
        
        private float GetVolumeRate() => _isAudioOn ? _volumeRate : 0f;
        
        public bool IsPlaying(string _strName)
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                if (audioSourceInfo._audioSource.isPlaying &&
                    audioSourceInfo._audioSource.clip != null &&
                    audioSourceInfo._audioSource.clip.name == _strName)
                    return true;
            }
            return false;
        }

        public void SetActive(bool isActive) => _isActive = isActive;
        public int GetAudioSourceCount() => _audioSourceInfos.Count;

        #endregion
        
        #region << =========== INIT =========== >>

        public void Init(GameObject owner)
        {
            _owner = owner;
            
            IsAudioOn = PlayerPrefs.GetInt(SFX_STATE_KEY, DEFAULT_SFX_STATE) == 1;
            VolumeRate = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_SFX_VOLUME);
            
            DebugHelper.Log(EDebugType.Audio, $"[SFX] Audio On: {IsAudioOn}, Volume: {VolumeRate}");
        }

        private AudioClip SoundReady(SoundSettingData settingData, AudioClip reqClip = null)
        {
            byte overlapCount = 0;
            AudioClip audioClip = null;

            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                if (audioSourceInfo._audioSource != null &&
                    audioSourceInfo._audioSource.clip != null &&
                    audioSourceInfo._audioSource.clip.name == settingData.SoundName)
                {
                    audioClip = audioSourceInfo._audioSource.clip;
                    if (audioSourceInfo._audioSource.isPlaying && ++overlapCount >= settingData.MaxOverlapCount)
                        return null;
                }
            }
            if (audioClip == null) audioClip = reqClip;
            if (audioClip == null) audioClip = GetAudioClip(settingData.SoundName);

            return audioClip;
        }
        
        protected virtual AudioClip GetAudioClip(string soundName)
        {
            return Global.Resource.GetAudioClip(soundName);
        }
        
        #endregion
        
        #region << =========== PLAY =========== >>
        
        public void PlaySound(string soundName, int maxSoundCount = 3)
        {
            var settingItem = new SoundSettingData
            {
                MaxOverlapCount = maxSoundCount,
                SoundName = soundName
            };
            PlaySound(settingItem, null);
        }

        public void PlaySound(SoundSettingData settingData, AudioClip clip, float minDis = -1f, float maxDis = -1f, bool isLoop = false, bool isOneShot = false, bool is3D = false)
        {
            if (!_isActive || _owner == null)
                return;

            AudioClip audioClip = SoundReady(settingData, clip);
            if (audioClip == null)
                return;

            AudioSourceInfo audioSourceInfo = GetEmptyAudioSourceInfo();
            if (audioSourceInfo == null)
            {
                AudioSource audioSource = _owner.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSourceInfo = new AudioSourceInfo { _audioSource = audioSource };
                _audioSourceInfos.Add(audioSourceInfo);
            }

            audioSourceInfo.Play(settingData, audioClip, GetVolumeRate(), Time.time + _soundLifeTime, minDis, maxDis, isLoop, isOneShot, is3D);
        }
        
        #endregion

        #region << =========== STOP =========== >>
        
        public void StopSound(string soundName)
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                if (audioSourceInfo._audioSource.isPlaying &&
                    audioSourceInfo._audioSource.clip != null &&
                    audioSourceInfo._audioSource.clip.name == soundName)
                {
                    audioSourceInfo.Stop();
                }
            }
        }
        
        public void AllStop()
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                audioSourceInfo.Stop();
                audioSourceInfo._audioSource.clip = null;
            }
        }
        
        #endregion
        
        #region << =========== REMOVE =========== >>
        
        private void RemoveSound(string soundName)
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                if (audioSourceInfo._audioSource.clip != null &&
                    audioSourceInfo._audioSource.clip.name == soundName)
                {
                    audioSourceInfo.Stop();
                    audioSourceInfo._audioSource.clip = null;
                }
            }
        }

        #endregion

        #region << =========== AUDIO SOURCE INFO =========== >>

        private AudioSourceInfo GetEmptyAudioSourceInfo()
        {
            foreach (var audioSourceInfo in _audioSourceInfos)
            {
                if (audioSourceInfo._audioSource.clip == null ||
                    (!audioSourceInfo._audioSource.isPlaying && audioSourceInfo._lifeTime < Time.time))
                    return audioSourceInfo;
            }
            return null;
        }

        private class AudioSourceInfo
        {
            public AudioSource _audioSource;
            public SoundSettingData _soundSetting;
            public float _lifeTime;

            public float VolumeRate
            {
                get => _audioSource.volume;
                set => _audioSource.volume = value;
            }

            public void Play(SoundSettingData settingData, AudioClip clip, float volumeRate, float lifeTime, float minDist, float maxDist, bool isLoop = false, bool isOneShot = false, bool is3D = false)
            {
                _soundSetting = settingData;
                _lifeTime = lifeTime;
                _audioSource.volume = volumeRate;
                _audioSource.clip = clip;
                _audioSource.loop = isLoop;
                
                if (isOneShot) _audioSource.PlayOneShot(_audioSource.clip, _audioSource.volume);
                else _audioSource.Play();
            }

            public void Stop() => _audioSource.Stop();
        }

        #endregion
    }
}
