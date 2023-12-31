using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class MovingPlatformEntity : AbstractStatefulEntity
    {
        protected override bool InitialState => m_initialState;

        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_moveOffset;
        [SerializeField] private float m_moveDuration;
        [SerializeField] private float m_waitTime;
        [SerializeField] private float m_phaseTime;

        private bool m_wasPaused;
        private float m_pausedPhase;
        private float m_totalTweenTime;
        private Tween m_tween;

        protected override void OnStateChange(bool state, bool isInitial)
        {
            if (isInitial)
            {
                m_wasPaused = false;
            }

            if (!isActiveAndEnabled)
            {
                return;
            }

            if (state)
            {
                CreateTween();
            }
            else
            {
                m_wasPaused = true;
                m_pausedPhase = m_tween != null ? m_tween.position : 0f;
                m_tween?.Kill();
                m_tween = null;
            }
        }

        protected override void Start()
        {
            base.Start();
            m_totalTweenTime = 2f * (m_moveDuration + m_waitTime);
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null && State)
            {
                CreateTween();
            }
        }

        private void OnDisable()
        {
            m_tween?.Kill();
            m_tween = null;
        }

        private void CreateTween()
        {
            m_tween?.Kill();

            var sequence = DOTween.Sequence();

            sequence.Append(
                m_model
                    .DOLocalMove(m_moveOffset, m_moveDuration)
                    .From(Vector3.zero)
                    .SetEase(Ease.Linear)
            );
            sequence.AppendInterval(m_waitTime);
            sequence.Append(
                m_model
                    .DOLocalMove(Vector3.zero, m_moveDuration)
                    .From(m_moveOffset)
                    .SetEase(Ease.Linear)
            );
            sequence.AppendInterval(m_waitTime);
            sequence.SetLoops(-1);

            if (m_wasPaused)
            {
                sequence.Goto(m_pausedPhase, true);
                m_wasPaused = false;
            }
            else
            {
                sequence.Goto(
                    (GameManager.Instance.GameTime + m_phaseTime) % m_totalTweenTime, true
                );
            }

            m_tween = sequence;
        }
    }
}
