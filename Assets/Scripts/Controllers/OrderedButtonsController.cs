using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Controllers
{
    public class OrderedButtonsController : MonoBehaviour
    {
        [SerializeField] private AbstractUsableEntity[] m_orderedButtons;
        [Space]
        [SerializeField] private UnityEvent m_onActivated;

        private bool m_isActivated;
        private int m_lastBtnIdx;

        private void OnButtonClick(int buttonIdx)
        {
            if (m_isActivated)
            {
                return;
            }

            if (buttonIdx - m_lastBtnIdx != 1)
            {
                m_lastBtnIdx = -1;
                return;
            }

            m_lastBtnIdx++;

            if (m_lastBtnIdx == m_orderedButtons.Length - 1)
            {
                m_isActivated = true;
                m_onActivated?.Invoke();
            }
        }

        private void Start()
        {
            GameManager.Instance.OnStart += OnGameStart;

            for (int i = 0; i < m_orderedButtons.Length; ++i)
            {
                int j = i;
                m_orderedButtons[i].OnUseAction += () =>
                {
                    OnButtonClick(j);
                };
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
            m_isActivated = false;
            m_lastBtnIdx = -1;
        }
    }
}
