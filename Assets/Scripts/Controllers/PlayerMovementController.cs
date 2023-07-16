using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Controllers
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerController m_player;
        private PlayerInput m_input;
        private InputAction m_moveAction;
        private InputAction m_jumpAction;

        private void Awake()
        {
            m_player = GetComponent<PlayerController>();
            m_input = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            m_moveAction = m_input.actions.FindAction("Move");
            m_moveAction.performed += HandleMoveAction;
            m_moveAction.canceled += HandleMoveAction;

            m_jumpAction = m_input.actions.FindAction("Jump");
            m_jumpAction.performed += HandleJumpAction;
        }

        private void OnDestroy()
        {
            m_moveAction.performed -= HandleMoveAction;
            m_moveAction.canceled -= HandleMoveAction;

            m_jumpAction.performed -= HandleJumpAction;
        }

        private void HandleMoveAction(InputAction.CallbackContext context)
        {
            m_player.Character.MoveVector = context.ReadValue<Vector2>();
        }

        private void HandleJumpAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                m_player.Character.Jump();
            }
        }
    }
}
