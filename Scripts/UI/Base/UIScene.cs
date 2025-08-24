using UnityEngine;

namespace SongLib
{
    public abstract class UIScene: UIPanel
    {


        public override void Init()
        {
            base.Init();
            
            // if (_uiContainer == null)
            // {
            //     InitChildUIBase();
            // }
            // else
            // {
            //     _uiContainer.Init();
            // }
        }

        public override void Refresh()
        {
            base.Refresh();
        }

        protected abstract override void OnInit();
        protected abstract override void OnRefresh();
    }
}