using DG.Tweening;
using UnityEngine;

public class UIScene : UIBase
{

    public override void Show()
    {
        base.Show();
        
        foreach (Transform child in transform)
        {
            child.transform.DOKill();
            child.transform.localScale = Vector3.one * 0.9f;
            child.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        } 
    }
}
