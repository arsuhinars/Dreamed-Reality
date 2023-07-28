using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace DreamedReality.Entities
{
    public class NoteEntity : AbstractUsableEntity
    {
        public override LocalizedString UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private LocalizedString m_usageHintText;
        [SerializeField] private LocalizedString m_noteText;
        [Space]
        [SerializeField] private UnityEvent m_onUse;

        protected override void OnUse(GameObject user)
        {
            GameManager.Instance.ReadNote(m_noteText);
            m_onUse?.Invoke();
        }

        private void Start()
        {
            IsActive = true;
        }
    }
}
