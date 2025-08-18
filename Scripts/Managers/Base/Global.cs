

using System.Collections.Generic;

namespace SongLib
{
    public static class Global
    {
        #region << ========== MANAGERS PROPERTIES ========== >>
        public static IEventManager Event { get; private set; }
        public static IPopupManager Popup { get; private set; }
        public static IResourceManager Resource { get; private set; }
        public static IAudioManager Audio { get; private set; }
        public static IPoolManager Pool { get; private set; }
        public static IUIManager UI { get; private set; }
        public static IObjectManager Object { get; private set; }
        public static ITimeManager Time { get; private set; }
        #endregion

        #region << ========== SETUP ========== >>
        public static bool IsInitialized => BaseGameManager.Instance.IsInitialized;

        public static List<IBaseManager> Managers;

        public static void SetManagers(List<IBaseManager> managers)
        {
            Managers = managers;
        }
        #endregion
        
        #region << ========== INIT MANAGERS ========== >>
        public static void Init(IEventManager eventManager) => Event = eventManager;
        public static void Init(IPopupManager popupManager) => Popup = popupManager;
        public static void Init(IResourceManager resourceManager) => Resource = resourceManager;
        public static void Init(IAudioManager audioManager) => Audio = audioManager;
        public static void Init(IPoolManager poolManager) => Pool = poolManager;
        public static void Init(IUIManager uiManager) => UI = uiManager;
        public static void Init(IObjectManager objectManager) => Object = objectManager;
        public static void Init(ITimeManager timeManager) => Time = timeManager;
        #endregion
    }
}