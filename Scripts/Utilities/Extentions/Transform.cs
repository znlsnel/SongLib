using UnityEngine;


public static class TransformExtensions
{
    public static void AttachParentObject(this Transform child, Transform parent, bool isInit = true)
    {
        child.SetParent(parent);
        if (!isInit) return;

        RectTransform component = child.GetComponent<RectTransform>();
        if (component != null)
            component.anchoredPosition = Vector2.zero;
        else
            child.localPosition = Vector3.zero;

        child.localScale = Vector3.one;
        child.localRotation = Quaternion.identity;
    }
        
    public static GameObject CreateObject(this Transform parent, string objectName)
    {
        GameObject gameObject = CreateObject(parent);
        gameObject.name = objectName;
        return gameObject;
    }

    public static GameObject CreateObject(this Transform parent)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.AttachParentObject(parent);
        return gameObject;
    }
}