using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Controllers
{
    public class SlotsController : MonoBehaviour
    {
        [SerializeField] private SlotEntity[] m_slots;
        [Space]
        [SerializeField] private UnityEvent m_onItemsPut;

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
            m_areItemsPut = false;
        }

        private void CheckSlots()
        {
            if (m_areItemsPut)
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
