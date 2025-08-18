using UnityEngine;

namespace SongLib
{
    public interface IObjectManager
    {
        public GameObject Spawn(string key, bool isPooling = true);
        public void Despawn(GameObject obj);
    }
}