using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SongLib
{
    public class UIPopup : UIBase
    {
        [Header("PopupUI Settings")]
        [SerializeField] private Transform _panel;
        [SerializeField] private List<Button> _closeButtons = new List<Button>();

        public override void Init()
        {
            base.Init();

            foreach (var button in _closeButtons)
                button.onClick.AddListener(Hide);

        }

        public void Hide()
        {
            
        }

    }
}
