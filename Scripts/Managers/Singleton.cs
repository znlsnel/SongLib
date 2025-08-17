using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SongLib
{
    /// <summary>
    /// 전역적으로 접근할 수 있는 일반 싱글톤 클래스
    /// </summary>
    public abstract class Singleton<T> where T : class, new()
    {
        protected static T _instance;
        private static readonly object _lock = new object();
        private static bool applicationIsQuitting;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] '{typeof(T)}' 인스턴스가 애플리케이션 종료 시 이미 파괴되었습니다. null을 반환합니다.");
                    return null;
                }

                lock (_lock)
                {
                    return _instance ??= new T();
                }
            }
        }

        public static bool IsCreatedInstance() => _instance != null;
        public static void OnApplicationQuit() => applicationIsQuitting = true;
    }

    /// <summary>
    /// 전역적으로 접근할 수 있는 MonoBehaviour 싱글톤 클래스
    /// </summary>
    public class SingletonWithMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool applicationIsQuitting;

        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
             //       DebugHelper.LogWarning(DebugType.System, $"[Singleton] '{typeof(T)}' 인스턴스가 애플리케이션 종료 시 이미 파괴되었습니다. null을 반환합니다.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindInstanceInScene();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject($"{typeof(T)} (Singleton)");
                            _instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject);
                        }
                        else
                        {
                            DontDestroyOnLoad(_instance.gameObject);
                   //         DebugHelper.Log(DebugType.System, $"[Singleton] 기존 인스턴스를 사용합니다: {_instance.gameObject.name}");
                        }
                    }

                    return _instance;
                }
            }
        }

        private static T FindInstanceInScene()
        {
            foreach (var rootObj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var instance = rootObj.GetComponentInChildren<T>();
                if (instance != null)
                    return instance;
            }

            return null;
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
               // DebugHelper.LogWarning(DebugType.System, $"[Singleton] 중복된 인스턴스가 발견되었습니다. 기존 인스턴스를 유지하고 새 오브젝트를 삭제합니다: {gameObject.name}");
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        public static bool IsInited() => _instance != null;
        public static bool IsApplicationQuitting() => applicationIsQuitting;
    }

    /// <summary>
    /// 씬 내에서 유지되는 싱글톤 클래스입니다.
    /// </summary>
    public class SingletonWithScene<T> : MonoBehaviour where T : MonoBehaviour
    {
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
    }
    

    /// <summary>
    /// 전역적으로 접근할 수 있는 ScriptableObject 싱글통 클래스
    /// 저장 위치를 항상 Resources/Assets 폴더에 둬야함
    /// </summary>
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load($"Assets/{typeof(T).Name}") as T;
                    if (_instance == null)
                    {
                        _instance = CreateInstance<T>();
                    }
                }

                return _instance;
            }
        }
    }
}
