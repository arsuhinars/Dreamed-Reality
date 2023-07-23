using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public abstract class AbstractStatefulEntity : MonoBehaviour
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
                OnStateChange(value, false);
            }
        }
        protected abstract bool InitialState { get; }

        private bool m_state;

        public void ToggleState()
        {
            State = !m_state;
        }

        protected abstract void OnStateChange(bool state, bool isInitial);

        protected virtual void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;
        }

        protected virtual void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        protected virtual void OnGameStart()
        {
            m_state = InitialState;
            OnStateChange(m_state, true);
        }
    }
}
