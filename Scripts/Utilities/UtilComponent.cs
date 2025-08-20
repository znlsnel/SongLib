using UnityEngine;

public static class UtilComponent
{
    public static T GetOrAddComponent<T>(this Component cmp) where T : Component
    {
        if (cmp.gameObject.TryGetComponent(out T component))
        {
            return component;
        }
        return cmp.gameObject.AddComponent<T>();
    }

    public static void AttachParentObject(this Component child, Transform parent, bool isInit = true)
    {
        child.transform.SetParent(parent);
        if (!isInit) return;

        RectTransform component = child.GetComponent<RectTransform>();
        if (component != null)
            component.anchoredPosition = Vector2.zero;
        else
            child.transform.localPosition = Vector3.zero;

        child.transform.localScale = Vector3.one;
        child.transform.localRotation = Quaternion.identity;
    }
}