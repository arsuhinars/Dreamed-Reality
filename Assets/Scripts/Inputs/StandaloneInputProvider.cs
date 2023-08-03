using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DreamedReality.Inputs
{
    public class StandaloneInputProvider : IInputProvider
    {
        public event Action<Vector2> OnMoveActionUpdate;
        public event Action OnJumpActionDown;
        public event Action OnUseActionRelease;
        public event Action OnPauseActionRelease;

        private InputAction m_moveAction;
        private InputAction m_jumpAction;
        private InputAction m_useAction;
        private InputAction m_pauseAction;

        public StandaloneInputProvider(InputActionAsset inputActions)
        {
            m_moveAction = inputActions.FindAction("Move");
            m_jumpAction = inputActions.FindAction("Jump");
            m_useAction = inputActions.FindAction("Use");
            m_pauseAction = inputActions.FindAction("Pause");

            m_moveAction.performed += HandleMoveAction;
            m_moveAction.canceled += HandleMoveAction;
            m_jumpAction.performed += HandleJumpAction;
            m_useAction.canceled += HandleUseAction;
            m_pauseAction.canceled += HandlePauseAction;
        }

        ~StandaloneInputProvider()
        {
            m_moveAction.performed -= HandleMoveAction;
            m_moveAction.canceled -= HandleMoveAction;
            m_jumpAction.performed -= HandleJumpAction;
            m_useAction.canceled -= HandleUseAction;
            m_pauseAction.canceled -= HandlePauseAction;
        }

        private void HandleMoveAction(InputAction.CallbackContext context)
        {
            OnMoveActionUpdate?.Invoke(context.ReadValue<Vector2>());
        }

        private void HandleJumpAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnJumpActionDown?.Invoke();
            }
        }

        private void HandleUseAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                OnUseActionRelease?.Invoke();
            }
        }

        private void HandlePauseAction(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                OnPauseActionRelease?.Invoke();
            }
        }
    }
}
