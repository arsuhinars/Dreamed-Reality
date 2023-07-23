using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Tweeners
{
    public class MoveableTweener : MonoBehaviour, IStatefulTweener
    {
        [SerializeField] private MovableTweenerSettings m_settings;
        [Space]
        [SerializeField] private Transform m_targetTransform;

        private Tween m_activeTween;

        public void PlayIn()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalMove(
                m_settings.moveOffset, m_settings.moveDuration
            );
        }

        public void PlayOut()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalMove(
                Vector3.zero, m_settings.moveDuration
            );
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
