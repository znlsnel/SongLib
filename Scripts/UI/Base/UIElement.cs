using UnityEngine;

namespace SongLib
{
    public abstract class UIElement<T> : UIBase where T : UIBase
    {
        private T _parent;
        public void Setup(T parent)
        {
            _parent = parent;
        }
    }
}
