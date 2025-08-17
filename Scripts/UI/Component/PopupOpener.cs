using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupOpener : MonoBehaviour
{
    [SerializeField] private UIPopup _popupPrefab;
    private UIPopup _popup;

    public void OpenPopup()
    {
        if (_popup == null)
        {
            _popup = Instantiate(_popupPrefab);
        }

        Managers.UI.ShowPopupUI(_popup);
    }

    
}
