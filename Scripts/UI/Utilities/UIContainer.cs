using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SongLib
{
    public class UIContainer : MonoBehaviour
    {
        [SerializeField] private List<UIBase> _uiBaseList = new();

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            _isInitialized = true;

            for (int i = 0; i < _uiBaseList.Count; i++)
            {
                if (_uiBaseList[i] == null) continue;

                _uiBaseList[i].Init();
            }
        }
        
        public void OnValidate()
        {
            if (Application.isPlaying) return;
            
            AutoCollectUIBase();
        }

#if UNITY_EDITOR
        [GUIColor(0.2f, 1f, 0f)]
        [PropertyOrder(-1)]
        [Button(ButtonSizes.Large, Name = "📦 Auto Collect UIBase")]
        public void AutoCollectUIBase()
        {
            _uiBaseList.Clear();
            _uiBaseList.AddRange(GetComponentsInChildren<UIBase>(true)
                .Where(uiBase => uiBase.gameObject != this.gameObject));
        }
#endif
    }
}