using UnityEngine;
using DG.Tweening;

namespace SongLib
{
    public class BGMSound
    {
        private GameObject _owner;
        private AudioSource _bgmSource;
        private AudioClip _nextBgmClip;
        private SoundSettingData _nextSoundSetting;
        private Tween _fadeTween;

        public bool IsPause { get; set; } = false;
        public bool IsActive { get; set; } = false;

        public string GetCurSoundName => _nextBgmClip != null ? _nextBgmClip.name : "";
        
        #region << =========== INIT =========== >>

        public void Init(GameObject owner)
        {
            _owner = owner;
            if (_bgmSource == null)
            {
                _bgmSource = _owner.AddComponent<AudioSource>();
                _bgmSource.playOnAwake = false;
            }

            IsAudioOn = PlayerPrefs.GetInt(BGM_STATE_KEY, DEFAULT_BGM_STATE) == 1;
            VolumeRate = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, DEFAULT_BGM_VOLUME);
            
            DebugHelper.Log(EDebugType.Audio, $"[BGM] Audio On: {IsAudioOn}, Volume: {VolumeRate}");
        }

        public void Update()
        {
            if (!IsActive || IsPause) return;

            if (!_bgmSource.isPlaying && _nextBgmClip != null)
            {
                ChangeNextSound();
            }
        }

        #endregion

        #region << =========== STATUS =========== >>
        
        private const string BGM_STATE_KEY = "BGMState";
        private const string BGM_VOLUME_KEY = "BGMVolume";
        private const int DEFAULT_BGM_STATE = 1;
        private const float DEFAULT_BGM_VOLUME = 1f;
        
        private bool _isAudioOn;
        public bool IsAudioOn
        {
            get => _isAudioOn = PlayerPrefs.GetInt(BGM_STATE_KEY, DEFAULT_BGM_STATE) == 1;
            set
            {
                _isAudioOn = value;
                PlayerPrefs.SetInt(BGM_STATE_KEY, _isAudioOn ? 1 : 0);
                UpdateBGMVolume();
            }
        }
        
        private float _volumeRate = 1f;
        public float VolumeRate
        {
            get => _volumeRate = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, DEFAULT_BGM_VOLUME);
            set
            {
                _volumeRate = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(BGM_VOLUME_KEY, _volumeRate);
                UpdateBGMVolume();
            }
        }
        
        private void UpdateBGMVolume()
        {
            _bgmSource.volume = GetVolumeRate();
        }
        
        private float GetVolumeRate() => _isAudioOn ? _volumeRate : 0f;
        
        #endregion

        #region << =========== PLAY =========== >>

        public void Play()
        {
            if (_nextBgmClip != null && _bgmSource.clip != _nextBgmClip) ChangeNextSound();
            else _bgmSource.Play();

            IsPause = false;
        }
        
        public void PlaySound(AudioClip bgmClip, SoundSettingData settingData, float endTime)
        {
            if (_bgmSource.clip == null)
            {
                SetNextSound(bgmClip, settingData);
                ChangeNextSound();
            }
            else
            {
                if (_bgmSource.isPlaying && _bgmSource.clip == bgmClip)
                    return;

                EndSound(endTime);
                SetNextSound(bgmClip, settingData);
            }
        }

        #endregion

        #region << =========== PAUSE =========== >>
        
        public void Pause()
        {
            _bgmSource.Pause();
            IsPause = true;
        }
        
        #endregion

        #region << =========== STOP =========== >>

        public void StopSound(float endTime)
        {
            _nextBgmClip = null;
            if (endTime > 0f)
            {
                EndSound(endTime);
            }
            else
            {
                _bgmSource.Stop();
            }
        }
        
        public void AllStop()
        {
            _bgmSource.Stop();
            _nextBgmClip = null;
            _fadeTween?.Kill();
        }

        private void EndSound(float endTime)
        {
            _fadeTween?.Kill();

            _fadeTween = _bgmSource.DOFade(0f, endTime).OnComplete(() =>
            {
                _bgmSource.Stop();
                ChangeNextSound();
            });
        }
        
        #endregion

        #region <<========= NEXT =========== >>
        
        private void ChangeNextSound()
        {
            if (_nextBgmClip != null)
            {
                if (_bgmSource.clip != _nextBgmClip) _bgmSource.clip = _nextBgmClip;

                _bgmSource.volume = GetVolumeRate();
                _bgmSource.loop = _nextSoundSetting.IsLoop;
                _bgmSource.Play();
            }
            else
            {
                _bgmSource.Stop();
            }
        }

        private void SetNextSound(AudioClip bgmClip, SoundSettingData settingData)
        {
            _nextBgmClip = bgmClip;
            _nextSoundSetting = settingData;
        }
        
        #endregion
    }
}