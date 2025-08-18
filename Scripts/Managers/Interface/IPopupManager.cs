using System;
using System.Collections.Generic;

namespace SongLib
{
    public interface IPopupManager
    {
        bool IsInitialized { get; set; }
        void Init();

        void SetEvent(Action<UIPopup> firstOpenEvent, Action<UIPopup> allCloseEvent);

        bool OnUpdateAndroidKey(EAndroidKeyStateType keyState);

        UIPopup OpenPopup(int popupID);
        T OpenPopup<T>(int popupID) where T : UIPopup;
        void ClosePopup(UIPopup uiPopup);
        void ClosePopup(int popupID);
        void AllCloseOpenPopup();
        void ForceCloseAllOpenPopup();

        void SetReserveMode(bool isReserveMode);
        UIPopup ReservePopup(int popupID);
        UIPopup ReservePopup(int popupID, int prevPopupID);
        UIPopup ReservePopup(int popupID, UIPopup prevUIPopup);

        void MoveToFront(int popupID);

        UIPopup GetPopup(int popupID);
        UIPopup GetTopPopup();
        T GetPopup<T>(int popupID) where T : UIPopup;

        bool IsOpenedPopup();
        bool IsOpenedPopup(int popupID);

        (List<UIPopup> popupList, int popupCount) GetPopupList();
        (List<UIPopup> openPopupList, int openPopupCount) GetOpenPopupList();
        (List<UIPopup> reservePopupList, int reservePopupCount) GetReservePopupList();
        
        string GetPopupDebugInfo();
        string GetOpenPopupDebugInfo();
        string GetTopPopupDebugInfo();
        string GetReservePopupDebugInfo();

    }
}