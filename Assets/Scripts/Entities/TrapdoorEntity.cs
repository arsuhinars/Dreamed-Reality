using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class TrapdoorEntity : AbstractStatefulEntity
    {
        protected override bool InitialState => m_initialState;

        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_rotateAngles;
        [SerializeField] private float m_fallDuration;
        [SerializeField] private float m_riseDuration;
        [SerializeField] private float m_waitTime;
        [SerializeField] private float m_phaseTime;

        private bool m_wasPaused;
        private float m_pausedPhase;
        private float m_totalTweenTime;
        private Tween m_tween;

        protected override void OnStateChange(bool state, bool isInitial)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (isInitial)
            {
                m_wasPaused = false;
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
            m_totalTweenTime = m_fallDuration + m_riseDuration + m_waitTime * 2f;
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
                    .DOLocalRotate(m_rotateAngles, m_fallDuration)
                    .From(Vector3.zero)
                    .SetEase(Ease.OutBounce)
            );
            sequence.AppendInterval(m_waitTime);
            sequence.Append(
                m_model
                    .DOLocalRotate(Vector3.zero, m_riseDuration)
                    .From(m_rotateAngles)
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
