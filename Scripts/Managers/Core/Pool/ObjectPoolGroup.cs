using System;
using UnityEngine;
using SongLib;

public class ObjectPoolGroup : BasePoolGroup
{
    protected override BasePool CreateResourcePool(string resourceName)
    {
        GameObject prefab = Global.UtilResource.GetPrefab(resourceName);
        if (prefab == null)
        {
            return null;
        }

        ObjectPool resPool = new ObjectPool();
        resPool.Instantiate(prefab);
        resPool.SetPoolGroup(this);
        ResetAndHideObject(resPool);
        return resPool;
    }

    public GameObject AddObjectPool(string resourceName)
    {
        return AddResource(resourceName)?.GetGameObject();
    }
}