using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace SongLib
{
    public abstract class BaseAudioManager<T> : SingletonWithMono<T>, IBaseManager, IAudioManager where T : BaseAudioManager<T>
    {
        protected List<SoundSettingData> _soundSettings = new List<SoundSettingData>();
        private BGMSound _bgmSound = new BGMSound();
        private EffectSound _effectSound = new EffectSound();
        private AudioClip _bgmAudioClip;
        
        private readonly string AUDIO_STATE_KEY = "AudioState";
        private readonly string AUDIO_VOLUME_KEY = "AudioVolume";
        private readonly int DEFAULT_AUDIO_STATE = 1; 
        private readonly float DEFAULT_AUDIO_VOLUME = 1f;
        
        public bool IsInitialized { get; set; }
        public void Init()
        {
            Global.Init(this);
            _bgmSound.Init(gameObject);
            _effectSound.Init(gameObject);

            IsAudioOn = PlayerPrefs.GetInt(AUDIO_STATE_KEY, DEFAULT_AUDIO_STATE) == 1;
            VolumeRate = PlayerPrefs.GetFloat(AUDIO_VOLUME_KEY, DEFAULT_AUDIO_VOLUME);
            
            DebugHelper.Log(EDebugType.Init, $"🟢- [ AudioManager ] Initialize Completed!");
            
            IsInitialized = true;
        }

        private void Update() => _bgmSound.Update();

        private bool _isAudioOn;
        public bool IsAudioOn
        {
            get => _isAudioOn = PlayerPrefs.GetInt(AUDIO_STATE_KEY, DEFAULT_AUDIO_STATE) == 1;
            set
            {
                _isAudioOn = value;
                PlayerPrefs.SetInt(AUDIO_STATE_KEY, _isAudioOn ? 1 : 0);
                UpdateAudioListenerVolume();
            }
        }

        private float _volumeRate;
        public float VolumeRate
        {
            get => _isAudioOn ? _volumeRate = PlayerPrefs.GetFloat(AUDIO_VOLUME_KEY, DEFAULT_AUDIO_VOLUME) : 0f;
            set
            {
                _volumeRate = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(AUDIO_VOLUME_KEY, _volumeRate);
                UpdateAudioListenerVolume();
            }
        }

        private void UpdateAudioListenerVolume()
        {
            AudioListener.volume = _isAudioOn ? _volumeRate : 0f;
        }

        public void AllStop()
        {
            _bgmSound.AllStop();
            _effectSound.AllStop();
        }

        // Sound Setting ========================================================
        public SoundSettingData GetSoundSetting(string soundName, bool isLoop = true)
        {
            foreach (var soundSetting in _soundSettings)
            {
                if (soundSetting.SoundName == soundName)
                    return soundSetting;
            }

            var newSetting = new SoundSettingData { MaxOverlapCount = 3, SoundName = soundName, IsLoop = isLoop};
            _soundSettings.Add(newSetting);
            return newSetting;
        }

        public void SetSoundSetting(SoundSettingData setData)
        {
            for (int i = 0; i < _soundSettings.Count; i++)
            {
                if (_soundSettings[i].SoundName == setData.SoundName)
                {
                    _soundSettings[i].MaxOverlapCount = setData.MaxOverlapCount;
                    return;
                }
            }

            _soundSettings.Add(setData);
        }
        
        // BGM ========================================================
        public void PlayBGM(string bgmName, bool isLoop = true, float endTime = 0.5f)
        {
            StopAllCoroutines();

            if (PlayBGM(Global.UtilResource.GetAudioClip(bgmName), isLoop, endTime)) return;
            Debug.LogError($"Do Not Exist Sound : {bgmName}");
        }

        public bool PlayBGM(AudioClip bgmClip, bool isLoop = true, float endTime = 0.5f)
        {
            if (bgmClip == null) return false;
            _bgmAudioClip = bgmClip;
            var soundSetting = GetSoundSetting(bgmClip.name, isLoop);
            _bgmSound.PlaySound(bgmClip, soundSetting, endTime);
            return true;
        }
        
        public void PlayBGMWithDelay(string bgmName, float delay, bool isLoop = true)
        {
            StopAllCoroutines();
            StartCoroutine(PlayBGMWithDelayCoroutine(bgmName, delay, isLoop));
        } 
        
        private IEnumerator PlayBGMWithDelayCoroutine(string bgmName, float delay, bool isLoop = true)
        {
            yield return new WaitForSeconds(delay);
            PlayBGM(bgmName, isLoop);
        }

        public void StopBGM(float endTime = 0.5f, bool isBgmInit = false)
        {
            _bgmSound.StopSound(endTime);
            if (isBgmInit) _bgmAudioClip = null;
        }
        
        public bool IsBGMOn
        {
            get => _bgmSound.IsAudioOn;
            set => _bgmSound.IsAudioOn = value;
        }

        public float BGMVolumeRate
        {
            get => _bgmSound.VolumeRate;
            set => _bgmSound.VolumeRate = value;
        }

        public string GetCurBGMName()
        {
            var curBgmName = _bgmSound.GetCurSoundName;
            return curBgmName == "" && _bgmAudioClip != null ? _bgmAudioClip.name : curBgmName;
        }

        public void PauseBGM(bool _bPause)
        {
            if (_bPause) _bgmSound.Pause();
            else _bgmSound.Play();
        }

        public void SetBGMActive(bool isActive)
        {
            _bgmSound.IsActive = isActive;
            if (isActive) PlayBGM(_bgmAudioClip);
            else StopBGM();
        }

        // SFX ========================================================
        public void PlaySFX(string soundName, bool isLoop = false, bool isOneShot = false)
        {
            if(soundName.IsNullOrEmpty()) return;
            
            _effectSound.PlaySound(GetSoundSetting(soundName), null, isLoop: isLoop, isOneShot: isOneShot);
        }

        public void PlaySFX(AudioClip clip, bool isLoop = false, bool isOneShot = false)
        {
            _effectSound.PlaySound(GetSoundSetting(clip.name), clip, isLoop: isLoop, isOneShot: isOneShot);
        }
        
        public void PlaySFXWithDelay(string soundName, float delay)
        {
            StopAllCoroutines();
            StartCoroutine(PlaySFXWithDelayCoroutine(soundName, delay));
        } 
        
        private IEnumerator PlaySFXWithDelayCoroutine(string soundName, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySFX(soundName);
        }

        public void StopSFX(string soundName) => _effectSound.StopSound(soundName);
        public bool IsPlayingSFX(string soundName) => _effectSound.IsPlaying(soundName);

        public bool IsSFXOn
        {
            get => _effectSound.IsAudioOn;
            set => _effectSound.IsAudioOn = value;
        }
        
        public float SFXVolumeRate
        {
            get => _effectSound.VolumeRate;
            set => _effectSound.VolumeRate = value;
        }

        public int GetAudioSourceCount() => _effectSound.GetAudioSourceCount();
        public void SetEffectActive(bool isActive) => _effectSound.SetActive(isActive);
    }
}