using DreamedReality.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerController m_player;

        private void Awake()
        {
            m_player = GetComponent<PlayerController>();
        }

        private void Start()
        {
            GameInputManager.Instance.OnMoveActionUpdate += OnMoveActionUpdate;
            GameInputManager.Instance.OnJumpActionDown += OnJumpActionRelease;
        }

        private void OnDestroy()
        {
            if (GameInputManager.Instance != null)
            {
                GameInputManager.Instance.OnMoveActionUpdate -= OnMoveActionUpdate;
                GameInputManager.Instance.OnJumpActionDown -= OnJumpActionRelease;
            }
        }

        private void OnMoveActionUpdate(Vector2 v)
        {
            m_player.Character.MoveVector = v;
        }

        private void OnJumpActionRelease()
        {
            m_player.Character.Jump();
        }
    }
}
