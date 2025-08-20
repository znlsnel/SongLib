using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SongLib
{
    public abstract class BaseSceneFlowManager<U> : SingletonWithMono<U>, IBaseManager where U : BaseSceneFlowManager<U>, new()
    {
        protected readonly Dictionary<int, ISceneTransition> sceneTransitionDict = new();
        private ISceneTransition _currentSceneTransition;
        private Coroutine _sceneTransitionCoroutine;

        private float _sceneTransitionStopTime = 1f;

        public bool IsInitialized { get; set; }

        public virtual void Init()
        {
            DebugHelper.Log(EDebugType.Init, $"[ SceneFlowManager ] Initialize Completed!");
            IsInitialized = true;
        }
        
        public void LoadScene(string sceneName, int transitionType = -1)
        {
            ClearTransitionCoroutine();

            Global.UtilPopup?.ForceCloseAllOpenPopup();

            if (transitionType == -1)
            {
                DebugHelper.Log(EDebugType.System, "[ SceneFlowManager ] Using default transition.");
                _currentSceneTransition = null;
            }
            else
            {
                if (sceneTransitionDict.TryGetValue(transitionType, out _currentSceneTransition))
                {
                    DebugHelper.Log(EDebugType.System, $"[ SceneFlowManager ] Using transition type: {transitionType}");
                }
                else
                {
                    _currentSceneTransition = AddPopup(transitionType);
                }   
            }

            _sceneTransitionCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            if (_currentSceneTransition != null)
            {
                bool isDone = false;
                _currentSceneTransition.Open();
                _currentSceneTransition.StartTransition(() => isDone = true);
                yield return new WaitUntil(() => isDone);
            }

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            if (asyncLoad == null)
            {
                Debug.LogError($"Failed to load scene: {sceneName}");
                yield break;
            }
            
            asyncLoad.allowSceneActivation = false;
            
            while (asyncLoad.progress < 0.9f)
            {
                DebugHelper.Log(EDebugType.Info, $"Loading progress: {asyncLoad.progress}");
                yield return null;
            }
            
            asyncLoad.allowSceneActivation = true;

            if (_currentSceneTransition != null)
            {
                bool isDone = false;
                yield return new WaitForSecondsRealtime(_sceneTransitionStopTime);
                _currentSceneTransition.EndTransition(() => isDone = true);
                yield return new WaitUntil(() => isDone);
                _currentSceneTransition.Close();
            }

            _sceneTransitionCoroutine = null;
        }

        private void ClearTransitionCoroutine()
        {
            if (_sceneTransitionCoroutine != null)
            {
                StopCoroutine(_sceneTransitionCoroutine);
                _sceneTransitionCoroutine = null;
            }
        }

        protected abstract ISceneTransition AddPopup(int popupID);
    }
}