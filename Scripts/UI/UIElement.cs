using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement<T> : UIBase where T : UIBase
{
    private T _parent;
    public void Setup(T parent)
    {
        _parent = parent;
    }
}
