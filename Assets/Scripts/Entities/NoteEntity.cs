using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Localization;

namespace DreamedReality.Entities
{
    public class NoteEntity : AbstractUsableEntity
    {
        public override LocalizedString UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private LocalizedString m_usageHintText;
        [Space]
        [SerializeField] private LocalizedString m_noteText;

        protected override void OnUse(GameObject user)
        {
            GameManager.Instance.ReadNote(m_noteText);
        }

        private void Start()
        {
            IsActive = true;
        }
    }
}
