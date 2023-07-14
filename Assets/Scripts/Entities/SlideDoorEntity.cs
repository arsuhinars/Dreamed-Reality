using DG.Tweening;
using DreamedReality.Scriptables;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class SlideDoorEntity : StatefulEntity
    {
        [Space]
        [SerializeField] private SlideDoorSettings m_settings;
        [SerializeField] private Transform m_doorTransform;

        private Tween m_activeTween = null;

        protected override void Start()
        {
            base.Start();

            if (State)
            {
                m_doorTransform.localPosition = m_settings.slideOffset;
            }
            else
            {
                m_doorTransform.localPosition = Vector3.zero;
            }
        }

        protected override void OnStateChange(bool state)
        {
            m_activeTween?.Kill();

            if (state)
            {
                m_activeTween = m_doorTransform.DOLocalMove(
                    m_settings.slideOffset, m_settings.slideDuration
                );
            }
            else
            {
                m_activeTween = m_doorTransform.DOLocalMove(
                    Vector3.zero, m_settings.slideDuration
                );
            }
        }
    }
}
