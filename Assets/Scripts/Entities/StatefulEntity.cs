using DG.Tweening;
using DreamedReality.Managers;
using DreamedReality.Tweeners;
using DreamedReality.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Entities
{
    public class StatefulEntity : MonoBehaviour
    {
        public bool State
        {
            get => m_state;
            set
            {
                if (value == m_state)
                {
                    return;
                }

                m_state = value;
                OnStateChange(value);

                if (value)
                {
                    m_tweener.Value?.PlayIn();
                }
                else
                {
                    m_tweener.Value?.PlayOut();
                }
            }
        }

        [SerializeField] private bool m_initialState;
        [SerializeField]
        private SerializedInterface<IStatefulTweener> m_tweener;
        [Space]
        [SerializeField] private UnityEvent OnTurnOn;
        [SerializeField] private UnityEvent OnTurnOff;

        private bool m_state;

        public void ToggleState()
        {
            State = !m_state;
        }

        protected virtual void OnStateChange(bool state) { }

        protected virtual void OnStart() { }

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

        private void OnGameStart()
        {
            State = m_initialState;
            m_tweener.Value?.Complete();
            OnStart();
        }
    }
}
