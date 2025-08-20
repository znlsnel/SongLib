using UnityEngine;


public static class UtilTransform
{

        
    public static GameObject CreateObject(this Transform parent, string objectName)
    {
        GameObject gameObject = CreateObject(parent);
        gameObject.name = objectName;
        return gameObject;
    }

    public static GameObject CreateObject(this Transform parent)
    {
        GameObject gameObject = new GameObject();
        gameObject.AttachParentObject(parent);
        return gameObject;
    }
}