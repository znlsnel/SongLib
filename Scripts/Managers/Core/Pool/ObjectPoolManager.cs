using SongLib;
using UnityEngine;

public class ObjectPoolManager: BasePoolManager<ObjectPoolManager>
{ 
    private const string POOL_NAME = "[POOL] ";
    public Transform ParentTran { get; set; }
    
    protected override BasePoolGroup CreatePoolGroup(string resourceName, string subName ="", int maxCount = 0, bool overCreate = true, bool isNotDestroyed = false)
    {
        ObjectPoolGroup poolGroup = new ObjectPoolGroup();
        poolGroup.Init(maxCount, overCreate, isNotDestroyed);
        poolGroup.CreateParent(transform, POOL_NAME + resourceName + subName);
        poolGroups.Add(resourceName+subName, poolGroup);
        return poolGroup;
    }
}