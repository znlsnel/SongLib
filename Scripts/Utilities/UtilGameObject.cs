using UnityEngine;


public static class UtilGameObject
{
    public static void AttachParentObject(this GameObject child, Transform parent, bool isInit = true)
    {
        child.transform.AttachParentObject(parent, isInit);
    }

    public static GameObject FindChild(this GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject CreateObject(this GameObject prefab, Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("[GeekFunction] The prefab is null.");
            return null;
        }

        GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
        gameObject.transform.AttachParentObject(parent);
        return gameObject;
    }

    public static GameObject CreateObject(this GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("[GeekFunction] The prefab is null.");
            return null;
        }
        
        GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localRotation = Quaternion.identity;
        return gameObject;
    }
}