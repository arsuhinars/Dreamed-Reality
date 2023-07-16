using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Tweeners
{
    public class ButtonTweener : MonoBehaviour, ITweener
    {
        [SerializeField] private ButtonSettings m_settings;
        [Space]
        [SerializeField] private Transform m_targetTransform;

        private Tween m_activeTween;

        public void Play()
        {
            m_activeTween?.Kill();

            float tweenDuration = m_settings.buttonMoveDuration * 0.5f;

            var sequence = DOTween.Sequence();
            sequence.Append(
                m_targetTransform.DOLocalMove(
                    m_settings.buttonMoveOffset, tweenDuration
                )
            );
            sequence.Append(
                m_targetTransform.DOLocalMove(
                    Vector3.zero, tweenDuration
                )
            );

            m_activeTween = sequence;
        }

        public void Complete()
        {
            m_activeTween?.Complete();
            m_activeTween?.Kill();
            m_activeTween = null;
        }

        private void OnDisable()
        {
            Complete();
        }
    }
}
