using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SongLib
{
    public class UIPopupTransitionDim : UIPopup, ISceneTransition
    {
        [SerializeField] private Image _dimImg;
        [SerializeField] private float _fadeDuration = 1f;
        
        private Coroutine _transitionCoroutine;

        public override int GetPopupID() => (int)GlobalType.TransitionDim;

        protected override void OnInitPopup() { }
        protected override void OnRefresh() { }

        #region << =========== START TRANSITION =========== >>

        public void StartTransition(Action onComplete = null)
        {
            ClearTransitionCoroutine();
            
            _transitionCoroutine = StartCoroutine(FadeIn(onComplete));
        }

        #endregion

        #region << =========== END TRANSITION =========== >>

        public void EndTransition(Action onComplete = null)
        {
            ClearTransitionCoroutine();
            
            _transitionCoroutine = StartCoroutine(FadeOut(() =>
            {
                onComplete?.Invoke();
            }));
        }

        #endregion

        private void ClearTransitionCoroutine()
        {
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
                _transitionCoroutine = null;
            }
        }

        private IEnumerator FadeIn(Action onComplete)
        {
            float time = 0f;
            Color color = _dimImg.color;
            color.a = 0f;
            _dimImg.color = color;

            while (time < _fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                color.a = Mathf.Clamp01(time / _fadeDuration);
                _dimImg.color = color;
                yield return null;
            }

            color.a = 1f;
            _dimImg.color = color;
            onComplete?.Invoke();
        }

        private IEnumerator FadeOut(Action onComplete)
        {
            float time = 0f;
            Color color = _dimImg.color;
            color.a = 1f;
            _dimImg.color = color;

            while (time < _fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                color.a = 1f - Mathf.Clamp01(time / _fadeDuration);
                _dimImg.color = color;
                yield return null;
            }

            color.a = 0f;
            _dimImg.color = color;
            onComplete?.Invoke();
        }
    }
}