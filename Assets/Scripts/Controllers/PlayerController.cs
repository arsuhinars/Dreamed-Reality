using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(CharacterEntity))]
    [RequireComponent(typeof(InventoryController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public InventoryController Inventory => m_inventory;

        [SerializeField] private InputActionAsset m_actionAsset;
        [SerializeField] private CameraController m_playerCamera;

        private CharacterEntity m_char;
        private InventoryController m_inventory;
        private PlayerInput m_playerInput;

        private UsableEntity m_activeUsableEntity;

        private InputAction m_pauseAction;
        private InputAction m_moveAction;
        private InputAction m_jumpAction;
        private InputAction m_useAction;

        private void Awake()
        {
            m_char = GetComponent<CharacterEntity>();
            m_inventory = GetComponent<InventoryController>();
            m_playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            m_pauseAction = m_playerInput.actions.FindAction("Pause");
            m_pauseAction.canceled += HandlePauseAction;

            m_moveAction = m_playerInput.actions.FindAction("Move");
            m_moveAction.performed += HandleMoveAction;
            m_moveAction.canceled += HandleMoveAction;

            m_jumpAction = m_playerInput.actions.FindAction("Jump");
            m_jumpAction.performed += HandleJumpAction;

            m_useAction = m_playerInput.actions.FindAction("Use");
            m_useAction.canceled += HandleUseAction;

            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            m_pauseAction.canceled -= HandlePauseAction;
            m_moveAction.performed -= HandleMoveAction;
            m_moveAction.canceled -= HandleMoveAction;
            m_jumpAction.canceled -= HandleJumpAction;
            m_useAction.canceled -= HandleUseAction;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<UsableEntity>(out var usableEntity))
            {
                if (!usableEntity.IsActive)
                {
                    return;
                }

                m_activeUsableEntity = usableEntity;

                string itemTag = usableEntity.RequiredItemTag;
                if (!string.IsNullOrEmpty(itemTag) && !m_inventory.HasItem(itemTag))
                {
                    UIManager.Instance.ShowHint(
                        string.Empty, $"{itemTag} is required"
                    );
                }
                else
                {
                    UIManager.Instance.ShowHint(
                        GetActionDisplayName(m_useAction),
                        usableEntity.UsageHintText
                    );
                }

                return;
            }

            if (other.TryGetComponent<PickableEntity>(out var pickableEntity))
            {
                m_inventory.TryAddItem(pickableEntity);
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (
                m_activeUsableEntity != null &&
                other.gameObject == (m_activeUsableEntity as Component).gameObject
            ) {
                m_activeUsableEntity = null;
                UIManager.Instance.HideHint();
            }
        }

        private void HandlePauseAction(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled)
            {
                return;
            }

            var gameManager = GameManager.Instance;
            if (gameManager.IsPaused)
            {
                gameManager.ResumeGame();
            }
            else
            {
                gameManager.PauseGame();
            }
        }

        private void HandleMoveAction(InputAction.CallbackContext context)
        {
            m_char.MoveVector = context.ReadValue<Vector2>();
        }

        private void HandleJumpAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                m_char.Jump();
            }
        }

        private void HandleUseAction(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Canceled || m_activeUsableEntity == null)
            {
                return;
            }

            string itemTag = m_activeUsableEntity.RequiredItemTag;
            if (string.IsNullOrEmpty(itemTag) || m_inventory.HasItem(itemTag))
            {
                m_inventory.ReleaseItem(itemTag);
                m_activeUsableEntity.Use();
            }
        }

        private void OnGameStart()
        {
            m_char.Spawn();
            m_playerCamera.ResetPosition();
        }

        private string GetActionDisplayName(InputAction action)
        {
            int binding = action.GetBindingIndex(group: m_playerInput.currentControlScheme);
            var displayName = action.bindings[binding].ToDisplayString(
                out var deviceLayoutName, out var controlPath
            );

            return displayName;
        }
    }
}
