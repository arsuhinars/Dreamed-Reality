using UnityEngine;

namespace DreamedReality.Entities
{
    public class StatefulEntity : MonoBehaviour
    {
        public bool State
        {
            get => m_state;
            set
            {
                if (value != m_state)
                {
                    m_state = value;
                    OnStateChange(value);
                }
            }
        }

        [SerializeField] private bool m_initialState;

        private bool m_state;

        public void ToggleState()
        {
            State = !m_state;
        }

        protected virtual void Start()
        {
            State = m_initialState;
        }

        protected virtual void OnStateChange(bool state) { }
    }
}
