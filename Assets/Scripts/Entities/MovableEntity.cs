using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class MovableEntity : AbstractStatefulEntity
    {
        protected override bool InitialState => m_initialState;

        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_pushOffset;
        [SerializeField] private float m_moveDuration;
        [SerializeField] private SfxType m_moveSfx;

        private Tween m_tween;

        protected override void OnStateChange(bool state, bool isInitial)
        {
            m_tween?.Kill();

            if (state)
            {
                m_tween = m_model.DOLocalMove(m_pushOffset, m_moveDuration);
            }
            else
            {
                m_tween = m_model.DOLocalMove(Vector3.zero, m_moveDuration);
            }

            if (!isInitial)
            {
                AudioManager.Instance.PlaySound(m_moveSfx, transform.position);
            }

            if (!isActiveAndEnabled || isInitial)
            {
                m_tween.Complete();
                m_tween.Kill();
                m_tween = null;
            }
        }

        private void OnDisable()
        {
            m_tween?.Kill();
            m_tween = null;
        }
    }
}
