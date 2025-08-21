using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
#if SET_URP
using UnityEngine.Rendering.Universal;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SongLib
{
    public abstract class UIPopup : UIPanel
    {
        #region << ========== FIELDS ========== >>

        [BoxGroup("UI CONTAINER")] [SerializeField] private UIContainer _uiContainer;

        [BoxGroup("Components")] [SerializeField] private Camera _popupCamera; 
        [BoxGroup("Components")] [SerializeField] protected Transform contentTF;
        [BoxGroup("Components")] [SerializeField] private GameObject _blockObj;
        [BoxGroup("Components")] [SerializeField] private Button _exitBtn;
        
        // ===============================================
        [BoxGroup("Parameter")] [SerializeField] private bool _isAutoAnimation = true;
        [BoxGroup("Parameter")] [SerializeField] private bool _isCustomAnimation = false;
        
        // ===============================================
        [Title("Open Animation")]
        [BoxGroup("Auto Animation")] [ShowIf(nameof(IsNotEqualCustomDOTween))] [SerializeField] private Ease _openEaseType = Ease.Unset;
        [BoxGroup("Auto Animation")] [ShowIf(nameof(IsNotEqualOpenEaseUnset))] [SerializeField] private float _openDuration = 0.1f;
        
        [Title("Close Animation")]
        [BoxGroup("Auto Animation")] [ShowIf(nameof(IsNotEqualCustomDOTween))] [SerializeField] private Ease _closeEaseType = Ease.Unset;
        [BoxGroup("Auto Animation")] [ShowIf(nameof(IsNotEqualCloseEaseUnset))] [SerializeField] private float _closeDuration = 0.1f;
        
        [Title("Open/Close Factor")]
        [BoxGroup("Auto Animation")] [ShowIf(nameof(IsNotEqualEaseUnset))] [SerializeField] private float _initScaleFactor = 0.5f;
        
        private bool IsNotEqualCustomDOTween => !_isCustomAnimation && _isAutoAnimation;
        private bool IsNotEqualOpenEaseUnset => _openEaseType != Ease.Unset && IsNotEqualCustomDOTween;
        private bool IsNotEqualCloseEaseUnset => _closeEaseType != Ease.Unset && IsNotEqualCustomDOTween;
        private bool IsNotEqualEaseUnset => IsNotEqualOpenEaseUnset || IsNotEqualCloseEaseUnset && IsNotEqualCustomDOTween;
        
        // ===============================================
        // TODO:minb - 아직 로직은 안넣음
        [BoxGroup("Delay")] [SerializeField] private bool _isOpenDelay = false;
        [BoxGroup("Delay")] [ShowIf(nameof(_isOpenDelay))] [SerializeField] private float _openDelayTime = 0.0f;
        
        [BoxGroup("Delay")] [SerializeField] private bool _isCloseDelay = false;
        [BoxGroup("Delay")] [ShowIf(nameof(_isCloseDelay))] [SerializeField] private float _closeDelayTime = 0.0f;
        
        // ===============================================
        [BoxGroup("Custom Animation")] [ShowIf(nameof(IsCustomAnim))] [SerializeField] private Animator _popupAnimator;
        [BoxGroup("Custom Animation")] [SerializeField] private bool _isCustomOpenAnim = false;
        [BoxGroup("Custom Animation")] [SerializeField] private bool _isCustomCloseAnim = false;
        
        public bool IsCustomAnim => _isCustomOpenAnim || _isCustomCloseAnim;
        
        // ===============================================
        [BoxGroup("Auto Close")] [SerializeField] private bool _isAutoClose = false;
        [BoxGroup("Auto Close")] [ShowIf(nameof(_isAutoClose))] [SerializeField] private float _autoCloseTime = 0.0f;
            
        // ===============================================
        [BoxGroup("Sound")] [SerializeField] private string _openSoundName = "";
        [BoxGroup("Sound")] [SerializeField] private string _closeSoundName = "";

        #endregion

        #region << ========== PROPERTIES ========== >>
        // ===============================================
        public int PrevPopupID { get; set; } = 0;
        public bool IsOpened { get; set; } = false;
        private bool IsOpening { get; set; } = false;
        private bool IsClosing { get; set; } = false;

        private int _depth = 0;
        protected bool _isDisableAndroidKey = false;
        private IEnumerator _autoCloseCoroutine = null;
        
        #endregion

        #region << ========== EVENTS ========== >>
        
        private Action<UIPopup> _onInternalClose;
        private Action _onClose;
        
        public void SetCloseEvent(Action onClose)
        {
            _onClose = onClose;
        }

        #endregion

        #region << ========== ABSTRACT METHODS ========== >>
        public abstract int GetPopupID();
        protected abstract void OnInitPopup();
        #endregion

        #region << ========== INITIALIZATION ========== >>

        protected sealed override void OnInit()
        {
            if (_uiContainer == null)
            {
                InitChildUIBase();
            }
            else
            {
                _uiContainer.Init();
            }
            
            SetCanvasGroupAll(true);

            SceneManager.sceneLoaded += OnSceneLoaded;

            _exitBtn.onClick.AddListener(Close);

            OnInitPopup();
        }
        

        public void SetInternalEvent(Action<UIPopup> internalClose)
        {
            _onInternalClose = internalClose;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetCameraStack();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _onInternalClose = null;
            _onClose = null;
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        #endregion

        #region << =========== CAMERA =========== >>

        private void SetCameraStack()
        {
#if SET_URP
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                DebugHelper.LogError(DebugType.UI, "[CameraManager] Base Camera(Main Camera)가 없습니다.");
                return;
            }

            var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();

            if (!mainCameraData.cameraStack.Contains(_popupCamera))
            {
                mainCameraData.cameraStack.Add(_popupCamera);
                DebugHelper.Log(DebugType.UI, "UI Camera가 Main Camera Stack에 추가되었습니다.");
            }
#endif
        }

        private void RemoveCameraStack()
        {
#if SET_URP
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                DebugHelper.LogError(DebugType.UI, "[CameraManager] Base Camera(Main Camera)가 없습니다.");
                return;
            }

            var mainCameraData = mainCamera.GetUniversalAdditionalCameraData();

            if (mainCameraData.cameraStack.Contains(_popupCamera))
            {
                mainCameraData.cameraStack.Remove(_popupCamera);
                DebugHelper.Log(DebugType.UI, "UI Camera가 Main Camera Stack에서 제거되었습니다.");
            }
#endif
        }

        #endregion

        #region << =========== PUBLIC METHOD =========== >>
        
        public virtual void Open()
        {
            if (IsOpening)
            {
                return;
            }
            
            IsOpening = true;
            IsClosing = false;
            IsOpened = true;
            PrevPopupID = 0;
            
            OpenSound();
            OpenWindow();
            
            if (_isAutoClose)
            {
                if (_autoCloseCoroutine != null)
                {
                    StopCoroutine(_autoCloseCoroutine);
                }
            }
        }

        public virtual void Close()
        {
            if (IsClosing)
            {
                return;
            }
            
            IsOpening = false;
            IsClosing = true;
            IsOpened = false;
            
            CloseSound();
            CloseWindow();
        }
        
        #endregion

        #region << =========== OPEN =========== >>

        protected virtual void OpenWindow()
        {
            Show();
            SetBlockObj(true);
            SetCameraStack();
            
            Refresh();

            if (_isCustomOpenAnim) _popupAnimator.SetTrigger(StringKeys.Open);
            else if (_isAutoAnimation || _isCustomAnimation) OpenAnimation();
            else OnOpenWindowComplete();
        }

        protected virtual void OpenAnimation()
        {
            if (_openEaseType == Ease.Unset || contentTF == null)
            {
                OnOpenWindowComplete();
                return;
            }

            contentTF.localScale = Vector3.one * _initScaleFactor;
            contentTF.DOScale(Vector3.one, _openDuration).SetEase(_openEaseType)
                .SetUpdate(true).OnComplete(OnOpenWindowComplete);
        }

        protected virtual void OnOpenWindowComplete()
        {
            SetBlockObj(false);
            IsOpening = false;

            if (_isAutoClose)
            {
                _autoCloseCoroutine = AutoClose();
                StartCoroutine(_autoCloseCoroutine);
            }
        }
        
        #endregion

        #region << =========== CLOSE =========== >>

        protected virtual void CloseWindow()
        {
            SetBlockObj(true);

            if (_isCustomCloseAnim) _popupAnimator.SetTrigger(StringKeys.Close);
            else if (_isAutoAnimation || _isCustomAnimation) CloseAnimation();
            else OnCloseWindowComplete();
        }
        
        public virtual void ForceClose()
        {
            if (IsClosing) return;
            
            IsClosing = true;
            
            if (contentTF != null)
            {
                contentTF.DOKill();
            }
            
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
                _autoCloseCoroutine = null;
            }
            
            OnCloseWindowComplete();
        }
        
        protected virtual void CloseAnimation()
        {
            if (_closeEaseType == Ease.Unset || contentTF == null)
            {
                OnCloseWindowComplete();
                return;
            }
            
            contentTF.DOScale(Vector3.one*_initScaleFactor, _closeDuration).SetEase(_closeEaseType).
                SetUpdate(true).OnComplete(OnCloseWindowComplete);
        }

        public virtual void OnCloseWindowComplete()
        {
            SetBlockObj(false);
            IsClosing = false;
            RemoveCameraStack();
            
            _onInternalClose?.Invoke(this);
            _onClose?.Invoke();
            
            Hide();
        }
        
        private IEnumerator AutoClose()
        {
            yield return new WaitForSeconds(_autoCloseTime);
            Close();
        }

        #endregion
        
        #region << =========== ANIMATION =========== >>

        public void OnOpenAnimationCompleted()
        {
            OnOpenWindowComplete();
        }

        public void OnCloseAnimationCompleted()
        {
            OnCloseWindowComplete();
        }

        #endregion

        #region << =========== SOUND =========== >>

        protected void OpenSound()
        {
            if (_openSoundName.IsNullOrEmpty()) return;
            
            Global.UtilAudio.PlaySFX(_openSoundName);
        }

        protected void CloseSound()
        {
            if (_closeSoundName.IsNullOrEmpty()) return;

            Global.UtilAudio.PlaySFX(_closeSoundName);
        }

        #endregion

        #region << =========== ANDROID KEY =========== >>

        public virtual void OnUpdateAndroidKey(EAndroidKeyStateType keyState)
        {
            if (_isDisableAndroidKey || keyState != EAndroidKeyStateType.Back)
            {
                return;
            }

            Close();
        }

        public void SetDisableAndroidBackKey(bool isActive) => _isDisableAndroidKey = isActive;
        public bool IsDisableAndroidBackKey() => _isDisableAndroidKey;

        #endregion
        
        #region << =========== UTIL =========== >>
        
        public Camera GetCamera() => _popupCamera;
        public float GetCameraSize() => _popupCamera.orthographicSize;

        public virtual int SetPosAndDepth(Vector3 pos, int depth)
        {
            _depth = depth;
            if (_popupCamera != null)
            {
                _popupCamera.depth = _depth;
                transform.localPosition = pos;
            }
            else
                transform.localPosition = pos;

            return _depth;
        }

        public virtual int GetDepth() => _depth;

        public virtual Vector3 GetNextUIPos()
        {
            return transform.position + Vector3.down * 5f * _popupCamera.orthographicSize;
        }
        
        private void SetBlockObj(bool isActive)
        {
            if (_blockObj != null)
            {
                _blockObj.SetActive(isActive);
            }
        }

        #endregion

        #region << =========== DEBUG =========== >>
        
        public new string GetDebugInfo()
        {
            return $"UIPopup Info:\n" +
                   $"Init: {(isInitialized ? "true" : "false")}\n" +
                   $"Name: {gameObject.name}\n" +
                   $"Camera: {_popupCamera?.name ?? "null"}\n" +
                   $"Content Transform: {contentTF?.name ?? "null"}\n" +
                   $"Is Auto Animation: {_isAutoAnimation}\n" +
                   $"Is Custom Animation: {_isCustomAnimation}\n" +
                   $"Open Ease Type: {_openEaseType}\n" +
                   $"Close Ease Type: {_closeEaseType}\n" +
                   $"Open Duration: {_openDuration}\n" +
                   $"Close Duration: {_closeDuration}\n" +
                   $"Init Scale Factor: {_initScaleFactor}\n" +
                   $"Is Auto Close: {_isAutoClose}\n" +
                   $"Auto Close Time: {_autoCloseTime}";
        }

        #endregion
    }
}