using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Entities
{
    public class UsableEntity : MonoBehaviour
    {
        public bool IsActive
        {
            get => m_isActive;
            set => m_isActive = value;
        }
        public string UsageHintText => m_usageHintText;
        public string RequiredItemTag => m_requiredItemTag;

        [SerializeField] private bool m_isActive = true;
        [SerializeField] private string m_usageHintText;
        [SerializeField] private string m_requiredItemTag;
        [Space]
        [SerializeField] private UnityEvent m_onUse;

        public void Use()
        {
            if (!m_isActive)
            {
                return;
            }

            OnUse();
            m_onUse?.Invoke();
        }

        protected virtual void OnUse() { }
    }
}
