using DG.Tweening;
using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class TrapdoorEntity : AbstractStatefulEntity
    {
        [SerializeField] private bool m_initialState;
        [Space]
        [SerializeField] private Transform m_model;
        [SerializeField] private Vector3 m_rotateAngles;
        [SerializeField] private float m_fallDuration;
        [SerializeField] private float m_riseDuration;
        [SerializeField] private float m_waitTime;
        [SerializeField] private float m_phaseTime;

        private float m_pausedPhase;
        private Tween m_tween;

        protected override void OnStateChange(bool state)
        {

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

            if (!Mathf.Approximately(m_pausedPhase, 0f))
            {
                sequence.Goto(m_pausedPhase, true);
                m_pausedPhase = 0f;
            }
            else
            {
                float totalTime = m_fallDuration + m_riseDuration + 2f * m_waitTime;
                sequence.Goto(
                    (GameManager.Instance.GameTime + m_phaseTime) % totalTime, true
                );
            }

            m_tween = sequence;
        }
    }
}
