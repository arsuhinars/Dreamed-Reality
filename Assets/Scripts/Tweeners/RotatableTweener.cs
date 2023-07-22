using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Tweeners
{
    public class RotatableTweener : MonoBehaviour, IStatefulTweener
    {
        [SerializeField] private RotatableTweenerSetting m_settings;
        [Space]
        [SerializeField] private Transform m_targetTransform;

        private Tween m_activeTween;

        public void PlayIn()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalRotate(
                m_settings.rotationAngle,
                m_settings.rotationDuration
            );
        }

        public void PlayOut()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalRotate(
                Vector3.zero, m_settings.rotationDuration
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
