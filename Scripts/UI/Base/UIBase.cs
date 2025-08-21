using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SongLib
{
    public abstract class UIBase : MonoBehaviour
    {
        [PropertyOrder(-4)] [BoxGroup("UI Base")][ShowInInspector, ReadOnly] private int _uiIndex;
        [PropertyOrder(-3)] [BoxGroup("UI Base")] [SerializeField] private string _tagName = "";

        protected IInfo _infoData;
        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        private Tween _fadeTween;
        
        #region << =========== LIFE CYCLE =========== >>
        
        protected bool isInitialized = false;

        public virtual void Init()
        {
            if (isInitialized) return;
                
            Global.UtilUI.EnrollUI(this);
            
            canvasGroup = this.GetOrAddComponent<CanvasGroup>();
            rectTransform = this.GetOrAddComponent<RectTransform>();
            
            SetCanvasGroupAll(false);
            
            isInitialized = true;

            OnInit();
        }
        
        public virtual void Refresh()
        {
            OnRefresh();
        }

        public virtual void SetInfo(IInfo infoData)
        {
            _infoData = infoData;
        }

        protected abstract void OnInit();
        protected abstract void OnRefresh();

        protected virtual void OnDestroy()
        {
            Global.UtilUI.UnenrollUI(this);
            _fadeTween.Kill();
        }

        #endregion

        #region << =========== INIT =========== >>

        // TODO:minb:check - Show&Hide 부분 최적화 고려해서 추후 수정
        public virtual void Show(float duration = .0f)
        {
            if (GetActive() && GetCanvasGroupAlpha() == 1) return;

            gameObject.SetActive(true);
            
            FadeIn(duration);
        }

        public virtual void Hide(float duration = .0f)
        {
            FadeOut(duration);
        }

        #endregion

        #region << =========== ANIMATION =========== >>

        private void FadeIn(float duration)
        {
            _fadeTween?.Kill();

            if (duration <= 0)
            {
                canvasGroup.alpha = 1;
                return;
            }

            _fadeTween = canvasGroup.DOFade(1, duration)
                .SetUpdate(true);
        }

        private void FadeOut(float duration)
        {
            _fadeTween?.Kill();

            if (duration <= 0)
            {
                canvasGroup.alpha = 0;
                gameObject.SetActive(false);

                return;
            }

            _fadeTween = canvasGroup.DOFade(0, duration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        #endregion

        #region << =========== GET METHOD =========== >>

        public bool IsInitialized() => isInitialized;
        public string GetName() => gameObject.name;
        public int GetIndex() => _uiIndex;
        public string GetTagName() => _tagName;
        
        public bool GetActive() => gameObject.activeSelf;
        public Vector2 GetPosition() => transform.position;
        public Vector3 GetScale() => transform.localScale;
        public Quaternion GetRotation() => transform.localRotation;
        
        public RectTransform GetRectTransform() => rectTransform;
        public Rect GetRect() => rectTransform.rect;
        public Vector2 GetAnchoredPosition() => rectTransform.anchoredPosition;
        public Vector2 GetSizeDelta() => rectTransform.sizeDelta;
        
        public CanvasGroup GetCanvasGroup() => canvasGroup;
        public bool GetCanvasGroupInteractable() => canvasGroup.interactable;
        public bool GetCanvasGroupRaycasts() => canvasGroup.blocksRaycasts;
        public float GetCanvasGroupAlpha() => canvasGroup.alpha;
        public bool GetCanvasGroupVisible() => gameObject.activeSelf && canvasGroup.alpha > 0;

        #endregion
        
        #region << =========== SET METHOD =========== >>
        
        public void SetInitialized(bool isInit) => isInitialized = isInit;
        public void SetIndex(int index) => _uiIndex = index;
        public void SetTagName(string tagName) => _tagName = tagName;
        
        public void SetActive(bool active) => gameObject.SetActive(active);
        public void SetPosition(Vector3 position) => transform.position = position;
        public void SetScale(Vector3 scale) => transform.localScale = scale;
        public void SetRotation(Quaternion rotation) => transform.localRotation = rotation;
        
        public void SetAnchoredPosition(Vector2 position) => rectTransform.anchoredPosition = position;
        public void SetSizeDelta(Vector2 size) => rectTransform.sizeDelta = size;

        public void SetCanvasGroupAll(bool flag)
        {
            SetCanvasGroupInteractable(flag);
            SetCanvasGroupBlocksRaycasts(flag);
            SetCanvasGroupIgnoreParentGroups(flag);
        }
        public void SetRaycastControl(bool blocksRaycasts, bool ignoreParentGroups)
        {
            SetCanvasGroupBlocksRaycasts(blocksRaycasts);
            SetCanvasGroupIgnoreParentGroups(ignoreParentGroups);
        }
        public void SetCanvasGroupInteractable(bool interactable) => canvasGroup.interactable = interactable;
        public void SetCanvasGroupBlocksRaycasts(bool blocksRaycasts) => canvasGroup.blocksRaycasts = blocksRaycasts;
        public void SetCanvasGroupIgnoreParentGroups(bool isIgnore) => canvasGroup.ignoreParentGroups = isIgnore;
        public void SetCanvasGroupAlpha(float alpha) => canvasGroup.alpha = Mathf.Clamp01(alpha);

        #endregion

        #region << =========== UTILITIES =========== >>

        [Obsolete("This method causes performance degradation and should not be used. Please use a more efficient approach to initialize child UI components.")]
        protected void InitChildUIBase()
        {
            DebugHelper.LogWarning(EDebugType.UI, $"[{GetName()}] InitChildUIBase");
            UIBase[] uiBases = GetComponentsInChildren<UIBase>(true);

            foreach (var uiBase in uiBases)
            {
                uiBase.Init();
            }
        }
        
        public UIBase[] GetChildUIBases()
        {
            return GetComponentsInChildren<UIBase>(true);
        }

        protected void SetInteractable()
        {
            DebugHelper.LogWarning(EDebugType.UI, $"[{GetName()}] SetInteractable Method is Not Working");
            // if (UtilTutorial.IsActive())
            // {
            //     SetCanvasGroupIgnoreParentGroups(true);
            //     SetCanvasGroupInteractable(true);
            // }
            // else
            // {
            //     SetCanvasGroupAll(true);
            // }
        }

        #endregion

        #region << =========== DEBUG =========== >>
        
        public string GetShortDebugInfo() => $"[ {GetName()} ] - ( Index: {_uiIndex} )";
        
        public string GetDebugInfo()
        {
            return $"UIBase Info:\n" +
                   $"Init: {IsInitialized()}\n" +
                   $"Name: {GetName()}\n" +
                   $"Active: {GetActive()}\n" +
                   $"Visible: {GetCanvasGroupVisible()}\n" +
                   $"CanvasGroup Interactable: {GetCanvasGroupInteractable()}\n" +
                   $"CanvasGroup BlocksRaycasts: {GetCanvasGroupRaycasts()}\n" +
                   $"CanvasGroup Alpha: {GetCanvasGroupAlpha()}\n" +
                   $"RectTransform Size: {GetSizeDelta()}\n" +
                   $"RectTransform Position: {GetPosition()}\n";
        }

        #endregion
    }
}