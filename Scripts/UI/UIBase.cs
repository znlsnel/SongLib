using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SongLib
{
    public abstract class UIBase : MonoBehaviour
    {
        [PropertyOrder(-4)][BoxGroup("UI Base")][ShowInInspector, ReadOnly] private int _uiIndex;
        [PropertyOrder(-3)][BoxGroup("UI Base")][SerializeField] private string _tagName = "";

        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        protected bool isInitialized = false;


        protected abstract void OnInit();
        protected abstract void OnRefresh();

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



        #region << ========== CANVAS GROUP ========== >>
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
    }
}
