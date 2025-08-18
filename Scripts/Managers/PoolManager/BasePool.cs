using UnityEngine;

namespace SongLib
{
    public abstract class BasePool
    {
        protected GameObject resourceObj = null;
        
        protected BasePoolGroup poolGroup = null;

        public void SetPoolGroup(BasePoolGroup poolGroup) => this.poolGroup = poolGroup;
        public BasePoolGroup GetPoolGroup() => poolGroup;

        public virtual void Instantiate(GameObject _prefab)
        {
            resourceObj = Object.Instantiate(_prefab);
        }

        public virtual void Active() => resourceObj.SetActive(true);
        public virtual void Inactive() => resourceObj.SetActive(false);
        public virtual void Destroy() => Object.Destroy(resourceObj);

        public GameObject GetGameObject() => resourceObj;

        public void SetPosition(Vector3 pos)
        {
            if (resourceObj == null)
            {
                return;
            }
            resourceObj.transform.position = pos;
        }

        public void SetRotation(Quaternion rot)
        {
            if (resourceObj == null)
            {
                return;
            }
            resourceObj.transform.rotation = rot;
        }
    }
}