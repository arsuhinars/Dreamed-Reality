using DreamedReality.Managers;
using DreamedReality.Tweeners;
using DreamedReality.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Entities
{
    public class UsableEntity : AbstractUsableEntity
    {
        public override string UsageHintText => m_usageHintText;
        public override string RequiredItemTag => m_requiredItemTag;

        [SerializeField] private bool m_isInitiallyActive = true;
        [SerializeField] private bool m_disableOnUse = false;
        [SerializeField] private string m_usageHintText;
        [SerializeField] private string m_requiredItemTag;
        [Space]
        [SerializeField] private SerializedInterface<ITweener> m_tweener;
        [SerializeField] private SfxType m_usingSfx;
        [Space]
        [SerializeField] private UnityEvent m_onUse;

        protected override void OnUse(GameObject user)
        {
            m_tweener.Value?.Play();
            AudioManager.Instance.PlaySound(m_usingSfx, transform.position);
            m_onUse?.Invoke();

            if (m_disableOnUse)
            {
                IsActive = false;
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
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnGameStart()
        {
            IsActive = m_isInitiallyActive;
        }
    }
}
