using System;
using UnityEngine;
using System.Collections;

namespace SongLib
{
    public abstract class BaseSceneManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool IsInitialized { get; private set; } = false;
        
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            Instance = this as T;
        }
        
        protected virtual void Start()
        {
            if (BaseGameManager.Instance.IsInitialized)
            {
                Initialize();
            }
            else
            {
                StartCoroutine(InitalizeAsync());
            }
        }

        private IEnumerator InitalizeAsync()
        {
            yield return new WaitUntil(() => BaseGameManager.Instance.IsInitialized);
            Initialize();
        }

        private void Initialize()
        {
            DebugHelper.Log(DebugType.Init, $"🔵 - [ {typeof(T).Name} ] Initialize Completed!");
            IsInitialized = true;
            Init();
        }
        protected abstract void Init();
    }
}