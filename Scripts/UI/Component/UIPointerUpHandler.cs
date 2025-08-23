using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SongLib
{
    public class UIPointerUpHandler : MonoBehaviour, IPointerUpHandler
    {
        public Action<PointerEventData> onPointerUp;

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
        }
    }
}