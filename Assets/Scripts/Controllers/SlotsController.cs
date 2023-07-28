using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Controllers
{
    public class SlotsController : MonoBehaviour
    {
        public bool IsActive
        {
            get => m_isActive;
            set => m_isActive = value;
        }

        [SerializeField] private bool m_isInitiallyActive;
        [SerializeField] private SlotEntity[] m_slots;
        [Space]
        [SerializeField] private UnityEvent m_onItemsPut;

        private bool m_isActive;
        private bool m_areItemsPut;
        
        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;

            for (int i = 0; i < m_slots.Length; ++i)
            {
                m_slots[i].OnItemPut += CheckSlots;
            }
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
            m_isActive = m_isInitiallyActive;
            m_areItemsPut = false;
        }

        private void CheckSlots()
        {
            if (!m_isActive || m_areItemsPut)
            {
                return;
            }

            bool areItemsPut = true;
            for (int i = 0; i < m_slots.Length; ++i)
            {
                if (!m_slots[i].IsItemPut)
                {
                    areItemsPut = false;
                    break;
                }
            }

            if (areItemsPut)
            {
                m_areItemsPut = true;
                m_onItemsPut?.Invoke();
            }
        }
    }
}
