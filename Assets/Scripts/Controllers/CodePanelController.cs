using DreamedReality.Entities;
using DreamedReality.Managers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DreamedReality.Controllers
{
    public class CodePanelController : MonoBehaviour
    {
        [Serializable]
        private class CodeElement
        {
            public CodeDisplayEntity entity;
            public int requiredCode;
        }

        public bool IsActive
        {
            get => m_isActive;
            set => m_isActive = value;
        }

        [SerializeField] private bool m_isInitiallyActive;
        [SerializeField] private CodeElement[] m_codeElements;
        [Space]
        [SerializeField] private UnityEvent m_onCodeApproved;

        private bool m_isActive;
        private bool m_isCodeApproved;

        public void CheckCode()
        {
            if (!IsActive || m_isCodeApproved)
            {
                return;
            }

            bool isCodeApproved = true;
            for (int i = 0; i < m_codeElements.Length; ++i)
            {
                var el = m_codeElements[i];
                if (el.entity.CurrentCode != el.requiredCode)
                {
                    isCodeApproved = false;
                    break;
                }
            }

            if (isCodeApproved)
            {
                m_isCodeApproved = true;
                m_onCodeApproved?.Invoke();
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
            m_isActive = m_isInitiallyActive;
            m_isCodeApproved = false;
        }
    }
}
