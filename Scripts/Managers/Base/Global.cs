

using System.Collections.Generic;

namespace SongLib
{
    public static class Global
    {
        #region << ========== MANAGERS ========== >>
        private static IEventManager _eventManager;
        private static IPopupManager _popupManager;
        private static IResourceManager _resourceManager;
        private static IAudioManager _audioManager;
        private static IPoolManager _poolManager;
        private static IUIManager _uiManager;
        private static IObjectManager _objectManager;
        private static ITimeManager _timeManager;
        #endregion

        #region << ========== UTIL MANAGERS PROPERTIES ========== >>
        public static IEventManager UtilEvent => _eventManager;
        public static IPopupManager UtilPopup => _popupManager;
        public static IResourceManager UtilResource => _resourceManager;
        public static IAudioManager UtilAudio => _audioManager;
        public static IPoolManager UtilPool => _poolManager;
        public static IUIManager UtilUI => _uiManager;
        public static IObjectManager UtilObject => _objectManager;
        public static ITimeManager UtilTime => _timeManager;
        #endregion

        #region << ========== MANAGERS PROPERTIES ========== >>
        public static IEventManager Event => _eventManager;
        public static IPopupManager Popup => _popupManager;
        public static IResourceManager Resource => _resourceManager;
        public static IAudioManager Audio => _audioManager;
        public static IPoolManager Pool => _poolManager;
        public static IUIManager UI => _uiManager;
        public static IObjectManager Object => _objectManager;
        public static ITimeManager Time => _timeManager;
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
        public static void Init(IEventManager eventManager) => _eventManager = eventManager;
        public static void Init(IPopupManager popupManager) => _popupManager = popupManager;
        public static void Init(IResourceManager resourceManager) => _resourceManager = resourceManager;
        public static void Init(IAudioManager audioManager) => _audioManager = audioManager;
        public static void Init(IPoolManager poolManager) => _poolManager = poolManager;
        public static void Init(IUIManager uiManager) => _uiManager = uiManager;
        public static void Init(IObjectManager objectManager) => _objectManager = objectManager;
        public static void Init(ITimeManager timeManager) => _timeManager = timeManager;
        #endregion
    }
}