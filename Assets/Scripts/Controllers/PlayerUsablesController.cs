﻿using DreamedReality.Entities;
using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerUsablesController : MonoBehaviour
    {
        [SerializeField] private LocalizedString m_requiredItemText;
        [SerializeField] private VariablesGroupAsset m_itemsNamesTable;

        private PlayerController m_player;
        private PlayerInput m_input;
        private InputAction m_useAction;

        private AbstractUsableEntity m_usableEntity = null;

        private void Awake()
        {
            m_player = GetComponent<PlayerController>();
            m_input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            m_useAction = m_input.actions.FindAction("Use");
            m_useAction.canceled += HandleUseAction;

            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            m_useAction.canceled -= HandleUseAction;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AbstractUsableEntity>(out var entity))
            {
                ClearCurrentUsableEntity();
                SetCurrentUsableEntity(entity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (m_usableEntity != null && other.gameObject == m_usableEntity.gameObject)
            {
                ClearCurrentUsableEntity();
            }
        }

        private void HandleUseAction(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled || m_usableEntity == null)
            {
                return;
            }

            TryUseCurrentUsableEntity();
        }

        private void SetCurrentUsableEntity(AbstractUsableEntity entity)
        {
            if (!entity.IsActive)
            {
                return;
            }

            m_usableEntity = entity;
            m_usableEntity.OnStateChange += OnUsableEntityStateChange;

            var uiHint = UIManager.Instance.UIHint;
            var itemTag = entity.RequiredItemTag;
            if (!string.IsNullOrEmpty(itemTag) && !m_player.Inventory.HasItem(itemTag))
            {
                uiHint.PromptIcon = UIHint.PromptIconType.None;

                if (m_itemsNamesTable.TryGetValue(itemTag, out var itemName))
                {
                    m_requiredItemText["itemName"] = itemName;
                    uiHint.Text = m_requiredItemText.GetLocalizedString();
                }
                else
                {
                    Debug.LogError($"Unable to find name for {itemTag} item tag");
                }
            }
            else
            {
                uiHint.PromptIcon = UIHint.PromptIconType.UsePrompt;
                uiHint.Text = entity.UsageHintText.GetLocalizedString();
            }

            uiHint.Show();
        }

        private void ClearCurrentUsableEntity()
        {
            if (m_usableEntity != null)
            {
                m_usableEntity.OnStateChange -= OnUsableEntityStateChange;
                m_usableEntity = null;

                UIManager.Instance.UIHint.Hide();
            }
        }

        private void TryUseCurrentUsableEntity()
        {
            if (m_usableEntity == null)
            {
                return;
            }

            m_usableEntity.Use(gameObject);
        }

        private void OnUsableEntityStateChange(bool state)
        {
            if (!state)
            {
                ClearCurrentUsableEntity();
            }
        }

        private void OnGameStart()
        {
            ClearCurrentUsableEntity();
        }
    }
}
