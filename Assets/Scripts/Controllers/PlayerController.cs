using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(CharacterEntity))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset m_actionAsset;
        [SerializeField] private CameraController m_playerCamera;

        private PlayerInput m_playerInput;
        private CharacterEntity m_char;

        private InputAction m_pauseAction;
        private InputAction m_moveAction;
        private InputAction m_jumpAction;
        private InputAction m_useAction;

        private void Awake()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_char = GetComponent<CharacterEntity>();
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
            if (context.phase == InputActionPhase.Canceled)
            {
                Debug.Log("Use click");
            }
        }

        private void OnGameStart()
        {
            m_char.Spawn();
            m_playerCamera.ResetPosition();
        }
    }
}
