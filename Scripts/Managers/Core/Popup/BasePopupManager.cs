using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SongLib
{
    /// <summary>
    /// 게임에서는 PopupManager를 상속 받아서 사용 바랍니다.
    /// 게임에서 GAME_UI_ID enum 값은 100번 부터 할당해서 사용 바랍니다.
    /// </summary>
    public abstract class BasePopupManager<U> : SingletonWithMono<U>, IBaseManager, IPopupManager where U : BasePopupManager<U>, new()
    {
        protected PopupSystem _popupSystem;
        
        public bool IsInitialized { get; set; }
        public void Init()
        {
            Global.Init(this);
            _popupSystem = new PopupSystem(transform);
            
            DebugHelper.Log(EDebugType.Init, $"🟢 - [ PopupManager ] Initialize Completed!");
            IsInitialized = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _popupSystem.OnDestroy();
        }

        /// <summary>팝업 시스템 이벤트</summary>
        /// <param name="firstOpenEvent">첫번째 팝업창이 열리면 알려주는 이벤트</param>
        /// <param name="allCloseEvent">팝업창 전체가 닫히면 알려주는 이벤트</param>
        public void SetEvent(Action<UIPopup> firstOpenEvent, Action<UIPopup> allCloseEvent)
        {
            _popupSystem.SetEvent(firstOpenEvent, allCloseEvent);
        }

        /// <summary>안드로이드키 업데이트</summary>
        public bool OnUpdateAndroidKey(EAndroidKeyStateType keyState)
        {
            return _popupSystem.OnUpdateAndroidKey(keyState);
        }

        #region << =========== OPEN POPUP =========== >>

        public UIPopup OpenPopup(int popupID) => _popupSystem.OpenPopup(popupID);
        public T OpenPopup<T>(int popupID) where T : UIPopup => _popupSystem.OpenPopup(popupID) as T;

        #endregion

        #region << =========== CLOSE POPUP =========== >>

        public virtual void ClosePopup(UIPopup uiPopup) => _popupSystem.ClosePopup(uiPopup);
        public void ClosePopup(int popupID) => _popupSystem.ClosePopup(popupID);
        public void AllCloseOpenPopup() => _popupSystem.AllCloseOpenPopup();
        public void ForceCloseAllOpenPopup() => _popupSystem.ForceCloseAllOpenPopup();

        #endregion

        #region << =========== RESERVE POPUP =========== >>
        
        public void SetReserveMode(bool isReserveMode) => _popupSystem.SetReserveMode(isReserveMode);
        public UIPopup ReservePopup(int popupID) => _popupSystem.ReservePopupUI(popupID);
        public UIPopup ReservePopup(int popupID, int prevPopupID) => _popupSystem.ReservePopupUI(popupID, prevPopupID);
        public UIPopup ReservePopup(int popupID, UIPopup prevUIPopup) => _popupSystem.ReservePopupUI(popupID, prevUIPopup);

        #endregion

        #region << =========== GET =========== >>

        /// <summary>팝업창 가져오기</summary>
        public UIPopup GetPopup(int popupID) => _popupSystem.GetPopup(popupID);
        public T GetPopup<T>(int popupID) where T : UIPopup => _popupSystem.GetPopup(popupID) as T;
        public UIPopup GetTopPopup() => _popupSystem.GetTopPopup();

        /// <summary>팝업창이 열려 있는가?</summary>
        public bool IsOpenedPopup() => _popupSystem.IsOpenedPopup();
        public bool IsOpenedPopup(int popupID) => _popupSystem.IsOpenedPopup(popupID);
        
        public (List<UIPopup> popupList, int popupCount) GetPopupList() => _popupSystem.GetPopupList();
        public (List<UIPopup> openPopupList, int openPopupCount) GetOpenPopupList() => _popupSystem.GetOpenPopupList();
        public (List<UIPopup> reservePopupList, int reservePopupCount) GetReservePopupList() => _popupSystem.GetReservePopupList();

        #endregion

        #region << =========== UTIL =========== >>
        
        public void MoveToFront(int popupID) => _popupSystem.MoveToFront(popupID);

        #endregion
        
        /// <summary>
        /// 로딩 팝업 열기 (static 함수를 통해서 호출하면 좋습니다. UI_ID.UI_LOADING을 가진 팝업창 프리팹을 만들어야 합니다.)
        /// </summary>
        /// <param name="isLoadingImg">즉시 로딩 이미지 보여줄 경우 true</param>
        /// <returns></returns>
        protected UIPopupLoading OpenLoading(bool isLoadingImg)
        {
            UIPopupLoading uiPopupLoading = IsOpenedPopup(4) ? (UIPopupLoading)GetPopup(4) : (UIPopupLoading)OpenPopup(4);

            if (uiPopupLoading != null & isLoadingImg)
            {
                uiPopupLoading.SetWaitTime(0.0f);
            }
            
            return uiPopupLoading;
        }

        /// <summary>로딩 팝업 닫기</summary>
        protected void CloseLoading() => ClosePopup(4);

        /// <summary>
        /// 블럭 팝업 열기 (static 함수를 통해서 호출하면 좋습니다. UI_ID.UI_BLOCK을 가진 팝업창 프리팹을 만들어야 합니다.)
        /// </summary>
        protected void OpenBlock()
        {
            if (IsOpenedPopup(5))
            {
                return;
            }
            
            OpenPopup(5);
        }

        /// <summary>블럭 팝업 닫기</summary>
        protected void CloseBlock() => ClosePopup(5);

        #region << =========== DEBUG =========== >>
        
        public string GetPopupDebugInfo()
        {
            StringBuilder popupInfo = new StringBuilder();
            var (popupList, popupCount) = _popupSystem.GetPopupList();
            if (popupCount > 0)
            {
                popupInfo.Append($"Popup Count: {popupCount}\n---------------\n");
                foreach (var popup in popupList)
                {
                    popupInfo.Append($"ID: {popup.GetPopupID()} - {popup.name}\n");
                }
                popupInfo.Append("---------------\n");
            }
            else
            {
                popupInfo.Append("No Popup");
            }
            
            return popupInfo.ToString();
        }
        
        public string GetOpenPopupDebugInfo()
        {
            StringBuilder openPopupInfo = new StringBuilder();
            var (openPopupList, openPopupCount) = _popupSystem.GetOpenPopupList();
            if (openPopupCount > 0)
            {
                openPopupInfo.Append($"Open Popup Count: {openPopupCount}\n---------------\n");
                foreach (var popup in openPopupList)
                {
                    openPopupInfo.Append($"ID: {popup.GetPopupID()} - {popup.name}\n");
                }
                openPopupInfo.Append("---------------\n");
            }
            else
            {
                openPopupInfo.Append("No Open Popup");
            }
            
            return openPopupInfo.ToString();
        }
        
        public string GetTopPopupDebugInfo()
        {
            UIPopup topPopup = _popupSystem.GetTopPopup();
            StringBuilder topPopupInfo = new StringBuilder();

            if (topPopup != null)
            {
                topPopupInfo.Append($"Top Popup ID: {topPopup.GetPopupID()} - {topPopup.name}");
            }
            else
            {
                topPopupInfo.Append("No Top Popup");
            }
            
            return topPopupInfo.ToString();
        }
        
        public string GetReservePopupDebugInfo()
        {
            StringBuilder reservePopupInfo = new StringBuilder();
            var (reservePopupList, reservePopupCount) = _popupSystem.GetReservePopupList();
            if (reservePopupCount > 0)
            {
                reservePopupInfo.Append($"Reserve Popup Count: {reservePopupCount}\n---------------\n");
                foreach (var popup in reservePopupList)
                {
                    reservePopupInfo.Append($"ID: {popup.GetPopupID()} - {popup.name}\n");
                }
                reservePopupInfo.Append("---------------\n");
            }
            else
            {
                reservePopupInfo.Append("No Reserve Popup");
            }
            
            return reservePopupInfo.ToString();
        }

        #endregion
    }
}