using DreamedReality.Entities;
using DreamedReality.Inputs;
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

        private StandaloneInputProvider m_inputProvider;

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

            m_inputProvider = new StandaloneInputProvider(m_playerInput.actions);
            GameInputManager.Instance.RegisterInputProvider(m_inputProvider);
            GameInputManager.Instance.OnPauseActionRelease += OnPauseActionRelease;

            GameManager.Instance.OnStart += OnGameStart;
            GameManager.Instance.OnEnd += OnGameEnd;
        }

        private void OnDestroy()
        {
            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.OnPauseActionRelease -= OnPauseActionRelease;
                GameInputManager.Instance.UnregisterInputProvider(m_inputProvider);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStart -= OnGameStart;
                GameManager.Instance.OnEnd -= OnGameEnd;
            }
        }

        private void OnPauseActionRelease()
        {
            var gameManager = GameManager.Instance;
            if (gameManager.IsPaused)
            {
                gameManager.ResumeGame();
            }
            else
            {
                gameManager.PauseGame();
            }

            AudioManager.Instance.PlaySound(SfxType.UIClick);
        }

        private void OnGameStart()
        {
            m_char.Spawn();
            m_char.IsFreezed = false;
            TeleportTo(m_initialPosition, m_initialRotation);
        }

        private void OnGameEnd(GameEndReason reason)
        {
            m_char.IsFreezed = true;
        }
    }
}
