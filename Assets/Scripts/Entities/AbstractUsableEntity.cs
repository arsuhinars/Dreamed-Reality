using DreamedReality.Controllers;
using System;
using UnityEngine;

namespace DreamedReality.Entities
{
    public abstract class AbstractUsableEntity : MonoBehaviour
    {
        public event Action<bool> OnStateChange;

        public bool IsActive
        {
            get => m_isActive;
            set
            {
                if (m_isActive != value)
                {
                    m_isActive = value;
                    OnStateChange?.Invoke(value);
                }
            }
        }
        public abstract string UsageHintText { get; }
        public abstract string RequiredItemTag { get; }

        private bool m_isActive;

        public void Use(GameObject user)
        {
            if (!IsActive)
            {
                return;
            }

            if (!string.IsNullOrEmpty(RequiredItemTag))
            {
                if (!user.TryGetComponent<PlayerInventoryController>(out var inventory))
                {
                    return;
                }

                if (!inventory.ReleaseItem(RequiredItemTag))
                {
                    return;
                }
            }

            OnUse(user);
        }

        protected abstract void OnUse(GameObject user);

        private void OnEnable()
        {
            if (IsActive)
            {
                OnStateChange?.Invoke(true);
            }
        }

        private void OnDisable()
        {
            if (IsActive)
            {
                OnStateChange?.Invoke(false);
            }
        }
    }
}
