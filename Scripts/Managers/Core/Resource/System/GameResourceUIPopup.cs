using System;
using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public class GameResourceUIPopup : GameResource<GameObject>
    {
        protected Dictionary<int, GameObject> popupObjects = new Dictionary<int, GameObject>();

        public GameResourceUIPopup(string pathName, string extension = "") : base(pathName, extension)
        {
        }

        public virtual void Init()
        {
            if (popupObjects.Count > 0) return;

            AllResourceLoad();
            Dictionary<string, GameObject>.Enumerator enumerator = GetAllResource().GetEnumerator();
            while (enumerator.MoveNext())
            {
                GameObject gameObject1 = enumerator.Current.Value;
                UIPopup component = gameObject1.GetComponent<UIPopup>();
                if (component != null)
                {
                    try
                    {
                        popupObjects.Add(component.GetPopupID(), gameObject1);
                    }
                    catch (Exception ex)
                    {
                        GameObject gameObject2;
                        popupObjects.TryGetValue(component.GetPopupID(), out gameObject2);
                        DebugHelper.LogWarning(DebugType.UI, $"UI Resource Same Key Already Exist : {gameObject1.name} ({component.GetPopupID()} - {gameObject2.name})");
                        Debug.LogError(ex);
                    }
                }
            }
        }

        public GameObject GetPopUpUI(int id)
        {
            if (popupObjects.ContainsKey(id))
            {
                return popupObjects[id];
            }

            DebugHelper.LogWarning(DebugType.UI, $"[ GameResourceUIPopup ] GetPopUpUI Fail : {id}");
            return null;
        }
        
        public void DelPopupUI(int id)
        {
            if (popupObjects.ContainsKey(id))
            {
                popupObjects.Remove(id);
            }
        }
    }
}