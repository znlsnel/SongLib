using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public class PopupControll : Singleton<PopupControll>
    {
        private readonly List<UIPopup> _openPopupList = new List<UIPopup>();

        public void OpenPopupUI(UIPopup uiPopup)
        {
            Vector3 pos;
            int depth;
            
            GetNextPosAndDepth(out pos, out depth, uiPopup);
            uiPopup.SetPosAndDepth(pos, depth);
            
            _openPopupList.Add(uiPopup);
        }

        public void ClosePopupUI(UIPopup uiPopup)
        {
            for (int index = _openPopupList.Count - 1; index >= 0; --index)
            {
                if (_openPopupList[index] == uiPopup)
                {
                    _openPopupList.RemoveAt(index);
                }
            }
        }

        private void GetNextPosAndDepth(out Vector3 pos, out int depth, UIPopup uiPopup)
        {
            if (_openPopupList.Count > 0)
            {
                int count = _openPopupList.Count;
                pos = _openPopupList[count - 1].GetNextUIPos();
                depth = _openPopupList[count - 1].GetDepth() + 5;
            }
            else
            {
                pos = Vector3.down * 5f * uiPopup.GetCameraSize();
                depth = GetStartDepth();
            }
        }

        protected virtual int GetStartDepth() => 100;
    }
}