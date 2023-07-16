using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Tweeners
{
    public class RotatableDoorTweener : MonoBehaviour, IStatefulTweener
    {
        [SerializeField] private RotatableDoorSettings m_settings;
        [Space]
        [SerializeField] private Transform m_targetTransform;

        private Tween m_activeTween;

        public void PlayIn()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalRotateQuaternion(
                Quaternion.AngleAxis(
                    m_settings.rotationAngle, Vector3.up
                ),
                m_settings.rotationDuration
            );
        }

        public void PlayOut()
        {
            m_activeTween?.Kill();

            m_activeTween = m_targetTransform.DOLocalRotateQuaternion(
                Quaternion.identity, m_settings.rotationDuration
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
