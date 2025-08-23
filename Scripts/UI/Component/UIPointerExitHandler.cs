using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SongLib
{
    public class UIPointerExitHandler : MonoBehaviour, IPointerExitHandler
    {
        public Action<PointerEventData> onPointerExit;

        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke(eventData);
        }
    }
}