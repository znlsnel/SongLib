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
}