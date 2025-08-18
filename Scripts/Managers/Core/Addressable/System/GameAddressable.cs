#if SET_ADDRESSABLE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace SongLib
{
    public class GameAddressable<T> where T : Object
    {
        protected Dictionary<string, T> resources = new Dictionary<string, T>();
        protected bool isAllLoaded = false;

        #region << =========== INIT =========== >>

        public virtual async Awaitable Init(string label = "PreLoad")
        {
            if (isAllLoaded)
            {
                return;
            }
            
            isAllLoaded = true;
            T[] allResources =  await LoadAllResourcesByLabel(label);
            foreach (T resource in allResources)
            {
                AddResource(resource.name, resource);
            }
            
            DebugHelper.Log(DebugTag.Loading, $"[GameResource] Init : label = {label}, Count = {GetCount()}");
        }

        #endregion

        #region << =========== LOAD =========== >>

        private async Awaitable<T[]> LoadAllResourcesByLabel(string label = "PreLoad")
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
            var locations = await operationHandle.Task;

            List<T> resourceObjects = new List<T>();

            if (locations != null && locations.Count > 0)
            {
                List<Task<T>> loadTasks = new List<Task<T>>();

                foreach (var location in locations)
                {
                    var resourceHandle = Addressables.LoadAssetAsync<T>(location.PrimaryKey);
                    loadTasks.Add(resourceHandle.Task);
                }

                // MEMO: Task.WhenAll()은 모든 Task가 완료될 때까지 대기
                T[] resources = await Task.WhenAll(loadTasks);

                foreach (var resource in resources)
                {
                    if (resource != null)
                    {
                        resourceObjects.Add(resource);
                    }
                }
            }
            
            Addressables.Release(operationHandle);
            return resourceObjects.ToArray();
        }

        private T LoadResource(string fileName)
        {
            T resourceObject = FindResource(fileName);
            if (resourceObject != null)
            {
                return resourceObject;
            }
        
            // MEMO: 동기적으로 실행, 메인 스레드 멈춤
            var resourceHandle = Addressables.LoadAssetAsync<T>(fileName);
            T resource = resourceHandle.WaitForCompletion();

            if (resource == null)
            {
                DebugHelper.LogWarning(DebugTag.Loading, $"[GameResource] LoadResource Fail (Null) : {fileName}");
                return default;
            }

            return resource;
        }

        #endregion
        
        #region << =========== DESTROY =========== >>
        
        public void DelResource(string fileName)
        {
            DebugHelper.Log(DebugTag.Loading, fileName);
            if (fileName.IsNullOrEmpty())
            {
                return;
            }
            
            Destroy(fileName);
        }

        public void DestroyAll()
        {
            isAllLoaded = false;

            foreach (var resource in resources.Values)
            {
                Object.DestroyImmediate(resource);
            }

            resources.Clear();
        }

        private void Destroy(string name)
        {
            T resource = FindResource(name);
            if (resource == null)
            {
                return;
            }
            Object.DestroyImmediate(resource);
            DebugHelper.Log(DebugTag.System, ("Destroy " + name));
            resources.Remove(name);
        }

        #endregion

        #region << =========== GET =========== >>

        public T GetResource(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            return LoadResource(fileName);
        }

        #endregion

        #region << =========== UTIL =========== >>

        protected void AddResource(string key, T resource)
        {
            if (ContainsKey(key))
            {
                return;
            }
            
            resources.Add(key, resource);
        }

        protected virtual T FindResource(string key)
        {
            if (resources.TryGetValue(key, out T resource))
            {
                return resource;
            }
            return default;
        }

        private bool ContainsKey(string key) => key != null && resources.ContainsKey(key);

        public Dictionary<string, T> GetAllResource() => resources;

        public string[] GetResourceNames() => resources.Keys.ToArray<string>();

        public int GetCount() => resources.Count;

        #endregion
    }
}
#endif