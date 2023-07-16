using DreamedReality.Entities;
using DreamedReality.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(CharacterEntity))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerUsablesController))]
    [RequireComponent(typeof(PlayerInventoryController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public CharacterEntity Character => m_char;
        public PlayerInventoryController Inventory => m_inventory;

        [SerializeField] private CameraController m_playerCamera;

        private CharacterEntity m_char;
        private PlayerInventoryController m_inventory;
        private PlayerInput m_playerInput;

        private Vector3 m_initialPosition;
        private Quaternion m_initialRotation;

        private InputAction m_pauseAction;

        public void TeleportTo(Vector3 position, Quaternion rotation)
        {
            m_char.TeleportTo(position, rotation);
            m_playerCamera.ResetPosition();
        }

        private void Awake()
        {
            m_char = GetComponent<CharacterEntity>();
            m_inventory = GetComponent<PlayerInventoryController>();
            m_playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            m_initialPosition = transform.position;
            m_initialRotation = transform.rotation;

            m_pauseAction = m_playerInput.actions.FindAction("Pause");
            m_pauseAction.canceled += HandlePauseAction;

            GameManager.Instance.OnStart += OnGameStart;
        }

        private void OnDestroy()
        {
            m_pauseAction.canceled -= HandlePauseAction;

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

        private void OnGameStart()
        {
            m_char.Spawn();
            TeleportTo(m_initialPosition, m_initialRotation);
        }
    }
}
