using DreamedReality.Managers;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class NoteEntity : AbstractUsableEntity
    {
        public override string UsageHintText => m_usageHintText;
        public override string RequiredItemTag => string.Empty;

        [SerializeField] private string m_usageHintText;
        [Space]
        [SerializeField, TextArea(5, 10)] private string m_noteText;

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
