#if SET_ADDRESSABLE
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public class GameAddressableUIPopup : GameAddressable<GameObject>
    {
        protected Dictionary<int, GameObject> popupObjects = new Dictionary<int, GameObject>();

        public override async Awaitable Init(string label = "PreLoad")
        {
            await base.Init(label);

            foreach (var resource in GetAllResource())
            {
                GameObject gameObject = resource.Value;
                UIPopup popupComponent = gameObject.GetComponent<UIPopup>();

                if (popupComponent == null)
                {
                    continue;
                }

                int popupID = popupComponent.GetPopupID();

                if (popupObjects.TryGetValue(popupID, out var popupObject))
                {
                    Debug.LogWarning($"UI Resource Same Key Already Exist: {gameObject.name} ({popupID} - {popupObject.name})");
                    continue;
                }

                popupObjects[popupID] = gameObject;
            }
        }

        public GameObject GetUIPopup(int id)
        {
            return popupObjects.GetValueOrDefault(id);
        }
        
        public void DelUIPopup(int id)
        {
            if (popupObjects.ContainsKey(id))
            {
                popupObjects.Remove(id);
            }
        }
    }
}
#endif