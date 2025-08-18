
using UnityEngine;

namespace SongLib
{
    public interface IPoolManager
    {
        bool IsInitialized { get; set; }
    
        void Init();
        BasePool Pop(string resourceName, string subName = "");
        bool Push(GameObject obj);

        bool IsContainUnuseResource(GameObject obj);
    
        void ClearPool();
    
        void StartPreLoad();
        void CreatePreResource(string resourceName, int maxCount, string subName ="", bool isOverCreate = true, bool isNonDestroyed = false);
        void EndPreLoad();

        void SetAllUnuseResource();
    }
}