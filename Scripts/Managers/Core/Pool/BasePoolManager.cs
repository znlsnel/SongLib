using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SongLib
{
    public abstract class BasePoolManager<T> : SingletonWithMono<T>, IBaseManager, IPoolManager where T : MonoBehaviour, new()
    {
        protected Dictionary<string, BasePoolGroup> poolGroups = new Dictionary<string, BasePoolGroup>();
        protected bool isPreLoadingReadyDone = false;

        protected abstract BasePoolGroup CreatePoolGroup(string resourceName, string subName ="", int maxCount = 0, bool isOverCreate = true, bool isNotDestroyed = false);
        
        public bool IsInitialized { get; set; }
        public void Init()
        {
            Global.Init(this);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            DebugHelper.Log(EDebugType.Init, $"🟢 - [ {typeof(T).Name} ] Initialize Completed!");
            IsInitialized = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            ClearPool();
        }
        
        #region << =========== POP & PUSH =========== >>

        public BasePool Pop(string resourceName, string subName = "")
        {
            if (resourceName.IsNullOrWhiteSpace())
            {
                return null;
            }

            BasePoolGroup poolGroup = GetPoolGroup(resourceName,subName) ?? CreatePoolGroup(resourceName,subName);
            return poolGroup.AddResource(resourceName);
        }
        
        public bool Push(GameObject obj)
        {
            foreach (var poolGroup in poolGroups.Values)
            {
                if (poolGroup.Contains(obj))
                {
                    poolGroup.PopUseResource(obj);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region << =========== Clear =========== >>

        public void ClearPool()
        {
            DebugHelper.Log(EDebugType.System, $"[{typeof(T).Name}] Clearing Object Pool...");
            
            foreach (var poolGroup in poolGroups.Values)
            {
                poolGroup.AllDeleteResource();
            }

            Resources.UnloadUnusedAssets();
        }
        
        #endregion

        #region << =========== PRELOAD =========== >>

        public void StartPreLoad()
        {
            isPreLoadingReadyDone = true;
    
            foreach (var poolGroup in poolGroups.Values)
            {
                poolGroup.IsPreLoadDone = false;
            }
        }

        public void CreatePreResource(string resourceName, int maxCount, string subName ="", bool isOverCreate = true, bool isNonDestroyed = false)
        {
            if (!isPreLoadingReadyDone)
            {
                Debug.LogError("PreLoading Not Ready!!!! Please StartPreLoad() Call");
            }
            else
            {
                BasePoolGroup basePoolGroup = GetPoolGroup(resourceName);
                
                if (basePoolGroup == null)
                {
                    basePoolGroup = CreatePoolGroup(resourceName, subName: subName, maxCount, isOverCreate, isNonDestroyed);
                }
                else
                {
                    basePoolGroup.Init(maxCount, isOverCreate, isNonDestroyed);
                }
                basePoolGroup.CreatePreResource(resourceName);
            }
        }

        public void EndPreLoad()
        {
            isPreLoadingReadyDone = false;
            List<string> list = poolGroups.Keys.ToList<string>();
            int count = list.Count;
            for (int index = 0; index < count; ++index)
            {
                if (!poolGroups[list[index]].IsPreLoadDone && !poolGroups[list[index]].IsNonDestroyed)
                {
                    poolGroups[list[index]].Destroy();
                    SingletonWithMono<AssetBundleResourceManager>.Instance.UnloadUnitAssetBundle(list[index]);
                    poolGroups.Remove(list[index]);
                }
            }

            Resources.UnloadUnusedAssets();
        }

        #endregion

        #region << =========== UTIL =========== >>

        protected BasePoolGroup GetPoolGroup(string resourceName, string subName ="") => poolGroups.GetValueOrDefault(resourceName+subName);

        public void SetAllUnuseResource()
        {
            foreach (var poolGroup in poolGroups.Values)
            {
                poolGroup.SetAllUnuseResource();
            }
        }
        
        public bool IsContainUnuseResource(GameObject obj)
        {
            foreach (var poolGroup in poolGroups.Values)
            {
                if (poolGroup.IsContainUnuseResource(obj))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}