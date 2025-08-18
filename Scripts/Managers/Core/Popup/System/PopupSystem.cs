using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SongLib
{
    public class PopupSystem
    {
        private Transform _tran;
        private UIPopup _topOpenPopup = null;

        private readonly List<UIPopup> _openPopupList = new List<UIPopup>();
        private readonly List<UIPopup> _popupList = new List<UIPopup>();
        private readonly List<UIPopup> _reservePopupList = new List<UIPopup>();

        private Action<UIPopup> _firstOpenEvent;
        private Action<UIPopup> _allCloseEvent;

        public PopupSystem(Transform tran) => _tran = tran;

        public void OnDestroy()
        {
            if (_openPopupList == null) return;
            
            int count = _openPopupList.Count;
            for (int index = 0; index < count; ++index)
            {
                PopupControll.Instance.ClosePopupUI(_openPopupList[index]);
            }
        }

        /// <summary>팝업 시스템 이벤트</summary>
        /// <param name="firstOpenEvent">첫번째 팝업창이 열리면 알려주는 이벤트</param>
        /// <param name="allClose">팝업창 전체가 닫히면 알려주는 이벤트</param>
        public void SetEvent(Action<UIPopup> firstOpenEvent, Action<UIPopup> allClose)
        {
            _firstOpenEvent = firstOpenEvent;
            _allCloseEvent = allClose;
        }

        /// <summary>안드로이드키 업데이트</summary>
        public bool OnUpdateAndroidKey(AndroidKeyStateType _keyState)
        {
            if (!(_topOpenPopup != null))
                return false;
            _topOpenPopup.OnUpdateAndroidKey(_keyState);
            return true;
        }

        #region << =========== SPAWN POPUP =========== >>

        private UIPopup SpawnPopup(int popupID)
        {
            GameObject uiPopupPrefab = Global.UtilResource.GetUIPopupPrefab(popupID);
            
            if (uiPopupPrefab == null)
            {
                Debug.LogError($"[PopupSystem] Popup prefab not found for ID: {popupID}");
                return null;
            }
            
            UIPopup uiPopup = uiPopupPrefab.CreateObject(_tran).GetComponent<UIPopup>();
            uiPopup.SetInternalEvent(OnClosePopup);
            uiPopup.Init();
            
            uiPopup.SetActive(false);
            
            _popupList.Add(uiPopup);
            
            return uiPopup;
        }

        #endregion

        #region << =========== OPEN POPUP =========== >>
        
        /// <summary>팝업창을 열기</summary>
        public UIPopup OpenPopup(int popupID)
        {
            if(_isReserveMode) return ReservePopupUI(popupID);
            
            UIPopup uiPopup = GetPopup(popupID);
            if (uiPopup == null)
            {
                uiPopup = SpawnPopup(popupID);
                if (uiPopup == null)
                {
                    return null;
                }
            }

            return OpenPopup(uiPopup);
        }

        protected virtual UIPopup OpenPopup(UIPopup uiPopup)
        {
            if (_topOpenPopup == null && _firstOpenEvent != null)
            {
                _firstOpenEvent(uiPopup);
            }

            PopupControll.Instance.OpenPopupUI(uiPopup);
            uiPopup.Open();
            
            _openPopupList.Remove(uiPopup);
            _openPopupList.Add(uiPopup);
            _topOpenPopup = uiPopup;
            
            return uiPopup;
        }
        
        #endregion
        
        #region << =========== CLOSE POPUP =========== >>
        
        // TODO:minb - close popup 관련 리팩토링 필요
        /// <summary>팝업창을 닫기</summary>
        public void ClosePopup(int popupID)
        {
            UIPopup uiPopup = GetPopup(popupID);
            ClosePopup(uiPopup);
        }
        
        public void ClosePopup(UIPopup uiPopup)
        {
            if (uiPopup == null || !uiPopup.IsOpened) return;
            
            uiPopup.Close();
            OnClosePopup(uiPopup);
        }

        /// <summary>팝업창을 닫기</summary>
        public virtual void OnClosePopup(UIPopup getPopup)
        {
            if (getPopup == null) return;
            
            RemoveOpenPopupUI(getPopup);
            PopupControll.Instance.ClosePopupUI(getPopup);
            
            bool flag = true;
            UIPopup uiPopup = null;
            
            int count = _reservePopupList.Count;
            for (int index = 0; index < count; ++index)
            {
                if (_reservePopupList[index].PrevPopupID == getPopup.GetPopupID())
                {
                    if (flag)
                    {
                        OpenPopup(_reservePopupList[index]);
                        uiPopup = _reservePopupList[index];
                        flag = false;
                    }
                    else if (uiPopup != null)
                        _reservePopupList[index].PrevPopupID = uiPopup.PrevPopupID;
                }
            }

            if (uiPopup != null)
            {
                _reservePopupList.Remove(uiPopup);
            }
            
            _topOpenPopup = null;
            
            if (_openPopupList.Count > 0)
            {
                _topOpenPopup = _openPopupList[_openPopupList.Count - 1];
            }
            else
            {
                if (_allCloseEvent == null)
                {
                    return;
                }
                
                _allCloseEvent(getPopup);
            }
        }

        private void RemoveOpenPopupUI(UIPopup uiPopup)
        {
            for (int index = _openPopupList.Count - 1; index >= 0; --index)
            {
                UIPopup tempPopup = _openPopupList[index];
                if (tempPopup == uiPopup)
                {
                    _openPopupList.Remove(tempPopup);
                }
            }
        }

        /// <summary>열려 있는 모든 팝업창을 즉시 닫는다.</summary>
        public void AllCloseOpenPopup()
        {
            for (int index = _openPopupList.Count - 1; index >= 0; --index)
            {
                UIPopup _popup = _openPopupList[index];
                if (!(_popup == null))
                {
                    _popup.Close();
                    _openPopupList.Remove(_popup);
                    Singleton<PopupControll>.Instance.ClosePopupUI(_popup);
                }
            }
        }
        
        /// <summary>열려 있는 모든 팝업창을 애니메이션과 효과 없이 즉시 닫는다.</summary>
        public void ForceCloseAllOpenPopup()
        {
            for (int index = _openPopupList.Count - 1; index >= 0; --index)
            {
                UIPopup _popup = _openPopupList[index];
                if (!(_popup == null))
                {
                    _popup.ForceClose();
                    _openPopupList.Remove(_popup);
                    Singleton<PopupControll>.Instance.ClosePopupUI(_popup);
                }
            }
        }
        
        #endregion
        
        #region << =========== RESERVE POPUP =========== >>

        // TODO:minb - 예약 모드 관련 로직 개편이 필요해서 아직 사용X
        // 팝업 방식에 대한 고민이 많음
        private bool _isReserveMode = false;
        public void SetReserveMode(bool isReserveMode)
        {
            _isReserveMode = isReserveMode;
            
            if(!_isReserveMode)
            {
                OpenReservedPopup();
            }
            else
            {
                _reservePopupList.Clear();
            }
        }
        
        private void OpenReservedPopup()
        {
            if (_reservePopupList.Count == 0 || _topOpenPopup != null) return;

            var popup = _reservePopupList[0];
            _reservePopupList.RemoveAt(0);
            
            OpenPopup(popup);
        }
        
        /// <summary>예약 팝업</summary>
        public UIPopup ReservePopupUI(int popupID)
        {
            UIPopup uiPrevPopup = null;

            if (_reservePopupList.Count > 0)
            {
                uiPrevPopup = _reservePopupList[^1];
            }
            
            for (int index = _openPopupList.Count - 1; index >= 0; --index)
            {
                if (_openPopupList[index].GetPopupID() != 4 && _openPopupList[index].GetPopupID() != 5)
                {
                    uiPrevPopup = _openPopupList[index];
                    break;
                }
            }

            return ReservePopupUI(popupID, uiPrevPopup);
        }

        /// <summary>예약 팝업</summary>
        public UIPopup ReservePopupUI(int popupID, int prevPopupID)
        {
            UIPopup prevPopup = GetOpenPopup(prevPopupID);
            
            return ReservePopupUI(popupID, prevPopup);
        }

        /// <summary>예약 팝업</summary>
        public UIPopup ReservePopupUI(int popupID, UIPopup prevUIPopup)
        {
            if (prevUIPopup == null && !_isReserveMode) return OpenPopup(popupID);
            
            UIPopup uiPopup = GetPopup(popupID);
            if (uiPopup == null)
            {
                uiPopup = SpawnPopup(popupID);
                if (uiPopup == null)
                {
                    return null;
                }
            }
            
            DebugHelper.Log(DebugType.UI, $"PopupUD = {popupID}, prevPopupID = {prevUIPopup?.GetPopupID()}");
            if (prevUIPopup != null)
            {
                uiPopup.PrevPopupID = prevUIPopup.GetPopupID();
            }
            
            _reservePopupList.Add(uiPopup);
            
            return uiPopup;
        }

        #endregion

        #region << =========== GET POPUP =========== >>
        
        /// <summary>열려있거나 열렸던 팝업창 클래스 가져오기</summary>
        public UIPopup GetPopup(int _nUIID)
        {
            int count = _popupList.Count;
            for (int index = 0; index < count; ++index)
            {
                UIPopup popup = _popupList[index];
                if (popup.GetPopupID() == _nUIID)
                    return popup;
            }

            return null;
        }

        /// <summary>열려있는 팝업창 클래스 가져오기</summary>
        public UIPopup GetOpenPopup(int _nUIID)
        {
            int count = _openPopupList.Count;
            for (int index = 0; index < count; ++index)
            {
                UIPopup openPopup = _openPopupList[index];
                if (openPopup.GetPopupID() == _nUIID)
                    return openPopup;
            }

            return null;
        }
        
        public List<UIPopup> GetUIPopups() => _popupList;
        public UIPopup GetTopPopup() => _topOpenPopup;
        public bool IsOpenedPopup() => _openPopupList.Count > 0;

        public bool IsOpenedPopup(int popupID)
        {
            UIPopup popup = GetPopup(popupID);
            return popup != null && popup.IsOpened;
        }

        public (List<UIPopup> popupList, int openPopupCount) GetPopupList() => (_popupList, _popupList.Count);
        public (List<UIPopup> openPopupList, int openPopupCount) GetOpenPopupList() => (_openPopupList, _openPopupList.Count);
        public (List<UIPopup> reservePopupList, int reservePopupCount) GetReservePopupList() => (_reservePopupList, _reservePopupList.Count);
        
        #endregion

        #region << =========== UTILS =========== >>
        
        /// <summary>팝업 순서 바꾸기</summary>
        public void MoveToFront(int popupID)
        {
            int count = _openPopupList.Count;
            for (int index = 0; index < count; ++index)
            {
                UIPopup uiPopup = _openPopupList[index];
                if (uiPopup.GetPopupID() == popupID)
                {
                    PopupControll.Instance.OpenPopupUI(uiPopup);
                    _openPopupList.RemoveAt(index);
                    _openPopupList.Add(uiPopup);
                    _topOpenPopup = uiPopup;
                    break;
                }
            }
        }
        
        /// <summary>팝업창 등록(외부에서 이미 생성된 팝업창 )</summary>
        public void OnRegisterPopupUI(UIPopup _uiPopup)
        {
            _uiPopup.SetInternalEvent(OnClosePopup);
            _uiPopup.SetActive(false);
            _popupList.Add(_uiPopup);
        }
        
        #endregion
    }
}