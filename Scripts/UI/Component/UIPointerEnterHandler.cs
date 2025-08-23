using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SongLib
{
    public class UIPointerEnterHandler : MonoBehaviour, IPointerEnterHandler
    {
        public Action<PointerEventData> onPointerEnter;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke(eventData);
        }
    }
}