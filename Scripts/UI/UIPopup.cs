using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : UIBase
{
    [Header("PopupUI Settings")]
	[SerializeField] private Transform _panel;
    [SerializeField] private List<Button> _closeButtons = new List<Button>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var button in _closeButtons)
            button.onClick.AddListener(Hide);
        
    }


    public void Init()
	{
		Managers.UI.SetCanvas(gameObject); 
	}

	public override void Hide()
    { 
		_panel.transform.DOKill(); 
        canvasGroup.DOFade(0, 0.3f).onComplete += () => {
            base.Hide();  
        };
    }

}
