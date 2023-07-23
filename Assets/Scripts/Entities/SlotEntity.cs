using DG.Tweening;
using DreamedReality.Managers;
using DreamedReality.Scriptables;
using System;
using UnityEngine;

namespace DreamedReality.Entities
{
    public class SlotEntity : AbstractUsableEntity
    {
        public event Action OnItemPut;

        public bool IsItemPut => m_isItemPut;
        public override string UsageHintText => m_usageHintText;
        public override string RequiredItemTag => m_requiredItemTag;

        [SerializeField] private string m_usageHintText;
        [SerializeField] private string m_requiredItemTag;
        [Space]
        [SerializeField] private GameObject m_itemModel;
        [SerializeField] private SlotSettings m_settings;

        private bool m_isItemPut;
        private Tween m_tween;

        protected override void OnUse(GameObject user)
        {
            if (m_isItemPut)
            {
                return;
            }

            m_itemModel.SetActive(true);

            m_tween?.Kill();
            m_tween = m_itemModel.transform.DOLocalMove(
                m_settings.putItemOffset, m_settings.itemPuttingDuration
            );

            IsActive = false;
            m_isItemPut = true;

            AudioManager.Instance.PlaySound(m_settings.usingSfx, transform.position);
            OnItemPut?.Invoke();
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

        private void OnDisable()
        {
            m_tween?.Complete();
            m_tween?.Kill();
            m_tween = null;
        }

        private void OnGameStart()
        {
            IsActive = true;
            m_isItemPut = false;
            m_itemModel.SetActive(false);
            m_itemModel.transform.localPosition = Vector3.zero;
        }
    }
}
