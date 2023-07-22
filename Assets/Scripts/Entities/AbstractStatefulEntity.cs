using UnityEngine;

namespace DreamedReality.Entities
{
    public abstract class AbstractStatefulEntity : MonoBehaviour
    {
        public bool State
        {
            get => m_state ?? false;
            set
            {
                if (value == m_state)
                {
                    return;
                }

                m_state = value;
                OnStateChange(value);
            }
        }

        private bool? m_state;

        public void ToggleState()
        {
            State = !m_state ?? true;
        }

        protected abstract void OnStateChange(bool state);
    }
}
