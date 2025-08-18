using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SongLib
{
    /// <summary>
    /// BaseGameManager를 상속받는 GameManager를 만들어주세요.
    /// GameManager 오브젝트를 생성하여 모든 씬에 배치해주세요.
    /// 그리고 Awake 함수에 필요한 매니저를 등록해줘야합니다.
    /// </summary>
    public abstract class BaseGameManager : MonoBehaviour
    {
        protected List<IBaseManager> _managers = new List<IBaseManager>();
        
        public bool IsInitialized { get; private set; } = false;
        
        public static BaseGameManager Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;

            AddManagers();
        }

        protected abstract void AddManagers();
        
        private void Start()
        {
            DebugManager.Instance.Init();
            CoroutineManager.Instance.Init();
            
            DebugHelper.Log(DebugType.Init, $"🔵 - [ {GetType().Name} ] Initialize Start!");
            
            Global.SetManagers(_managers);

            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            yield return StartCoroutine(InitializeManagers());
            InitializeManagerForce();
            InitializeCompleted();
        }

        private IEnumerator InitializeManagers()
        {
            yield return null;
            foreach (var manager in _managers)
            {
                manager.Init();
                yield return new WaitUntil(() => manager.IsInitialized);
            }
        }

        protected abstract void InitializeManagerForce();

        private void InitializeCompleted()
        {
            DebugHelper.Log(DebugType.Init, $"🔵 - [ {GetType().Name} ] Initialize Completed!");
            IsInitialized = true;
            OnInit();
        }

        protected abstract void OnInit();
    }
}
