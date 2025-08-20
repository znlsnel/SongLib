using UnityEngine;
using UnityEngine.Serialization;

namespace SongLib
{
    public class UIPopupLoading : UIPopup
    {
        protected float _loadingWaitTime = 3f;
        private float _remainTime = 0.0f;

        public override int GetPopupID() => 4;

        protected override void OnInitPopup() { }
        protected override void OnRefresh() { }

        public override void Open()
        {
            base.Open();
            canvasGroup.alpha = 0.0f;
            _remainTime = _loadingWaitTime;
        }

        private void Update()
        {
            if (canvasGroup.alpha == 1.0) return;
            
            _remainTime -= Time.deltaTime;
            if (_remainTime > 0.0) return;
            
            canvasGroup.alpha = 1f;
        }

        /// <summary>로딩 이미지가 몇초(기본 3초) 뒤에 나옵니다.</summary>
        public void SetWaitTime(float _fTime) => _loadingWaitTime = _fTime;
    }
}   