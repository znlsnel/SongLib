using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SongLib
{
    public abstract class UIPopup : UIBase
    {

        public abstract int GetPopupID();

        protected abstract void OnInitPopup();
        protected abstract void OnRefresh();

        public void Open()
        {

        }

        public void Close()
        {
            
        }

    }


}
