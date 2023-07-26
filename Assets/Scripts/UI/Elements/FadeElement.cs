using DG.Tweening;
using System;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Elements
{
    public class FadeElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<FadeElement, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public UxmlFloatAttributeDescription m_FadeDuration = new()
            {
                name = "fade-duration",
                defaultValue = 1f,
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var fadeElement = ve as FadeElement;
                fadeElement.FadeDuration = m_FadeDuration.GetValueFromBag(bag, cc);
            }
        }

        public float FadeDuration
        {
            get => m_fadeDuration;
            set => m_fadeDuration = value;
        }

        private float m_fadeDuration;
        private Tween m_tween;

        public void FadeIn(Action onComplete=null)
        {
            m_tween?.Kill();
            m_tween = CreateFadeTween(0f, 1f, onComplete);
        }

        public void FadeOut(Action onComplete = null)
        {
            m_tween?.Kill();
            m_tween = CreateFadeTween(1f, 0f, onComplete);
        }

        public void StopFade()
        {
            style.opacity = 0f;
            m_tween?.Kill();
        }

        private float GetOpacity() => style.opacity.value;
        private void SetOpacity(float value) => style.opacity = value;

        private Tween CreateFadeTween(float from, float to, Action onComplete)
        {
            return DOTween.To(
                GetOpacity, SetOpacity, to, m_fadeDuration
            ).From(from).SetEase(Ease.Linear).OnComplete(() =>
                {
                    m_tween = null;
                    onComplete?.Invoke();
                }
            );
        }
    }
}
