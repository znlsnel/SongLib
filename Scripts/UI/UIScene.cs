using DG.Tweening;
using UnityEngine;

namespace SongLib
{
    public class UIScene : UIBase
    {
        public override void Init()
        {
            base.Init();

            foreach (Transform child in transform)
            {
                child.transform.DOKill();
                child.transform.localScale = Vector3.one * 0.9f;
                child.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            }
        }
    }
}