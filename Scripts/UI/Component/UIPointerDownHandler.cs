using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SongLib
{
    public class UIPointerDownHandler : MonoBehaviour, IPointerDownHandler
    {
        public Action<PointerEventData> onPointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
        }
    }
}