using System;
using UnityEngine;
using System.Collections;

namespace SongLib
{
    public abstract class BaseSceneManager<T> : SingletonWithScene<T> where T : MonoBehaviour
    {
        public bool IsInitialized { get; private set; } = false;
        
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
            DebugHelper.Log(EDebugType.Init, $"🔵 - [ {typeof(T).Name} ] Initialize Completed!");
            IsInitialized = true;
            Init();
        }
        protected abstract void Init();
    }
}