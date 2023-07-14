using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class RotatableDoorEntity : StatefulEntity
    {
        [Space]
        [SerializeField] private RotatableDoorSettings m_settings;
        [SerializeField] private Transform m_doorTransform;

        private Quaternion m_openRotation;
        private Tween m_activeTween = null;

        protected override void Start()
        {
            base.Start();

            m_openRotation = Quaternion.AngleAxis(
                m_settings.rotationAngle, Vector3.up
            );

            if (State)
            {
                m_doorTransform.localRotation = m_openRotation;
            }
            else
            {
                m_doorTransform.localRotation = Quaternion.identity;
            }
        }

        protected override void OnStateChange(bool state)
        {
            m_activeTween?.Kill();

            if (state)
            {
                m_activeTween = m_doorTransform.DOLocalRotateQuaternion(
                    m_openRotation, m_settings.rotationDuration
                );
            }
            else
            {
                m_activeTween = m_doorTransform.DOLocalRotateQuaternion(
                    Quaternion.identity, m_settings.rotationDuration
                );
            }
        }
    }
}
