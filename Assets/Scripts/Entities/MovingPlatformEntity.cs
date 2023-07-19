using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class MovingPlatformEntity : AbstractStatefulEntity
    {
        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_moveOffset;
        [SerializeField] private float m_moveDuration;
        [SerializeField] private float m_waitTime;
        [SerializeField] private float m_phaseTime;

        private float m_pausedPhase;
        private Tween m_tween;

        protected override void OnStateChange(bool state)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (state)
            {
                CreateTween();
            }
            else if (m_tween != null)
            {
                m_pausedPhase = m_tween.position;
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
                GameManager.Instance.OnStart += OnGameStart;
            }
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                CreateTween();
            }
        }

        private void OnDisable()
        {
            m_tween?.Kill();
            m_tween = null;
        }

        private void OnGameStart()
        {
            m_pausedPhase = 0f;
            State = m_initialState;
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

            if (!Mathf.Approximately(m_pausedPhase, 0f))
            {
                sequence.Goto(m_pausedPhase, true);
                m_pausedPhase = 0f;
            }
            else
            {
                float totalTime = 2f * (m_moveDuration + m_waitTime);
                sequence.Goto(
                    (GameManager.Instance.GameTime + m_phaseTime) % totalTime, true
                );
            }

            m_tween = sequence;
        }
    }
}
