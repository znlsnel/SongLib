using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SongLib
{
    [RequireComponent(typeof(UIContainer))]
    public abstract class UIPanel : UIBase
    {
        [BoxGroup("UI Panel")][SerializeField] protected UIContainer _uiContainer;
        [BoxGroup("UI Panel")][SerializeField][PropertyOrder(-1)] private List<UIPanel> _childUIPanelList = new();

        private void OnValidate()
        {
            if (_uiContainer == null)
                _uiContainer = this.GetOrAddComponent<UIContainer>(); 
        }

        public override void Init()
        {
            base.Init();

            _uiContainer = this.GetOrAddComponent<UIContainer>();
            _uiContainer.Init();

            if (_childUIPanelList == null)
                return;

            for (int i = 0; i < _childUIPanelList.Count; i++)
            {
                _childUIPanelList[i]?.Init();
            }


        }

        public override void Refresh()
        {
            base.Refresh();

            if (_childUIPanelList == null)
                return;

            for (int i = 0; i < _childUIPanelList.Count; i++)
            {
                _childUIPanelList[i]?.Refresh();
            }
        }

        public override void SetInfo(IInfo infoData)
        {
            base.SetInfo(infoData);

            if (_childUIPanelList == null)
                return;

            for (int i = 0; i < _childUIPanelList.Count; i++)
            {
                _childUIPanelList[i]?.SetInfo(infoData);
            }
        }

        protected abstract override void OnInit();
        protected abstract override void OnRefresh();

        #region << =========== DEBUG =========== >>

        public new string GetDebugInfo()
        {
            string info = $"UIPanel Info:\n" +
                          $"Init: {(isInitialized ? "true" : "false")}\n" +
                          $"Name: {gameObject.name}\n" +
                          $"Child UIBase Count: {_childUIPanelList.Count}\n";

            foreach (var child in _childUIPanelList)
            {
                info += $"- {child.GetShortDebugInfo()}\n";
            }

            return info;
        }

        #endregion
    }
}   