using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class MovableEntity : AbstractStatefulEntity
    {
        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_pushOffset;
        [SerializeField] private float m_moveDuration;

        private Tween m_tween;

        protected override void OnStateChange(bool state)
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

            if (!isActiveAndEnabled)
            {
                m_tween.Complete();
                m_tween.Kill();
                m_tween = null;
            }
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnDisable()
        {
            m_tween?.Kill();
            m_tween = null;
        }

        private void OnGameStart()
        {
            State = m_initialState;
            m_tween.Complete();
            m_tween.Kill();
            m_tween = null;
        }
    }
}
