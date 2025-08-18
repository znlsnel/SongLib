using System;
using SongLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SongLib
{
    public abstract class BaseObjectManager<U> : SingletonWithMono<U>, IBaseManager, IObjectManager where U : MonoBehaviour, new()
    {
        public bool IsInitialized { get; set; }

        public void Init()
        {
            Global.Init(this);
            
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            
            DebugHelper.Log(DebugType.Init, $"🟢 - [ ObjectManager ] Initialize Completed!");
            IsInitialized = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene arg0) => Reset();
        protected abstract void Reset();

        public GameObject Spawn(string key, bool isPooling = true) => AddObject(key, pooling: isPooling);
        public void Despawn(GameObject obj) => DestroyObject(obj);

        #region << =========== ADD OBJECT =========== >>

        protected GameObject AddObject(string key, string subName = "", bool pooling = false)
        {
            if (pooling)
            {
                return Global.Pool.Pop(key, subName).GetGameObject();
            }

            GameObject prefab = Global.Resource.GetPrefab($"{key}");
            if (prefab == null)
            {
                Debug.LogError($"Failed to load prefab : {key}");
                return null;
            }

            GameObject go = Instantiate(prefab);
            go.name = prefab.name;
            return go;
        }

        protected BasePool AddBasePool(string key)
        {
            return Global.Pool.Pop(key);
        }

        protected GameObject AddObject(string key, Vector3 pos, Vector3 dir, string subName = "", bool pooling = false)
        {
            return AddObject(key, pos, Quaternion.LookRotation(dir), subName, pooling);
        }

        protected GameObject AddObject(string key, Vector3 pos, Quaternion rot, string subName = "", bool pooling = false)
        {
            GameObject obj = AddObject(key, subName, pooling);
            if (obj == null)
            {
                return null;
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.transform.localScale = Vector3.one;

            return obj;
        }

        protected GameObject AddObject(string key, Transform parentTran, string subName = "", bool pooling = false)
        {
            GameObject obj = AddObject(key, subName, pooling);
            if (obj == null)
            {
                return null;
            }

            obj.AttachParentObject(parentTran);

            return obj;
        }

        #endregion

        #region << =========== DESTROY OBJECT =========== >>

        protected void DestroyObject(GameObject go)
        {
            if (go == null)
            {
                return;
            }

            // Pool에 넣을 수 있다면 Pool에 넣고 return
            if (Global.Pool.Push(go))
            {
                return;
            }

            // UnuseResource에 포함되어 있다면 중복 호출이므로 return
            if (Global.Pool.IsContainUnuseResource(go))
            {
                DebugHelper.LogWarning(DebugType.System, "[ObjectManager] DestroyObject - Already Contain UnuseResource");
                return;
            }

            // Pool에 넣을 수 없다면 Destroy ( 오브젝트풀링이 아닌 경우 )
            Destroy(go);
        }

        #endregion
    }
}