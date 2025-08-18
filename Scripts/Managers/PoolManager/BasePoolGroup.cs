using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SongLib
{
    public abstract class BasePoolGroup
    {
        protected List<BasePool> useResources = new();
        protected Stack<BasePool> unuseResources = new();
        
        protected Transform parentTran;
        protected int createMaxCount;
        protected bool isOverCreate = true;

        public bool IsNonDestroyed { get; private set; }
        public bool IsPreLoadDone;

        #region << =========== INIT =========== >>
        
        public virtual void Init(int maxCount, bool isOverCreate, bool isNonDestroyed)
        {
            createMaxCount = maxCount;
            this.isOverCreate = isOverCreate;
            IsNonDestroyed = isNonDestroyed;
            if (maxCount != 0)
            {
                return;
            }
            this.isOverCreate = true;
        }

        #endregion

        #region << =========== CREATE =========== >>

        public void CreateParent(Transform parentTran, string name)
        {
            var gameObject = parentTran.CreateObject(name);
            gameObject.name = name;
            this.parentTran = gameObject.transform;
        }

        protected abstract BasePool CreateResourcePool(string resourceName);

        public void CreatePreResource(string resourceName)
        {
            var num = CalculateRemainingCapacity();
            for (var index = 0; index < num; ++index)
            {
                var resourcePool = CreateResourcePool(resourceName);
                if (resourcePool != null)
                {
                    PushUnuseResource(resourcePool);
                }
            }

            for (var index = num; index < 0; ++index)
            {
                DeleteResourcePool();
            }
            
            IsPreLoadDone = true;
        }


        #endregion

        #region << =========== DESTROY =========== >>

        public virtual void Destroy()
        {
            Object.Destroy(parentTran.gameObject);
        }

        #endregion

        #region << =========== PUSH & POP =========== >>

        // MEMO: 사용하지 않는 리소스가 있으면 사용하고, 없으면 새로 생성한다.
        public BasePool AddResource(string resourceName)
        {
            var unuseResource = GetUnuseResource();
            if (unuseResource != null)
            {
                useResources.Add(unuseResource);
                unuseResource.Active();
                return unuseResource;
            }

            if (!isOverCreate && useResources.Count + unuseResources.Count >= createMaxCount)
            {
                return null;
            }
            
            var resourcePool = CreateResourcePool(resourceName);
            if (resourcePool == null)
            {
                return null;
            }
            useResources.Add(resourcePool);
            resourcePool.Active();
            
            return resourcePool;
        }

        public void PopUseResource(GameObject obj)
        {
            useResources.RemoveAll(resource =>
            {
                if (resource.GetGameObject() == obj)
                {
                    PushUnuseResource(resource);
                    return true;
                }
                return false;
            });
        }

        protected void PushUnuseResource(BasePool pool)
        {
            ResetAndHideObject(pool);
            pool.Inactive();
            unuseResources.Push(pool);
        }

        #endregion

        #region << =========== DELETE =========== >>

        protected void DeleteResourcePool()
        {
            GetUnuseResource()?.Destroy();
        }

        public virtual void AllDeleteResource()
        {
            var count1 = useResources.Count;
            for (var index = 0; index < count1; ++index)
            {
                useResources[index].Destroy();
                useResources[index] = null;
            }

            useResources.Clear();
            var count2 = unuseResources.Count;
            for (var index = 0; index < count2; ++index)
            {
                unuseResources.Pop().Destroy();
            }
            unuseResources.Clear();
        }

        #endregion

        #region << =========== UTIL =========== >>

        public bool IsContainUnuseResource(GameObject obj) => unuseResources.Any(resource => resource.GetGameObject() == obj);
        protected virtual BasePool GetUnuseResource() => unuseResources.Count == 0 ? null : unuseResources.Pop();

        protected void ResetAndHideObject(BasePool pool)
        {
            var transform = pool.GetGameObject().transform;
            transform.SetParent(parentTran);
            transform.localPosition = Vector3.down * 2000f;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        protected int CalculateRemainingCapacity()
        {
            SetAllUnuseResource();
            return createMaxCount - (useResources.Count + unuseResources.Count);
        }

        public void SetAllUnuseResource()
        {
            for (int i = useResources.Count - 1; i >= 0; i--)
            {
                if (useResources[i].GetGameObject() != null)
                {
                    PushUnuseResource(useResources[i]);
                }
            }
            useResources.Clear();
        }
        
        public bool Contains(GameObject obj)
        {
            return useResources.Any(resource => resource.GetGameObject() == obj);
        }

        #endregion
    }
}