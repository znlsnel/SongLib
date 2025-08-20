using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SongLib
{
    public class UIManager: SingletonWithMono<UIManager>, IBaseManager, IUIManager
    {
        #region << =========== FLAG =========== >>
        
        [field: Title("Flag")]
        [field: SerializeField, ReadOnly] public bool IsInitialized { get; set; } = false;
        
        #endregion

        private int _enrollIndex = -1;
        
        private Dictionary<int, UIBase> uiDic = new();
        private List<UIBase> uiList = new();
        
        #region << =========== LIFE CYCLE =========== >>
        public void Init()
        {
            if (IsInitialized) return;
            
            Global.Init(this);
            
            DebugHelper.Log(EDebugType.Init, $"🟢 - [ UIManager ] Initialize Completed!");
            IsInitialized = true;
        }

        public void Reset()
        {
            _enrollIndex = -1;
            uiDic.Clear();
            uiList.Clear();
        }

        #endregion

        #region << =========== ENROLL =========== >>
        
        public void EnrollUI(UIBase uiBase)
        {
            if (uiList.Contains(uiBase))
            {
                return;
            }

            _enrollIndex += 1;
             uiBase.SetIndex(_enrollIndex);
            
            uiDic.Add(uiBase.GetIndex(), uiBase);
            uiList.Add(uiBase);
        }

        public void UnenrollUI(UIBase uiBase)
        {
            if (!uiList.Contains(uiBase)) return;

            uiDic.Remove(uiBase.GetIndex());
            uiList.Remove(uiBase);
        }

        #endregion

        #region <<========= GET =========== >>

        public UIBase GetUI(int index)
        {
            if (uiDic.ContainsKey(index))
            {
                return uiDic[index];
            }

            DebugHelper.LogError(EDebugType.UI, $"[ UIManager ] GetUI - Not Found UI : {index}");
            return null;
        }
        
        public UIBase GetUI(string tagName)
        {
            foreach (var ui in uiList)
            {
                if (ui.GetTagName() == tagName)
                {
                    return ui;
                }
            }

            DebugHelper.LogError(EDebugType.UI, $"[ UIManager ] GetUI - Not Found UI with Tag: {tagName}");
            return null;
        }
        
        public T GetUI<T>() where T : UIBase
        {
            int index = GetUIIndex<T>();
            if (index == -1)
            {
                return null;
            }
            
            return GetUI(index) as T;
        }

        public int GetEnrollIndex() => _enrollIndex;
        public int GetUICount() => uiList.Count;
        
        #endregion

        #region << =========== UTIL =========== >>
        
        private int GetUIIndex<T>() where T : UIBase
        {
            foreach (var ui in uiList)
            {
                if (ui is T targetUI)
                {
                    return targetUI.GetIndex();
                }
            }

            DebugHelper.LogError(EDebugType.UI, $"[ UIManager ] GetUIIndex<{typeof(T).Name}> - Not Found");
            return -1;
        }

        public void Localize()
        {
            Global.UtilEvent.Trigger((int)EGlobalType.Localize);
        }
        
        #endregion

        #region << =========== DEBUG =========== >>
        
        public string GetDebugInfo()
        {
            string info = $"[ UIManager Info ]\n" +
                          $"IsInitialized: {IsInitialized}\n" +
                          $"Enroll Index: {_enrollIndex}\n" +
                          $"UI Count: {uiList.Count}";
            
            return info;
        }
        
        public string GetUIListDebugInfo()
        {
            string info = $"[ UI List Info ]\n";

            foreach (var kvp in uiDic.OrderBy(kvp => kvp.Key))
            {
                if(kvp.Value == null) continue;
                
                info += $"- Index: {kvp.Key} - {kvp.Value.GetShortDebugInfo()}\n";
            }

            return info;
        }

        #endregion
    }
}